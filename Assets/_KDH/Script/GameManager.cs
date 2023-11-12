using CCGCard;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public enum GamePhase
{
    None,
    DrawPhase,
    ActionPhase,
    BattlePhase,
    ExecutionPhase,
    EndPhase,
}

public class GameManager : MonoBehaviour
{
    #region Event
    public event Action TurnStart;
    #endregion
    #region public member
    public static GameManager Instance;
    public bool startFirst;
    public bool isGameEnd;
    public TMP_Text turnText;
    public PhotonView photonView;
    public bool playerEnd
    {
        get
        {
            return _playerEnd;
        }
        set
        {
            _playerEnd = value;
            if (_playerEnd && (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase))
            {
                photonView.RPC("CallActionEnd", RpcTarget.Others);
            }
            CheckPhaseEnd();
        }
    }
    public bool enemyEnd
    {
        get
        {
            return _enemyEnd;
        }
        set
        {
            _enemyEnd = value;
            CheckPhaseEnd();
        }
    }
    public bool canAct
    {
        get
        {
            return _canAct;
        }
        set
        {
            _canAct = value;
            if (_canAct == false && (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase))
            {
                currentTurn++;
                photonView.RPC("MatchTurnNum", RpcTarget.Others, currentTurn);
                photonView.RPC("CallSummonEnd", RpcTarget.Others);
            }
            else if (canAct == true && gamePhase == GamePhase.ActionPhase)
            {
                TurnStart?.Invoke();
            }
        }
    }
    public string playerID
    {
        get
        {
            return _playerID;
        }
        set
        {
            _playerID = value;
        }
    }
    public int currentTurn
    {
        get
        {
            return _currentTurn;
        }
        set
        {
            _currentTurn = value;
            if (gamePhase == GamePhase.ActionPhase)
            {
                turnText.text = "Action Turn : " + (_currentTurn + 1).ToString();
            }
            else if (gamePhase == GamePhase.BattlePhase)
            {
                turnText.text = "Battle Turn : " + (_currentTurn + 1).ToString();
            }
        }
    }
    public GamePhase gamePhase
    {
        get
        {
            return _gamePhase;
        }
        private set 
        {
            _gamePhase = value;
        }
    }
    #endregion

    #region SerializeField Members
    [SerializeField] GameObject endScene;
    [SerializeField] Image personalColor;
    #endregion

    #region private Members
    private bool gameStart;
    [SerializeField] private bool _playerEnd;
    [SerializeField] private bool _enemyEnd;
    [SerializeField] private bool _canAct;
    [SerializeField] private string _playerID = "-1";
    [SerializeField] private int _currentTurn;
    [SerializeField] private GamePhase _gamePhase;
    private Deck deck;
    #endregion


    private GameObject tmpField = null;

    private void CheckPhaseEnd()
    {
        if (playerEnd && enemyEnd && gamePhase != GamePhase.BattlePhase)
        {
            EndPhase();
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        startFirst = NetworkManager.instance.first;
        NetworkManager.instance.SetPlayerID();
        ResetState();
        personalColor.color = new Color(255 - (255 * int.Parse(playerID)), 255 * int.Parse(playerID), 0);
    }

    private void ResetState()
    {
        _playerEnd = false;
        _enemyEnd = false;
    }



    public void MulliganHandCard()
    {
        gameStart = true;
        List<Card> handList = new();
        for (int i = 0; i < HandManager.Instance.hands.Count; i++)
        {
            handList.Add(HandManager.Instance.hands[i].card);
        }
        UIManager.Instance.PopupCard(handList);
    }

    private void SummonHero()
    {
        if (int.Parse(playerID) % 2 == 0)
        {
            FieldManager.Instance.PlaceCard(FieldManager.Instance.battleFields.First, deck.myHero, int.Parse(playerID), false);
        }
        else
        {
            FieldManager.Instance.PlaceCard(FieldManager.Instance.battleFields.Last, deck.myHero, int.Parse(playerID), true);
        }
        photonView.RPC("SummonHeroForPun", RpcTarget.Others, int.Parse(playerID), deck.myHero.cardID);
    }

    [PunRPC]
    public void SummonHeroForPun(int id, int cardID)
    {
        if (id % 2 == 0)
        {
            FieldManager.Instance.PlaceCard(FieldManager.Instance.battleFields.First, CardDB.instance.FindCardFromID(cardID), id, false);
        }
        else
        {
            FieldManager.Instance.PlaceCard(FieldManager.Instance.battleFields.Last, CardDB.instance.FindCardFromID(cardID), id, true);
        }
    }

    [PunRPC]
    public void EnemyCardChange(int num)
    {
        FieldManager.Instance.enemyCardNum = num;
    }

    public void EndPhase()
    {
        ResetState();
        switch (gamePhase)
        {
            case GamePhase.DrawPhase:
                if (startFirst)
                {
                    canAct = true;
                }
                else
                {
                    canAct = false;
                }
                currentTurn = 0;
                gamePhase = GamePhase.ActionPhase;
                break;
            case GamePhase.ActionPhase:
                currentTurn = 0;
                ResetState();
                gamePhase = GamePhase.BattlePhase;
                //if (startFirst)
                //{
                //    Debug.Log("StartFirst Can Act");
                //    _canAct = true;
                //}
                //else
                //{
                //    _canAct = false;
                //}
                BattleManager.instance.AttackButton();
                break;
            case GamePhase.BattlePhase:
                gamePhase = GamePhase.ExecutionPhase;
                ExecuteGame();
                break;
            case GamePhase.ExecutionPhase:
                gamePhase = GamePhase.EndPhase;
                EndGame();
                break;
            case GamePhase.EndPhase:
                gamePhase = GamePhase.DrawPhase;
                break;
        }
        turnText.text = gamePhase.ToString();
    }

    [PunRPC]
    public void AttackStartForPun(Vector3 fieldPos)
    {
        Field tempFIeld = null;
        foreach (Field field in BattleManager.instance.battleList)
        {
            if (field.transform.position == fieldPos)
            {
                tempFIeld = field;
                break;
            }
        }
        if (tempFIeld != null)
        {
            tempFIeld.unitObject.cardData.AttackStart(FieldManager.Instance.battleFields, tempFIeld);
            StartCoroutine(BattleManager.instance.AttackProcess(tempFIeld));
        }
        else
        {
            Debug.LogError("정상적이지 않은 필드를 공격시키려 했습니다.");
        }
    }

    private void EndGame()
    {
        Field tmp = FieldManager.Instance.battleFields.First;
        while (tmp != null)
        {
            if (tmp.card.cardCategory != CardCategory.hero)
            {
                if (tmp.unitObject.playerID == playerID)
                {
                    deck.Refill(tmp.unitObject.cardData);
                    tmp.ResetField();
                }
                else
                {
                    tmp.ResetField();
                }
            }
            tmp = tmp.Next;
        }
        Field tmp1 = BattleManager.instance.unitList[0];
        Field tmp2 = BattleManager.instance.unitList[1];

        BattleManager.instance.unitList.Clear();

        BattleManager.instance.unitList.Add(tmp1);
        BattleManager.instance.unitList.Add(tmp2);

        FieldManager.Instance.ResetAllField();
        EndPhase();
    }

    private void ExecuteGame()
    {
        Debug.LogError("처리 페이즈에 진입했습니다.");
        if (BattleManager.instance.damageSum == 0)
        {
            Debug.LogError("버릴 카드가 없습니다.");
            playerEnd = true;
        }
        StartCoroutine(DiscardByDamage());
    }

    IEnumerator DiscardByDamage()
    {
        Debug.LogError($"{BattleManager.instance.damageSum}장 버려야 합니다.");
        UIManager.Instance.PopupCard(deck.useDeck);
        UIManager.Instance.selectCardChanged += Discard;
        while (BattleManager.instance.damageSum > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        UIManager.Instance.selectCardChanged -= Discard;
        UIManager.Instance.ClosePopup();
        Debug.LogError("카드를 전부 버렸습니다");
        playerEnd = true;
    }

    public void Discard(Card card)
    {
        if (deck.RemoveDeckCard(card))
        {
            BattleManager.instance.damageSum--;
            UIManager.Instance.RemoveSelectObject();
        }
    }


    [PunRPC]
    public void MakeFieldAndSetCardForPun(Vector2 mousePos, Vector2 fieldPos, bool isLeft, int cardID, int playerID, bool lookLeft)
    {
        Debug.Log("MAKE");
        SelectFieldForPun(mousePos, fieldPos, isLeft);
        Card summonCard = CardDB.instance.FindCardFromID(cardID);
        FieldManager.Instance.PlaceCard(tmpField.GetComponent<Field>(), summonCard, playerID, lookLeft);
    }


    [PunRPC]
    public void PlaceCardForPun(Vector2 pos, int cardID, int playerID, bool lookLeft)
    {

        Collider2D[] colliders = Physics2D.OverlapPointAll(pos);
        foreach (Collider2D collider in colliders)
        {

            Field field = collider.gameObject.GetComponent<Field>();
            if ((Vector2)field.transform.position == pos)
            {
                if (cardID != 0)
                {

                    Card summonCard = CardDB.instance.FindCardFromID(cardID);
                    FieldManager.Instance.PlaceCard(collider.GetComponent<Field>(), summonCard, playerID, lookLeft);
                    return;
                }
            }
        }
    }

    [PunRPC]
    public void SelectFieldForPun(Vector2 mousePos, Vector2 pos, bool temp)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
        foreach (Collider2D collider in colliders)
        {
            Field field = collider.gameObject.GetComponent<Field>();
            if (field.isEmpty)
            {
                tmpField = field.gameObject;
                return;
            }
            if (temp == mousePos.x <= collider.transform.position.x)
            {
                if (temp)
                {
                    Debug.LogError(field.name + "좌측" + (temp ? "좌측" : "우측"));
                    GameObject newField = Instantiate(FieldManager.Instance.FieldPrefab, pos, Quaternion.identity);
                    FieldManager.Instance.newFields.Add(newField);
                    FieldManager.Instance.battleFields.AddBefore(field, newField);
                    Field tmp = FieldManager.Instance.battleFields.First;
                    FieldManager.Instance.fields.Clear();

                    while (tmp != null)
                    {
                        FieldManager.Instance.fields.Add(tmp.gameObject);
                        tmp = tmp.Next;
                    }
                    for (int posit = (FieldManager.Instance.fields.Count - 1) * -9, i = 0; i < FieldManager.Instance.fields.Count; posit += 18, i++)
                    {
                        try
                        {
                            FieldManager.Instance.fields[i].transform.position = new Vector3(posit, 0, 0);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e.Message);
                            break;
                        }
                    }
                    tmpField = newField;
                    Debug.LogError(tmpField.name);
                    return;
                }
                else
                {
                    Debug.LogError("우측" + (temp ? "좌측" : "우측"));
                    GameObject newField = Instantiate(FieldManager.Instance.FieldPrefab, pos, Quaternion.identity);
                    FieldManager.Instance.newFields.Add(newField);
                    FieldManager.Instance.battleFields.AddAfter(field, newField);
                    Field tmp = FieldManager.Instance.battleFields.First;
                    FieldManager.Instance.fields.Clear();
                    while (tmp != null)
                    {
                        FieldManager.Instance.fields.Add(tmp.gameObject);
                        tmp = tmp.Next;
                    }
                    for (int posit = (FieldManager.Instance.fields.Count - 1) * -9, i = 0; i < FieldManager.Instance.fields.Count; posit += 18, i++)
                    {
                        try
                        {
                            FieldManager.Instance.fields[i].transform.position = new Vector3(posit, 0, 0);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                            break;
                        }
                    }
                    tmpField = newField;
                    Debug.LogError(tmpField.name);
                    return;
                }

            }
        }
    }

    [PunRPC]
    public void AttackPhaseNetwork()
    {
        BattleManager.instance.AttackPhase();
    }

    [PunRPC]
    public void MatchTurnNum(int turnNum)
    {
        currentTurn = turnNum;
    }

    [PunRPC]
    public void CallActionEnd()
    {
        enemyEnd = true;
    }

    [PunRPC]
    public void CallSummonEnd() 
    {
        if (playerEnd)
        {
            canAct = false;
        }
        else
        {
            canAct = true;
        }
    }

    [PunRPC]
    public void GameSet()
    {
        if (isGameEnd == false)
        {
            WinnerProcess();
        }
    }

    private void LoserProcess()
    {
        if (isGameEnd == false)
        {
            isGameEnd = true;
            StopAllCoroutines();
            endScene.SetActive(true);
            endScene.GetComponentInChildren<TMP_Text>().text = "Lose";
        }
    }

    private void WinnerProcess()
    {
        if (isGameEnd == false)
        {
            isGameEnd = true;
            StopAllCoroutines();
            endScene.SetActive(true);
            endScene.GetComponentInChildren<TMP_Text>().text = "WIN";
        }
    }

    public void Lose()
    {
        if (isGameEnd == false)
        {
            photonView.RPC("GameSet", RpcTarget.Others);
            LoserProcess();
        }
    }

    public void ExitGame()
    {
        if (isGameEnd)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    /// <summary>
    /// Function for check animation is end with async
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="aniName"></param>
    /// <returns>return when animation is end</returns>
    public async Task CheckAnim(Animator animator, string aniName)
    {
        if (animator == null)
        {
            Debug.LogError("죽은 유닛 입니다");
            return;
        }
        while (animator != null && animator.runtimeAnimatorController != null && !animator.GetCurrentAnimatorStateInfo(0).IsName(aniName))
        {
            await Task.Delay((int)(Time.deltaTime * 1000));
        }
        while (animator != null && animator.runtimeAnimatorController != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            await Task.Delay((int)(Time.deltaTime * 1000));
        }
    }

    /// <summary>
    /// Function for setting when game is start
    /// </summary>
    public void StartGameSetting()
    {
        StartUISetting();
        StartComponentSetting();
        StartGame();
    }

    public void StartSetting()
    {
        if (deck == null)
        {
            deck = GetComponent<Deck>();
            deck.Shuffle();
            FieldManager.Instance.ResetAllField();
            SummonHero();
        }

        if (gamePhase == GamePhase.DrawPhase)
        {
            //deck.Shuffle();
            deck.Draw(4);
            photonView.RPC("EnemyCardChange", RpcTarget.Others, 4);
            playerEnd = true;
        }
        else
        {
            Debug.Log("드로우 페이즈에만 작동합니다.");
        }

        if (gameStart == false)
        {
            gameStart = true;
        }
    }

    /// <summary>
    /// Funcion for UI Setting when game is start
    /// </summary>
    private void StartUISetting()
    {
        return;
    }

    /// <summary>
    /// Function for Set Components
    /// </summary>
    private void StartComponentSetting()
    {
        if(deck == null)
        {
            deck = GetComponent<Deck>();
        }
        return;
    }

    private void StartGame()
    {
        return;
    }
}

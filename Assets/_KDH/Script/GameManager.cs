using CCGCard;
using JetBrains.Annotations;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public enum GamePhase
{
    DrawPhase,
    ActionPhase,
    BattlePhase,
    ExecutionPhase,
    EndPhase,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_Text text;
    public TMP_Text turnText;
    [SerializeField] GameObject endScene;
    public bool isGameEnd = false;

    //2인 플레이 테스트 용 임시 코드
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

    [SerializeField] private string _playerID = "-1";

    public PhotonView photonView;
    //End

    public GamePhase gamePhase
    {
        get
        {
            return _gamePhase;
        }
        private set { _gamePhase = value; }
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
                turnText.text = "Turn : " + _currentTurn.ToString();
            }
        }
    }

    public bool playerEnd
    {
        get
        {
            return _playerEnd;
        }
        set
        {
            _playerEnd = value;
            if (_playerEnd)
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
            if (_canAct == false && gamePhase == GamePhase.ActionPhase)
            {
                currentTurn++;
                photonView.RPC("MatchTurnNum", RpcTarget.Others, currentTurn);
                photonView.RPC("CallSummonEnd", RpcTarget.Others);
            }
        }
    }

    public bool startFirst = false;

    [SerializeField] private bool _canAct;

    private void CheckPhaseEnd()
    {
        if (playerEnd && enemyEnd)
        {
            EndPhase();
        }
    }

    [SerializeField] private int _currentTurn;

    [SerializeField] private bool _playerEnd;

    [SerializeField] private bool _enemyEnd;

    private Deck deck;

    [SerializeField] private GamePhase _gamePhase;

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
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            text.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
    }

    private void ResetState()
    {
        playerEnd = false;
        enemyEnd = false;
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
    }

    private void SummonHero()
    {
        if(int.Parse(playerID)%2 == 0)
        {
            FieldManager.Instance.PlaceCard(FieldManager.Instance.battleFields.First,deck.myHero,int.Parse(playerID),false);
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
                //가위바위보 결과로 대체 할 코드
                //if (int.Parse(playerID) % 2 == 0)
                //{
                //    canAct = true;
                //}
                //else
                //{
                //    canAct = false;
                //}
                //
                currentTurn = 0;
                gamePhase = GamePhase.ActionPhase;
                break;
            case GamePhase.ActionPhase:
                gamePhase = GamePhase.BattlePhase;
                BattleManager.instance.AttackButton();
                break;
            case GamePhase.BattlePhase:
                ResetState();
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
        BattleManager.instance.unitList.Clear();
        FieldManager.Instance.ResetAllField();
        EndPhase();
    }

    private void ExecuteGame()
    {
        Debug.LogError("처리 페이즈에 진입했습니다.");
        StartCoroutine(DiscardByDamage());
    }

    IEnumerator DiscardByDamage()
    {
        Debug.LogError($"{BattleManager.instance.damageSum}장 버려야 합니다.");
        UIManager.Instance.PopupCard(deck.useDeck);
        UIManager.Instance.selectCardChanged += Discard;
        while(BattleManager.instance.damageSum > 0)
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
    public void PlaceCardForPun(Vector2 pos, int cardID, int playerID, bool lookLeft)
    {

        Vector2 mousePos = pos;
        //FieldManager.Instance.instantiatePosition = new Vector3(mousePos.x, 0, 0);
        RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (cardID != 0)
        {
            Card summonCard = CardDB.instance.FindCardFromID(cardID);
            FieldManager.Instance.PlaceCard(rayhit.collider.GetComponent<Field>(), summonCard, playerID, lookLeft);
        }
    }

    /// <summary>
    /// Creat Tile By Pun
    /// </summary>
    /// <param name="mousePos"></param>
    /// <param name="pos"></param>
    [PunRPC]
    public void SelectFieldForPun(Vector2 mousePos, Vector2 pos, bool temp)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(mousePos);
        foreach (Collider2D collider in colliders)
        {
            if (temp == mousePos.x <= collider.transform.position.x)
            {
                Field field = collider.gameObject.GetComponent<Field>();
                if (temp)
                {
                    Debug.LogError("좌측" + (temp ? "좌측" : "우측"));
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
                    return;
                }

            }
            //if (collider.gameObject.layer == 7)
            //{

            //    GameObject newField = Instantiate(FieldManager.Instance.FieldPrefab, pos, Quaternion.identity);
            //    FieldManager.Instance.battleFields.AddBefore(field, newField);
            //    Field tmp = FieldManager.Instance.battleFields.First;
            //    FieldManager.Instance.fields.Clear();
            //    while (tmp != null)
            //    {
            //        FieldManager.Instance.fields.Add(tmp.gameObject);
            //        tmp = tmp.Next;
            //    }
            //    for (int posit = (FieldManager.Instance.fields.Count - 1) * -9, i = 0; i < FieldManager.Instance.fields.Count; posit += 18, i++)
            //    {
            //        try
            //        {
            //            FieldManager.Instance.fields[i].transform.position = new Vector3(posit, 0, 0);
            //        }
            //        catch (Exception e)
            //        {
            //            Debug.LogError(e.Message);
            //            break;
            //        }
            //    }
            //    return;
            //}
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
            photonView.RPC("CallSummonEnd", RpcTarget.Others);
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

    public async System.Threading.Tasks.Task CheckAnim(Animator animator, string aniName)
    {
        if (animator == null)
        {
            Debug.LogError("죽은 유닛 입니다");
            return;
        }
        while (animator != null && animator.runtimeAnimatorController != null && !animator.GetCurrentAnimatorStateInfo(0).IsName(aniName))
        {
            await System.Threading.Tasks.Task.Delay((int)(Time.deltaTime * 1000));
        }
        while (animator != null && animator.runtimeAnimatorController != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            await System.Threading.Tasks.Task.Delay((int)(Time.deltaTime * 1000));
        }
    }
}

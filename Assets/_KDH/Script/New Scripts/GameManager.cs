using CCGCard;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public static GameManager instance;

    public event Action CallTurnStart;

    public bool canAct
    {
        get
        {
            return _canAct;
        }
        set
        {
            if (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase)
            {
                if (canAct != value)
                {
                    if (value)
                    {
                        CallTurnStart?.Invoke();
                        useCard = false;
                    }
                    else
                    {
                        currentTurn++;
                        photonView.RPC("CallPlayerTurnEnd", RpcTarget.Others);
                    }
                }
            }
            _canAct = value;
            Debug.LogError($"CanAct is : {canAct}");
        }
    }

    public bool useCard = false;
    public bool startFirst;
    public bool isGameEnd;
    [SerializeField] TMP_Text turnText;
    [SerializeField] private bool _canAct;
    public PhotonView photonView;
    public Deck deck;
    private bool phaseTrigger;

    public int damageSum = 0;

    public string playerID;

    public GamePhase gamePhase
    {
        get
        {
            return _gamePhase;
        }
        set
        {
            if (_gamePhase != value)
                _gamePhase = value;
            if (gamePhase != GamePhase.None)
            {
                StartPhaseSetting();
            }
        }
    }

    [SerializeField] private GamePhase _gamePhase = GamePhase.None;
    private GamePhase nextPhase = GamePhase.DrawPhase;

    public Image personalColor;

    public bool playerEnd
    {
        get
        {
            return _playerEnd;
        }
        set
        {
            if (gamePhase != GamePhase.None)
            {

                if (playerEnd == false && value == true)
                {
                    photonView.RPC("CallPlayerPhaseEnd", RpcTarget.Others);
                    nextPhase = gamePhase + 1;
                    gamePhase = GamePhase.None;

                }
            }
            _playerEnd = value;
            Debug.LogError($"PlayerEnd is : {playerEnd}, Call CheckPhaseEnd");
            StartCoroutine(CheckPhaseEnd());
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
            Debug.LogError($"EnemyEnd is : {enemyEnd}");
            if (gamePhase == GamePhase.None)
            {
                Debug.LogError("EnemyEnd! Call CheckPhaseEnd");
                StartCoroutine(CheckPhaseEnd());
            }
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
            if (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase)
            {
                turnText.text = gamePhase.ToString() + " : " + currentTurn;
            }
        }
    }
    private int _currentTurn;

    [SerializeField] private bool _playerEnd;
    [SerializeField] private bool _enemyEnd;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        NetworkManager.instance.SetPlayerID();
        startFirst = NetworkManager.instance.first;
        Debug.LogError(startFirst);
        personalColor.color = new Color(255 - (255 * int.Parse(playerID)), 255 * int.Parse(playerID), 0);
        Invoke("FirstTurnSetting", 3f);
        CallTurnStart += TurnStart;
    }

    IEnumerator CheckPhaseEnd()
    {
        Debug.LogError($"PlayerEnd is : {playerEnd}, EnemyEnd is : {enemyEnd}");
        if (playerEnd && enemyEnd)
        {
            photonView.RPC("CallCheckPhaseEnd", RpcTarget.Others);
            int count = 0;
            while (phaseTrigger == false)
            {
                if (count % 9 == 0)
                {
                    photonView.RPC("CallPlayerPhaseEnd", RpcTarget.Others);
                }
                turnText.text = "Waiting Please";
                yield return new WaitForSeconds(1);
                count++;
            }
            _playerEnd = false;
            _enemyEnd = false;
            Debug.LogError(nextPhase);
            gamePhase = nextPhase;
            turnText.text = gamePhase.ToString();
            currentTurn = 0;
            phaseTrigger = false;
        }
        yield return null;
    }

    private void StartPhaseSetting()
    {
        switch (gamePhase)
        {
            case GamePhase.DrawPhase:
                Invoke("DrawPhaseStart", 3);
                break;
            case GamePhase.ActionPhase:
                Invoke("ActionPhaseStart", 3);
                break;
            case GamePhase.BattlePhase:
                Invoke("BattlePhaseStart", 3);
                break;
            case GamePhase.ExecutionPhase:
                Invoke("ExecutionPhaseStart", 3);
                break;
            case GamePhase.EndPhase:
                Invoke("EndPhaseStart", 3);
                break;
            default:
                Debug.LogError("Error is Occur in Phase Change");
                break;
        }
    }

    private void DrawPhaseStart()
    {
        deck.Draw(5);
        playerEnd = true;
    }

    private void ActionPhaseStart()
    {
        if (startFirst)
        {
            _canAct = true;
        }
        else
        {
            _canAct = false;
        }
    }

    private void BattlePhaseStart()
    {
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            if(temp.isEmpty == false)
            {
                temp.canBattle = true;
            }
            else
            {
                temp.canBattle = false;
            }
            temp = temp.Next;
        }

        CheckCanBattle();

        if (startFirst)
        {
            _canAct = true;
        }
        else
        {
            _canAct = false;
        }
    }

    private void CheckCanBattle()
    {
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            if (temp.isEmpty)
            {
                temp.canBattle = false;
            }
            else
            {
                if (temp.cardData.cardCategory == CardCategory.hero)
                {
                    if (((Hero)temp.cardData).canAttack == false)
                    {
                        temp.canBattle = false;
                    }
                    else
                    {
                        temp.canBattle = true;
                    }
                }
                else
                {
                    
                }
            }
            temp = temp.Next;
        }
    }

    private void ExecutionPhaseStart()
    {
        Debug.LogError("처리 페이즈에 진입했습니다.");
        if (damageSum == 0)
        {
            Debug.LogError("버릴 카드가 없습니다.");
            playerEnd = true;
            return;
        }
        StartCoroutine(DiscardByDamage());
    }

    private void EndPhaseStart()
    {
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            if (temp.playerID != -1)
            {
                if (temp.cardData.cardCategory != CardCategory.hero)
                {
                    if (temp.playerID == int.Parse(playerID))
                    {
                        deck.Refill(temp.cardData);
                        temp.cardData = null;
                    }
                    else
                    {
                        temp.cardData = null;
                    }
                }
            }
            temp = temp.Next;
        }
        playerEnd = true;
    }

    IEnumerator DiscardByDamage()
    {
        Debug.LogError($"{damageSum}장 버려야 합니다.");
        UIManager.Instance.PopupCard(deck.useDeck);
        UIManager.Instance.selectCardChanged += Discard;
        while (damageSum > 0)
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
            damageSum--;
            UIManager.Instance.RemoveSelectObject();
        }
    }

    [PunRPC]
    public void CallCheckPhaseEnd()
    {
        phaseTrigger = true;
    }


    public void FirstTurnSetting()
    {
        deck.Shuffle();
        deck.Draw(5);
        SummonHero();
        UIManager.Instance.StartMulligan();
        _gamePhase = GamePhase.DrawPhase;
    }

    private void SummonHero()
    {
        if (startFirst)
        {
            FieldManager.instance.battleField.First.cardData = deck.myHero;
            photonView.RPC("CallSummonUnit", RpcTarget.Others, 0, false, false, deck.myHero.cardID, false, int.Parse(playerID));
        }
        else
        {
            FieldManager.instance.battleField.Last.cardData = deck.myHero;
            FieldManager.instance.battleField.Last.lookingLeft = true;
            photonView.RPC("CallSummonUnit", RpcTarget.Others, 4, false, false, deck.myHero.cardID, true, int.Parse(playerID));
        }
    }

    public void AttackPhaseStart()
    {
        FieldCardObject tmp = FieldManager.instance.battleField.First;
        while (tmp != null)
        {
            tmp.GetComponent<BoxCollider2D>().enabled = true;
            tmp = tmp.Next;
        }
    }

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

    public void EndButton()
    {
        if (canAct && playerEnd == false && useCard)
        {
            canAct = false;
        }
        else if (canAct && playerEnd == false)
        {
            canAct = false;
            playerEnd = true;
        }
    }

    public void GameSet()
    {
        Debug.Log("GameSet");
    }

    public void TurnStart()
    {
        currentTurn++;
        if (canAct)
        {
            turnText.color = personalColor.color;
        }
        else
        {
            turnText.color = Color.white;
        }
    }

    [PunRPC]
    public void CallPlayerTurnEnd()
    {
        Debug.LogError("CallPlayerTurnEnd");
        if (playerEnd)
        {
            if (enemyEnd == false)
            {
                photonView.RPC("CallPlayerTurnEnd", RpcTarget.Others);
            }
            return;
        }
        else if (canAct == false && (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase))
        {
            canAct = true;
        }
    }

    [PunRPC]
    public void CallPlayerPhaseEnd()
    {
        Debug.LogError("CallPlayerPhaseEnd");
        enemyEnd = true;
        if (playerEnd == false && (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase))
        {
            if (canAct == false)
            {
                canAct = true;
            }
        }
    }

    [PunRPC]
    public void CallSummonUnit(int index, bool make, bool makeLeft, int cardID, bool lookingLeft, int playerID)
    {
        Debug.LogError("CallSummonUnit");
        FieldManager.instance.SetCardToFieldForPun(index, make, makeLeft, cardID, lookingLeft, playerID);
    }
}

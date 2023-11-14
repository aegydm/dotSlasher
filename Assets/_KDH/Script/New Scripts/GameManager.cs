using CCGCard;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
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
    public event Action CallTurnEnd;

    public bool isAlreadyAttack = false;
    public bool dirtySet = false;
    public bool battleDirtySet = false;
    public bool canAct
    {
        get
        {
            return _canAct;
        }
        set
        {
            if (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase || gamePhase == GamePhase.None)
            {
                if (canAct != value)
                {
                    if ((playerEnd == false) || (value == false))
                    {
                        _canAct = value;
                    }
                    if (value)
                    {
                        Debug.LogError("CallTurnStart");
                        CallTurnStart?.Invoke();
                        useCard = false;
                    }
                    else
                    {
                        Debug.LogError("CallTurnEnd");
                        CallTurnEnd?.Invoke();
                    }
                    if (canAct)
                    {
                        turnText.color = personalColor.color;
                    }
                    else
                    {
                        turnText.color = Color.white;
                    }
                    Debug.LogError($"CanAct is : {canAct}");
                }
            }
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
            {
                _gamePhase = value;
                if (gamePhase != GamePhase.None)
                {
                    StartPhaseSetting();
                    //FieldManager.instance.CheckInterAll();
                }
            }
        }
    }

    [SerializeField] private GamePhase _gamePhase = GamePhase.None;
    public GamePhase nextPhase = GamePhase.DrawPhase;

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
                    if (gamePhase == GamePhase.EndPhase)
                    {
                        nextPhase = GamePhase.DrawPhase;
                    }
                    gamePhase = GamePhase.None;
                }
            }
            if (playerEnd != value)
            {
                _playerEnd = value;
                StartCoroutine(CheckPhaseEnd());
            }
            Debug.LogError($"PlayerEnd is : {playerEnd}, Call CheckPhaseEnd");
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
                turnText.text = gamePhase.ToString() + " : " + (currentTurn + 1);
            }
            else if (currentTurn > 0)
            {
                turnText.text = turnText.text.Substring(0, turnText.text.Length - 2) + (currentTurn + 1);
            }
        }
    }
    private int _currentTurn;

    [SerializeField] private bool _playerEnd;
    [SerializeField] private bool _enemyEnd;

    public bool playerLose;

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
        CallTurnEnd += TurnEnd;
    }


    IEnumerator CheckPhaseEnd()
    {
        Debug.LogError($"PlayerEnd is : {playerEnd}, EnemyEnd is : {enemyEnd}");
        if (playerEnd && enemyEnd)
        {
            if (dirtySet == false)
            {
                dirtySet = true;
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
                yield return new WaitForSeconds(5);
                _playerEnd = false;
                _enemyEnd = false;
                Debug.LogError(nextPhase);
                gamePhase = nextPhase;
                turnText.text = gamePhase.ToString();
                Debug.LogError("Currnet Phase is " + gamePhase.ToString());
                currentTurn = 0;
                phaseTrigger = false;
                dirtySet = false;
            }
        }
        yield return Time.deltaTime;
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
        canAct = false;
        deck.Draw(5);
        playerEnd = true;
    }

    private void ActionPhaseStart()
    {
        if (startFirst)
        {
            _canAct = true;
            turnText.color = personalColor.color;
            CallTurnStart?.Invoke();
            currentTurn = 0;
        }
        else
        {
            _canAct = false;
            turnText.color = Color.white;
        }
    }

    private void BattlePhaseStart()
    {
        Debug.LogError("BattlePhaseStart");
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            temp.ResetField();
            temp.attackChance = true;
            temp.canBattle = !temp.isEmpty;
            temp = temp.Next;
        }
        CheckCanBattle();
        CheckMyUnitCanAttack();

        if (startFirst)
        {
            _canAct = true;
            turnText.color = personalColor.color;
            CallTurnStart?.Invoke();
            currentTurn = 0;
        }
        else
        {
            _canAct = false;
            turnText.color = Color.white;
        }
        CallTurnEnd -= TurnEnd;
        CallTurnStart += CheckCanBattle;
        CallTurnStart += CheckMyUnitCanAttack;
        CallTurnEnd += CheckCanBattle;
        CallTurnEnd += CheckMyUnitCanAttack;
        CallTurnEnd += TurnEnd;
    }

    private void CheckCanBattle()
    {
        Debug.LogError("CheckCanBattle");
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
                    if (temp.lookingLeft)
                    {
                        if (temp.Prev == null || temp.Prev.isEmpty)
                        {
                            temp.canBattle = false;
                        }
                        else if (temp.Prev.playerID == temp.playerID)
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
                        if (temp.Next == null || temp.Next.isEmpty)
                        {
                            temp.canBattle = false;
                        }
                        else if (temp.Next.playerID == temp.playerID)
                        {
                            temp.canBattle = false;
                        }
                        else
                        {
                            temp.canBattle = true;
                        }
                    }
                    if (temp.cardData.frontDamage == 0)
                    {
                        temp.canBattle = false;
                    }
                }
            }
            temp = temp.Next;
        }
    }

    private void CheckMyUnitCanAttack()
    {
        if (battleDirtySet == false)
        {
            battleDirtySet = true;
            Debug.LogError("CheckMUCA");
            bool trigger = false;
            if (gamePhase == GamePhase.BattlePhase || nextPhase == GamePhase.ExecutionPhase)
            {
                Debug.LogError("CheckMyUnitCanAttack");
                FieldCardObject myTemp = FieldManager.instance.battleField.First;
                while (myTemp != null)
                {
                    if ((myTemp.playerID == int.Parse(playerID)) && myTemp.canBattle)
                    {
                        Debug.LogError($"{myTemp} Can Attack!");
                        if (playerEnd)
                        {
                            Debug.LogError("CheckMUCA GamePhase RollBack");
                            _gamePhase = GamePhase.BattlePhase;
                            Debug.Log("CheckMyUnitCanAttack PlayerEnd False");
                            _playerEnd = false;
                        }
                        trigger = true;
                    }
                    if ((myTemp.playerID != int.Parse(playerID)) && myTemp.canBattle)
                    {
                        if (enemyEnd)
                        {
                            Debug.Log("CheckMyUnitCanAttack EnemyEnd False");
                            _enemyEnd = false;
                        }
                    }
                    myTemp = myTemp.Next;
                }
                if (trigger == false)
                {
                    if (playerEnd == false)
                    {
                        playerEnd = true;
                    }
                    if (canAct)
                    {
                        canAct = false;
                    }
                }
            }
            battleDirtySet = false;
            return;
        }
    }

    private void ExecutionPhaseStart()
    {
        canAct = false;
        CallTurnStart -= CheckCanBattle;
        CallTurnStart -= CheckMyUnitCanAttack;
        CallTurnEnd -= CheckCanBattle;
        CallTurnEnd -= CheckMyUnitCanAttack;
        Debug.LogError("처리페이즈에 진입 했습니다.");
        if (damageSum == 0)
        {
            Debug.LogError("모든 카드를 버렸습니다.");
            playerEnd = true;
            return;
        }
        StartCoroutine(DiscardByDamage());
    }

    private void EndPhaseStart()
    {
        canAct = false;
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
        FieldManager.instance.ResetGameField();
        FieldManager.instance.CheckInterAll();
        playerEnd = true;
    }

    IEnumerator DiscardByDamage()
    {
        Debug.LogError($"{damageSum}장 버려야합니다.");
        UIManager.Instance.PopupCard(deck.useDeck);
        UIManager.Instance.selectCardChanged += Discard;
        UIManager.Instance.exitButton.SetActive(false);
        while (damageSum > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        UIManager.Instance.exitButton.SetActive(true);
        UIManager.Instance.selectCardChanged -= Discard;
        UIManager.Instance.ClosePopup();
        Debug.LogError("모든 카드를 버렸습니다.");
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
        FieldManager.instance.CheckInterAll();
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
            Debug.LogError("null animator error");
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
        if (gamePhase == GamePhase.ActionPhase)
        {
            if (canAct && playerEnd == false && useCard)
            {
                canAct = false;
                if (PlayerActionManager.instance.handCardCount == 0 || FieldManager.instance.FieldIsFull())
                {
                    playerEnd = true;
                }
            }
            else if (canAct && playerEnd == false)
            {
                canAct = false;
                playerEnd = true;
            }
        }
        if (gamePhase == GamePhase.BattlePhase)
        {
            if (isAlreadyAttack == false && canAct)
            {
                Debug.LogError("시간초과로 자동 공격 처리되었습니다.");
                FieldCardObject temp = FieldManager.instance.battleField.First;
                while (temp != null)
                {
                    if (temp.canBattle && temp.playerID == int.Parse(playerID))
                    {
                        temp.FieldAttack();
                        break;
                    }
                    temp = temp.Next;
                }
            }
        }
    }

    public void GameSet()
    {
        Debug.Log("GameSet");
    }

    public void TurnStart()
    {
        if (enemyEnd == false)
        {

            currentTurn++;
        }
    }

    public void TurnEnd()
    {
        Debug.LogError("TurnEnd");
        currentTurn++;
        photonView.RPC("CallPlayerTurnEnd", RpcTarget.Others);
    }

    [PunRPC]
    public void CallPlayerTurnEnd()
    {
        Debug.LogError("CallPlayerTurnEnd");
        if (playerEnd)
        {
            if (enemyEnd == false)
            {
                if (nextPhase == GamePhase.ExecutionPhase)
                {
                    CheckCanBattle();
                    CheckMyUnitCanAttack();
                    if (playerEnd == false)
                    {
                        StopCoroutine("CheckPhaseEnd");
                        canAct = true;
                    }
                    else
                    {
                        currentTurn++;
                        photonView.RPC("CallPlayerTurnEnd", RpcTarget.Others);
                    }
                }
                else
                {
                    currentTurn++;
                    photonView.RPC("CallPlayerTurnEnd", RpcTarget.Others);
                }
            }
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
        if (playerEnd == false && (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase || nextPhase == GamePhase.ExecutionPhase))
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

    [PunRPC]
    public void CallPlayerWinOrLose(bool enemyLose)
    {
        if (enemyLose == playerLose)
        {
            Debug.Log("Draw");
        }
        else
        {
            if (enemyLose == false)
            {
                Debug.Log("Lose");
            }
            else
            {
                Debug.Log("Win");
            }
        }
    }

    [PunRPC]
    public void AttackUnit(int index)
    {
        FieldManager.instance.battleField[index].cardData.AttackStart(FieldManager.instance.battleField, FieldManager.instance.battleField[index]);
    }

    public void Lose()
    {
        playerLose = true;
        photonView.RPC("CallPlayerWinOrLose", RpcTarget.Others, playerLose);
    }

}

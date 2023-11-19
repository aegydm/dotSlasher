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
    public event Action CallPhaseStart;

    [SerializeField] GameObject[] enemyHandGO;
    public bool isAlreadyAttack = false;
    public bool dirtySet = false;
    public bool battleDirtySet = false;
    public bool isStart = false;
    public int enemyHandCount = 0;
    public GameObject mySkill1GO;
    public GameObject enemySkill1GO;
    public GameObject turnEndGO;

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
                        //Debug.LogError("CallTurnStart");
                        CallTurnStart?.Invoke();
                        useCard = false;
                    }
                    else
                    {
                        //Debug.LogError("CallTurnEnd");
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
                    //Debug.LogError($"CanAct is : {canAct}");
                }
            }
        }
    }

    public bool useCard = false;
    public bool startFirst;
    public bool isGameEnd;
    [SerializeField] TMP_Text turnText;
    [SerializeField] TMP_Text phaseText;
    [SerializeField] GameObject myPass;
    [SerializeField] GameObject enemyPass;
    [SerializeField] private bool _canAct;
    public PhotonView photonView;
    public Deck deck;
    private bool phaseTrigger;

    public int damageSum = 0;
    public int enemyDamageSum = 0;
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
                    myPass.SetActive(true);
                    photonView.RPC("CallPlayerPhaseEnd", RpcTarget.Others);
                    nextPhase = gamePhase + 1;
                    if (gamePhase == GamePhase.ActionPhase && enemyEnd == false)
                    {
                        startFirst = true;
                        Debug.LogError("StartFirst");
                    }
                    else if (gamePhase == GamePhase.ActionPhase && enemyEnd == true)
                    {
                        startFirst = false;
                        Debug.LogError("STartLAST");
                    }
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
            //Debug.LogError($"PlayerEnd is : {playerEnd}, Call CheckPhaseEnd");
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
            //Debug.LogError($"EnemyEnd is : {enemyEnd}");
            if (gamePhase == GamePhase.None)
            {
                //Debug.LogError("EnemyEnd! Call CheckPhaseEnd");
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
            if (dirtySet == false)
            {
                if (gamePhase == GamePhase.ActionPhase || gamePhase == GamePhase.BattlePhase)
                {
                    turnText.text = "Turn : " + (currentTurn + 1).ToString();
                }
                else if (gamePhase == GamePhase.None && (nextPhase == GamePhase.BattlePhase || nextPhase == GamePhase.ExecutionPhase))
                {
                    turnText.text = "Turn : " + (currentTurn + 1).ToString();
                }
                else
                {
                    turnText.text = null;
                }
            }
        }
    }
    private int _currentTurn;

    [SerializeField] private bool _playerEnd;
    [SerializeField] private bool _enemyEnd;

    [Header("Sounds")]
    [SerializeField] private AudioClip BGM;
    [SerializeField] private AudioClip TurnStartSound;
    [SerializeField] private AudioClip TurnEndSound;
    [SerializeField] private AudioClip DrawSound;
    [SerializeField] private AudioClip FirstTurnSound;
    [SerializeField] private AudioClip BattlePhaseStartSound;
    [SerializeField] private AudioClip BPhaseStart;
    [SerializeField] private AudioClip BPhaseEnd;
    public bool playerLose;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        NetworkManager.instance.SetPlayerID();
        startFirst = NetworkManager.instance.first;
        //Debug.LogError(startFirst);
        personalColor.color = new Color(255 - (255 * int.Parse(playerID)), 255 * int.Parse(playerID), 0);
        Invoke("FirstTurnSetting", 3f);
        CallTurnStart += TurnStart;
        CallTurnEnd += TurnEnd;
        CallPhaseStart += PhaseStart;
    }

    private void PhaseStart()
    {
        phaseText.text = gamePhase.ToString();
    }

    IEnumerator CheckPhaseEnd()
    {
        //Debug.LogError($"PlayerEnd is : {playerEnd}, EnemyEnd is : {enemyEnd}");
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
                myPass.SetActive(false);
                _enemyEnd = false;
                enemyPass.SetActive(false);
                //Debug.LogError(nextPhase);
                gamePhase = nextPhase;
                CallPhaseStart?.Invoke();
                //turnText.text = gamePhase.ToString();
                //Debug.LogError("Currnet Phase is " + gamePhase.ToString());
                phaseTrigger = false;
                dirtySet = false;
                currentTurn = 0;
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
                turnEndGO.SetActive(false);
                Invoke("BattlePhaseStart", 3);
                break;
            case GamePhase.ExecutionPhase:
                Invoke("ExecutionPhaseStart", 3);
                break;
            case GamePhase.EndPhase:
                Invoke("EndPhaseStart", 3);
                break;
            default:
                //Debug.LogError("Error is Occur in Phase Change");
                break;
        }
    }

    private void DrawPhaseStart()
    {
        canAct = false;
        deck.Draw(5);
        SoundManager.instance.PlayEffSound(DrawSound);
        playerEnd = true;
    }

    private void ActionPhaseStart()
    {
        turnEndGO.SetActive(true);
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
        //Debug.LogError("BattlePhaseStart");
        SoundManager.instance.PlayEffSound(BattlePhaseStartSound);
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            temp.ResetField();
            temp.attackChance = true;
            temp.canBattle = !temp.isEmpty;
            temp = temp.Next;
        }

        if (startFirst)
        {
            _canAct = true;
            //Debug.LogError("canAct is true!");
            turnText.color = personalColor.color;
            CallTurnStart?.Invoke();
            currentTurn = 0;
        }
        else
        {
            _canAct = false;
            //Debug.LogError("canAct is false!");
            turnText.color = Color.white;
        }
        //CallTurnEnd -= TurnEnd;
        CallTurnStart += CheckCanBattle;
        CallTurnStart += CheckMyUnitCanAttack;
        CallTurnEnd += CheckCanBattle;
        CallTurnEnd += CheckMyUnitCanAttack;
        //CallTurnEnd += TurnEnd;
        CheckCanBattle();
        CheckMyUnitCanAttack();
    }

    private void CheckCanBattle()
    {
        //Debug.LogError("CheckCanBattle");
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
            //Debug.LogError("CheckMUCA");
            bool trigger = false;
            if (gamePhase == GamePhase.BattlePhase || nextPhase == GamePhase.ExecutionPhase)
            {
                //Debug.LogError("CheckMyUnitCanAttack");
                FieldCardObject myTemp = FieldManager.instance.battleField.First;
                while (myTemp != null)
                {
                    if ((myTemp.playerID == int.Parse(playerID)) && myTemp.canBattle)
                    {
                        //Debug.LogError($"{myTemp} Can Attack!");
                        if (playerEnd)
                        {
                            //Debug.LogError("CheckMUCA GamePhase RollBack");
                            _gamePhase = GamePhase.BattlePhase;
                            //Debug.Log("CheckMyUnitCanAttack PlayerEnd False");
                            _playerEnd = false;
                            myPass.SetActive(false);
                        }
                        trigger = true;
                    }
                    if ((myTemp.playerID != int.Parse(playerID)) && myTemp.canBattle)
                    {
                        if (enemyEnd)
                        {
                            //Debug.Log("CheckMyUnitCanAttack EnemyEnd False");
                            _enemyEnd = false;
                            enemyPass.SetActive(false);
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
        //Debug.LogError("泥섎━?섏씠利덉뿉 吏꾩엯 ?덉뒿?덈떎.");
        if (damageSum == 0)
        {
            //Debug.LogError("紐⑤뱺 移대뱶瑜?踰꾨졇?듬땲??");
            playerEnd = true;
            return;
        }
        FindObjectOfType<Timer>().PlayerTimer();
        StartCoroutine(nameof(DiscardByDamage));
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
                else
                {
                    if (temp.playerID == int.Parse(playerID))
                    {
                        temp.cardData = CardDB.instance.FindCardFromID(temp.cardData.cardID);
                        temp.RenderCard();
                        temp.animator.Play("Breath");

                    }
                    else
                    {
                        int pid = temp.playerID;
                        temp.cardData = CardDB.instance.FindCardFromID(temp.cardData.cardID);
                        temp.playerID = pid;
                        temp.RenderCard();
                        temp.animator.Play("Breath");
                    }
                }
            }
            temp = temp.Next;
        }
        FieldManager.instance.ResetGameField();
        FieldManager.instance.CheckInterAll();
        FieldManager.instance.battleField.First.lookingLeft = false;
        FieldManager.instance.battleField.Last.lookingLeft = true;
        playerEnd = true;
    }

    IEnumerator DiscardByDamage()
    {
        //Debug.LogError($"{damageSum}??踰꾨젮?쇳빀?덈떎.");
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
        //Debug.LogError("紐⑤뱺 移대뱶瑜?踰꾨졇?듬땲??");
        FindObjectOfType<Timer>().StopTimer();
        enemyDamageSum = 0;
        playerEnd = true;
    }

    public void Discard(Card card)
    {
        if (deck.RemoveDeckCard(card))
        {
            damageSum--;
            UIManager.Instance.RemoveSelectObject();
            photonView.RPC("EnemyDeckDiscard", RpcTarget.Others, card.cardID);
        }
    }

    [PunRPC]
    public void CallCheckPhaseEnd()
    {
        phaseTrigger = true;
    }


    public void FirstTurnSetting()
    {
        //Please Input BGM Start Code
        //BGM ?ъ슫???쒖옉 肄붾뱶 ?ｌ뼱二쇱꽭??
        SoundManager.instance.PlayBGMSound(BGM);
        isStart = true;
        deck.Shuffle();
        deck.Draw(5);
        SoundManager.instance.PlayEffSound(DrawSound);
        SummonHero();
        UIManager.Instance.StartMulligan();
        _gamePhase = GamePhase.DrawPhase;
        FieldManager.instance.CheckInterAll();
        FieldManager.instance.additionalCount = 5;
        //Debug.Log("Timer Call");
        FindObjectOfType<Timer>().PlayerTimer();
    }

    private void SummonHero()
    {
        if (startFirst)
        {
            FieldManager.instance.battleField.First.cardData = deck.myHero;
            FieldManager.instance.battleField.First.RenderCard();
            photonView.RPC("CallSummonUnit", RpcTarget.Others, 0, false, false, deck.myHero.cardID, false, int.Parse(playerID));
        }
        else
        {
            FieldManager.instance.battleField.Last.cardData = deck.myHero;
            FieldManager.instance.battleField.Last.lookingLeft = true;
            FieldManager.instance.battleField.Last.RenderCard();
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
            //Debug.LogError("null animator error");
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
        if (gamePhase == GamePhase.DrawPhase)
        {
            UIManager.Instance.EndMulligan();
        }
        else if (gamePhase == GamePhase.ActionPhase)
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
        else if (gamePhase == GamePhase.BattlePhase)
        {
            if (isAlreadyAttack == false && canAct)
            {
                //Debug.LogError("?쒓컙珥덇낵濡??먮룞 怨듦꺽 泥섎━?섏뿀?듬땲??");
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
        else if (gamePhase == GamePhase.ExecutionPhase)
        {
            Debug.LogError("Time Limit Over. Discard Random");
            while(damageSum > 0)
            {
                int randNum = UnityEngine.Random.Range(0, UIManager.Instance.uiCardList.Count);
                UIManager.Instance.selectCard = UIManager.Instance.uiCardList[randNum].GetComponent<UICard>().cardData;
            }
            StopCoroutine(nameof(DiscardByDamage));
            UIManager.Instance.exitButton.SetActive(true);
            UIManager.Instance.selectCardChanged -= Discard;
            UIManager.Instance.ClosePopup();
            //Debug.LogError("紐⑤뱺 移대뱶瑜?踰꾨졇?듬땲??");
            enemyDamageSum = 0;
            playerEnd = true;
        }
    }

    public void GameSet()
    {
        Debug.Log("GameSet");
    }

    public void TurnStart()
    {
        //Please Input Turn Start Sound Code
        //???쒖옉???섏삤???뚮━ 肄붾뱶 ?ｌ뼱二쇱꽭??
        SoundManager.instance.PlayEffSound(TurnStartSound);
        if (FieldManager.instance.FieldIsFull() && (gamePhase == GamePhase.ActionPhase))
        {
            _canAct = false;
            GetComponent<Timer>().StopTimer();
            playerEnd = true;
            startFirst = false;
        }
    }

    public void TurnEnd()
    {
        //Please Input Turn End Sound Code
        //??醫낅즺???섏삤???뚮━ 肄붾뱶 ?ｌ뼱二쇱꽭??
        SoundManager.instance.PlayEffSound(TurnEndSound);
        if (useCard == false)
        {
            PlayerActionManager.instance.CancelAll();
        }
        //Debug.LogError("TurnEnd");
        currentTurn++;
        photonView.RPC("CallPlayerTurnEnd", RpcTarget.Others);
    }

    [PunRPC]
    public void CallPlayerTurnEnd()
    {
        currentTurn++;
        //Debug.LogError("CallPlayerTurnEnd");
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
                        CallTurnEnd();
                    }
                }
                else
                {
                    CallTurnEnd();
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
        enemyPass.SetActive(true);
        //Debug.LogError("CallPlayerPhaseEnd");
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
        //Debug.LogError("CallSummonUnit");
        FieldManager.instance.SetCardToFieldForPun(index, make, makeLeft, cardID, lookingLeft, playerID);
    }
    [PunRPC]
    public void AttackUnit(int index)
    {
        FieldManager.instance.battleField[index].cardData.AttackStart(FieldManager.instance.battleField, FieldManager.instance.battleField[index]);
    }

    [PunRPC]
    public void EnemyDeckDiscard(int cardID)
    {
        deck.enemyGrave.Add(CardDB.instance.FindCardFromID(cardID));
        deck.RefreshEnemyGraveCount();
    }

    [PunRPC]
    public void EnemyDeckReduce(int cardCount)
    {
        deck.enemyDeckCount = cardCount;
    }

    public void CheckGameEnd()
    {
        if (gamePhase == GamePhase.ExecutionPhase)
        {
            if (deck.enemyDeckCount < enemyDamageSum)
            {
                if (deck.countOfDeck < damageSum)
                {
                    if (deck.enemyDeckCount - enemyDamageSum < deck.countOfDeck - damageSum)
                    {
                        Win();
                    }
                    else if (deck.enemyDeckCount - enemyDamageSum > deck.countOfDeck - damageSum)
                    {
                        Lose();
                    }
                    Draw();
                }
                else
                {
                    Win();
                }
            }
            else
            {
                if (deck.countOfDeck < damageSum)
                {
                    Lose();
                }
                else
                {

                }
            }
        }
        else if (gamePhase == GamePhase.DrawPhase)
        {
            if ((5 - enemyHandCount) > deck.enemyDeckCount)
            {
                if ((5 - PlayerActionManager.instance.handCardCount) > deck.countOfDeck)
                {
                    if ((5 - enemyHandCount) - deck.enemyDeckCount < (5 - PlayerActionManager.instance.handCardCount) - deck.countOfDeck)
                    {
                        Win();
                    }
                    else if ((5 - enemyHandCount) - deck.enemyDeckCount > (5 - PlayerActionManager.instance.handCardCount) - deck.countOfDeck)
                    {
                        Lose();
                    }
                    else
                    {
                        Draw();
                    }
                }
                else
                {
                    Win();
                }
            }
            else
            {
                if ((5 - PlayerActionManager.instance.handCardCount) > deck.countOfDeck)
                {
                    Lose();
                }
                else
                {

                }
            }
        }

    }

    [PunRPC]
    public void CallPlayerWinOrLose(bool enemyLose)
    {
        //if (enemyLose == playerLose)
        //{
        //    Debug.Log("Draw");
        //}
        //else
        {
            if (enemyLose == false)
            {
                Debug.Log("Lose");
            }
            else
            {
                Win();
            }
        }
    }

    public void Win()
    {
        playerLose = false;
        StopAllCoroutines();
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            if (temp.cardData != null && temp.playerID != int.Parse(playerID) && temp.cardData.cardCategory == CardCategory.hero)
            {
                temp.animator.Play("Death");
                break;
            }
            temp = temp.Next;
        }
        if (UIManager.Instance.isPopUI)
        {
            UIManager.Instance.ClosePopup();
        }
        //Debug.LogError("You Win");
    }

    public void Lose()
    {
        playerLose = true;
        photonView.RPC("CallPlayerWinOrLose", RpcTarget.Others, playerLose);
        StopAllCoroutines();
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            if (temp.cardData != null && temp.playerID == int.Parse(playerID) && temp.cardData.cardCategory == CardCategory.hero)
            {
                temp.animator.Play("Death");
                break;
            }
            temp = temp.Next;
        }
        if (UIManager.Instance.isPopUI)
        {
            UIManager.Instance.ClosePopup();
        }
        //Debug.LogError("You Lose");
    }

    public void Draw()
    {
        StopAllCoroutines();
        if (UIManager.Instance.isPopUI)
        {
            UIManager.Instance.ClosePopup();
        }
        //Debug.LogError("Game Draw");
        Time.timeScale = 0;
    }

    [PunRPC]
    public void EnemyHandCardChange(int enemyHandCount)
    {
        this.enemyHandCount = enemyHandCount;
        for (int i = 0; i < 5; i++)
        {
            //Debug.LogError("HandCardChange + " + enemyHandCount);
            if (i < enemyHandCount)
            {
                enemyHandGO[i].SetActive(true);
            }
            else
            {
                enemyHandGO[i].SetActive(false);
            }
        }
    }

    [PunRPC]
    public void EnemyHeroSkill1()
    {
        FieldCardObject temp = FieldManager.instance.battleField.First;
        while (temp != null)
        {
            if (temp.cardData != null && temp.playerID.ToString() != GameManager.instance.playerID && temp.cardData.cardCategory == CardCategory.hero)
            {
                //Debug.LogError("EnemySkill One Use");
                ((Hero)temp.cardData).SkillUse(FieldManager.instance.battleField, temp);
                enemySkill1GO.GetComponent<Image>().color = Color.gray;
                return;
            }
            temp = temp.Next;
        }
    }
}

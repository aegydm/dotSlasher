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

public class TestManager : MonoBehaviour
{
    public static TestManager instance;

    public event Action CallTurnStart;

    public bool canAct
    {
        get
        {
            return _canAct;
        }
        set
        {
            if(canAct != value)
            {
                CallTurnStart?.Invoke();
            }
            _canAct = value;
        }
    }

    public bool startFirst;
    public bool isGameEnd;
    [SerializeField] TMP_Text turnText;
    [SerializeField] private bool _canAct;
    public PhotonView photonView;
    public Deck deck;

    public string playerID;

    public GamePhase gamePhase = GamePhase.None;
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
            _playerEnd = value;
            if(playerEnd == true)
            {
                nextPhase = gamePhase + 1;
                gamePhase = GamePhase.None;
                CheckPhaseEnd();
            }
        }
    }

    private void CheckPhaseEnd()
    {
        if(playerEnd && enemyEnd)
        {
            playerEnd = false;
            enemyEnd = false;
            if ((nextPhase == GamePhase.ActionPhase || nextPhase == GamePhase.BattlePhase) && startFirst)
            {
                canAct = true;
            }
            else
            {
                canAct = false;
            }
            gamePhase = nextPhase;
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

    public int currentTurn
    {
        get
        {
            return _currentTurn;
        }
        set
        {
            _currentTurn = value;
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
        personalColor.color = new Color(255 - (255 * int.Parse(playerID)), 255 * int.Parse(playerID), 0);
        Invoke("FirstTurnSetting", 3f);
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

    public void FirstTurnSetting()
    {
        deck.Shuffle();
        deck.Draw(5);
        SummonHero();
        UIManager.Instance.StartMulligan();
    }

    private void SummonHero()
    {
        if (startFirst)
        {
            FieldManagerTest.instance.battleField.First.cardData = deck.myHero;
        }
        else
        {
            FieldManagerTest.instance.battleField.Last.cardData = deck.myHero;
        }
    }

    public void AttackPhaseStart()
    {
        FieldCardObjectTest tmp = FieldManagerTest.instance.battleField.First;
        while(tmp != null)
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
}

using CCGCard;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public List<Card> useDeck = new List<Card>();
    public List<Card> originDeck = new List<Card>();
    public int enemyDeckCount
    {
        get
        {
            return _enemyDeckCount;
        }
        set
        {
            _enemyDeckCount = value;
            enemyDeckCountUI.text = _enemyDeckCount.ToString();
        }
    }
    [SerializeField] int _enemyDeckCount = 30;

    public List<Card> grave
    {
        get
        {
            return _grave;
        }
        set
        {
            if (_grave.Count != value.Count)
            {
                countOfGrave = value.Count;
            }
            _grave = value;
        }
    }
    [SerializeField] private List<Card> _grave = new List<Card>();

    public List<Card> enemyGrave
    {
        get
        {
            return _enemyGrave;
        }
        set
        {
            if (_enemyGrave.Count != value.Count)
            {
                countOfEnemyGrave = value.Count;
            }
            _enemyGrave = value;
        }
    }
    [SerializeField] List<Card> _enemyGrave = new List<Card>();
    public Card myHero = new Hero();

    public int countOfDeck
    {
        get
        {
            return _countOfDeck;
        }
        set
        {
            if (_countOfDeck != value)
            {
                _countOfDeck = value;
                OnDeckCountChanged?.Invoke();
            }
        }
    }

    public int countOfGrave
    {
        get
        {
            return _countOfGrave;
        }
        set
        {
            if (_countOfGrave != value)
            {
                _countOfGrave = value;
                OnGraveCountChanged?.Invoke();
            }
        }
    }

    public int countOfEnemyGrave
    {
        get
        {
            return _countOfEnemyGrave;
        }
        set
        {
            if (_countOfEnemyGrave != value)
            {
                _countOfEnemyGrave = value;
                OnEnemyGraveCountChanged?.Invoke();
            }
        }
    }
    [SerializeField] int _countOfEnemyGrave;
    [SerializeField] int _countOfGrave;
    [SerializeField] int _countOfDeck;
    [SerializeField] public List<int> sortedDeck;
    [SerializeField] TMP_Text deckCountUI;
    [SerializeField] TMP_Text enemyDeckCountUI;
    [SerializeField] TMP_Text graveCountUI;
    [SerializeField] TMP_Text enemyGraveCountUI;
    public event Action OnDeckCountChanged;
    public event Action OnGraveCountChanged;
    public event Action OnEnemyGraveCountChanged;


    private void Start()
    {
        OnDeckCountChanged = null;
        OnDeckCountChanged += RenderDeckCount;
        OnGraveCountChanged = null;
        OnGraveCountChanged += RenderGraveCount;
        OnEnemyGraveCountChanged = null;
        OnEnemyGraveCountChanged += RenderEnemyGraveCount;
        //if (BuildManager.instance.Load(BuildManager.instance.deckName, out originDeck) == false)
        if (NetworkManager.instance != null)
        {
            if (BuildManager.instance.Load(NetworkManager.instance.deckName, out originDeck) == false)
            {
                Debug.Log("Fail to Load Deck");
                //GameManager.instance.Lose();
            }
            foreach (var card in originDeck)
            {
                if (card.cardCategory != CardCategory.hero)
                {
                    useDeck.Add(card);
                }
                else
                {
                    myHero = card;
                }
            }
            RefreshDeckCount();
        }
    }

    public void LoadDeckFromBuildManager(string deckName = "")
    {
        if (NetworkManager.instance != null)
        {
            if (deckName == "")
            {
                if (BuildManager.instance.Load("1", out originDeck) == false)
                {
                    Debug.Log("Fail to Load Deck");
                    //GameManager.instance.Lose();
                }
                foreach (var card in originDeck)
                {
                    if (card.cardCategory != CardCategory.hero)
                    {
                        useDeck.Add(card);
                    }
                    else
                    {
                        myHero = card;
                    }
                }
                RefreshDeckCount();
            }
            else
            {
                if (BuildManager.instance.Load(NetworkManager.instance.deckName, out originDeck) == false)
                {
                    Debug.Log("Fail to Load Deck");
                    //GameManager.instance.Lose();
                }
                foreach (var card in originDeck)
                {
                    if (card.cardCategory != CardCategory.hero)
                    {
                        useDeck.Add(card);
                    }
                    else
                    {
                        myHero = card;
                    }
                }
                RefreshDeckCount();

            }
        }
        else
        {
            if (deckName == "")
            {
                if (BuildManager.instance.Load(BuildManager.instance.deckName, out originDeck) == false)
                {
                    Debug.Log("Fail to Load Deck");
                    //GameManager.instance.Lose();
                }
                foreach (var card in originDeck)
                {
                    if (card.cardCategory != CardCategory.hero)
                    {
                        useDeck.Add(card);
                    }
                    else
                    {
                        myHero = card;
                    }
                }
                RefreshDeckCount();
            }
            else
            {

                if (BuildManager.instance.Load(deckName, out originDeck) == false)
                {
                    Debug.Log("Fail to Load Deck");
                    //GameManager.instance.Lose();
                }
                foreach (var card in originDeck)
                {
                    if (card.cardCategory != CardCategory.hero)
                    {
                        useDeck.Add(card);
                    }
                    else
                    {
                        myHero = card;
                    }
                }
                RefreshDeckCount();
            }
        }
    }

    void RenderDeckCount()
    {
        deckCountUI.text = countOfDeck.ToString();
    }

    void RenderGraveCount()
    {
        graveCountUI.text = countOfGrave.ToString();
    }

    void RenderEnemyGraveCount()
    {
        enemyGraveCountUI.text = countOfEnemyGrave.ToString();
    }

    /// <summary>
    /// 덱의 카드를 섞는 카드
    /// </summary>
    public void Shuffle()
    {
        List<Card> list = new List<Card>();
        int listCount = useDeck.Count;
        for (int i = 0; i < listCount; i++)
        {
            int rand = Random.Range(0, useDeck.Count);
            list.Add(useDeck[rand]);
            useDeck.RemoveAt(rand);
        }
        useDeck = list;

        RefreshDeckCount();
    }

    /// <summary>
    /// int의 값 만큼 덱에서 카드를 뽑는 카드
    /// 현재 손패가 가득 찰 경우 에러코드를 보내도록 설계되어있음
    /// 덱이 0장이고 손패에 빈 자리가 있을 경우
    /// </summary>
    /// <param name="drawCard"></param>
    public void Draw(int drawCard)
    {

        for (int i = 0; i < drawCard; i++)
        {
            if (countOfDeck > 0)
            {
                if (PlayerActionManager.instance.AddHandCard(useDeck[0]))
                {
                    useDeck.Remove(useDeck[0]);
                    //GameManager.instance.photonView.RPC("EnemyCardChange", RpcTarget.Others, HandManager.Instance.GetHandCardNum());
                }
                else
                {
                    Debug.LogError("손패가 가득 찼습니다.");
                }
            }
            else
            {
                if (PlayerActionManager.instance.handCardCount < 5)
                {
                    //GameManager.instance.Lose();
                }
                Debug.Log(countOfDeck);
                Debug.Log("덱이 비었습니다.");
                GameManager.instance.Lose();
            }
            RefreshDeckCount();
        }
    }

    /// <summary>
    /// 덱에 특정 카드를 넣는 기능
    /// </summary>
    /// <param name="card"></param>
    public void Refill(Card card)
    {

        useDeck.Add(card);

        RefreshDeckCount();
    }

    /// <summary>
    /// 덱에서 특정 카드를 지우는 기능
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool RemoveDeckCard(Card card)
    {
        for (int i = 0; i < useDeck.Count; i++)
        {
            if (useDeck[i] == card)
            {
                grave.Add(useDeck[i]);
                useDeck.RemoveAt(i);
                RefreshGraveCount();
                RefreshDeckCount();
                return true;
            }
        }
        Debug.LogError($"덱에서 {card.cardName} 카드를 지우지 못했습니다. 덱에 해당 카드가 존재하는지 확인해주십시오.");
        RefreshDeckCount();
        return false;
    }

    public void RefreshDeckCount()
    {
        countOfDeck = useDeck.Count;
        GameManager.instance.photonView.RPC("EnemyDeckReduce", RpcTarget.Others, countOfDeck);
    }
    public void RefreshGraveCount()
    {
        countOfGrave = grave.Count;
    }

    public void RefreshEnemyGraveCount()
    {
        countOfEnemyGrave = enemyGrave.Count;
    }

    public void SortDeck()
    {
        List<int> idList = new List<int>();
        foreach (Card card in useDeck)
        {
            Card cardScript = card;
            if (cardScript != null && cardScript != new Card())
            {
                idList.Add(cardScript.frontDamage);
            }
        }
        idList.Sort();
        //return idList;

        sortedDeck = idList;
    }
}

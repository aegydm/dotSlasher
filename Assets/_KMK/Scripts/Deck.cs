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
    /// ?繹먮냱踰??곸궠?獄?쓣紐?????츎 ?곸궠?獄?
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
    /// int????嶺뚮씭??칰??繹먮끏????곸궠?獄?쓣紐?嶺뚮?理???곸궠?獄?
    /// ?熬곣뫗????믨퀡??뤆?쎛 ?띠럾???嶺??롪퍔????????袁⑤?獄?쓣紐??곌랜?亦?熬곣뫁夷???釉붋??琉우꽑???깅쾳
    /// ?繹먮냱逾?0???議온???믨퀡????????遊뷸뤆?쎛 ???깅굵 ?롪퍔???
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
                    Debug.LogError("??믨퀡??뤆?쎛 ?띠럾???嶺뚢돦?????덈펲.");
                }
            }
            else
            {
                if (PlayerActionManager.instance.handCardCount < 5)
                {
                    //GameManager.instance.Lose();
                }
                Debug.Log(countOfDeck);
                Debug.Log("?繹먮냱逾????닺굢????鍮??");
                GameManager.instance.Lose();
            }
            RefreshDeckCount();
        }
    }

    /// <summary>
    /// ?繹먮끏???獄????곸궠?獄?쓣紐??影?ル츎 ?リ옇???
    /// </summary>
    /// <param name="card"></param>
    public void Refill(Card card)
    {

        useDeck.Add(card);

        RefreshDeckCount();
    }

    /// <summary>
    /// ?繹먮끏????獄????곸궠?獄?쓣紐?嶺뚯솘???⑤베裕??リ옇???
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
        Debug.LogError($"?繹먮끏???{card.cardName} ?곸궠?獄?쓣紐?嶺뚯솘???? 嶺뚮쪇沅?쭛???鍮?? ?繹먮끏????????곸궠?獄?쑚泥? ?브퀡????濡ル츎嶺뚯솘? ?筌먦끉逾??怨삵룖?????");
        RefreshDeckCount();
        return false;
    }

    public void RefreshDeckCount()
    {
        countOfDeck = useDeck.Count;
    }
    public void RefreshGraveCount()
    {
        countOfGrave = grave.Count;
    }

    public void RefreshEnemyGraveCount()
    {
        countOfEnemyGrave = grave.Count;
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

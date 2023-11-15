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
    /// ?源놁벥 燁삳?諭띄몴???뚮뮉 燁삳?諭?
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
    /// int??揶?筌띾슦寃??源녿퓠??燁삳?諭띄몴?筌믩쵎??燁삳?諭?
    /// ?袁⑹삺 ?癒곕솭揶쎛 揶쎛??筌?野껋럩???癒?쑎?꾨뗀諭띄몴?癰귣?沅?袁⑥쨯 ??블??뤿선??됱벉
    /// ?源놁뵠 0?關?졿??癒곕솭?????癒?봺揶쎛 ??됱뱽 野껋럩??
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
                    Debug.LogError("?癒곕솭揶쎛 揶쎛??筌≪눘???덈뼄.");
                }
            }
            else
            {
                if (PlayerActionManager.instance.handCardCount < 5)
                {
                    //GameManager.instance.Lose();
                }
                Debug.Log(countOfDeck);
                Debug.Log("?源놁뵠 ??쑴肉??щ빍??");
                GameManager.instance.Lose();
            }
            RefreshDeckCount();
        }
    }

    /// <summary>
    /// ?源녿퓠 ?諭??燁삳?諭띄몴??節뗫뮉 疫꿸퀡??
    /// </summary>
    /// <param name="card"></param>
    public void Refill(Card card)
    {

        useDeck.Add(card);

        RefreshDeckCount();
    }

    /// <summary>
    /// ?源녿퓠???諭??燁삳?諭띄몴?筌왖?怨뺣뮉 疫꿸퀡??
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
        Debug.LogError($"?源녿퓠??{card.cardName} 燁삳?諭띄몴?筌왖?怨? 筌륁궢六??щ빍?? ?源녿퓠 ????燁삳?諭뜹첎? 鈺곕똻???롫뮉筌왖 ?類ㅼ뵥??곻폒?????");
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

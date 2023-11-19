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
    /// ?깆쓽 移대뱶瑜??욌뒗 移대뱶
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
    /// int??媛?留뚰겮 ?깆뿉??移대뱶瑜?戮묐뒗 移대뱶
    /// ?꾩옱 ?먰뙣媛 媛??李?寃쎌슦 ?먮윭肄붾뱶瑜?蹂대궡?꾨줉 ?ㅺ퀎?섏뼱?덉쓬
    /// ?깆씠 0?μ씠怨??먰뙣??鍮??먮━媛 ?덉쓣 寃쎌슦
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
                    Debug.Log("?먰뙣媛 媛??李쇱뒿?덈떎.");
                    break;
                }
            }
            else
            {
                if (PlayerActionManager.instance.handCardCount < 5)
                {
                    //GameManager.instance.Lose();
                }
                Debug.Log(countOfDeck);
                Debug.Log("?깆씠 鍮꾩뿀?듬땲??");
                GameManager.instance.Lose();
            }
            RefreshDeckCount();
        }
    }

    /// <summary>
    /// ?깆뿉 ?뱀젙 移대뱶瑜??ｋ뒗 湲곕뒫
    /// </summary>
    /// <param name="card"></param>
    public void Refill(Card card)
    {

        useDeck.Add(card);

        RefreshDeckCount();
    }

    /// <summary>
    /// ?깆뿉???뱀젙 移대뱶瑜?吏?곕뒗 湲곕뒫
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
        Debug.LogError($"?깆뿉??{card.cardName} 移대뱶瑜?吏?곗? 紐삵뻽?듬땲?? ?깆뿉 ?대떦 移대뱶媛 議댁옱?섎뒗吏 ?뺤씤?댁＜??떆??");
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

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
    public List<Card> grave =new List<Card>();
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
            }
        }
    }
    [SerializeField] int _countOfGrave;
    [SerializeField] int _countOfDeck;
    [SerializeField] public List<int> sortedDeck;
    [SerializeField] TMP_Text deckCountUI;

    public event Action OnDeckCountChanged;

    private void Start()
    {
        OnDeckCountChanged = null;
        //OnDeckCountChanged += RenderDeckCount;
        if(BuildManager.instance.Load("1",out originDeck) == false)
        //if(BuildManager.instance.Load(NetworkManager.instance.deckName, out originDeck) == false)
        {
            Debug.Log("Fail to Load Deck");
            //GameManager.Instance.Lose();
        }
        foreach(var card in originDeck)
        {
            if(card.cardCategory != CardCategory.hero)
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

    void RenderDeckCount()
    {
        deckCountUI.text = countOfDeck.ToString();
    }

    /// <summary>
    /// ???뷀뵆 湲곕뒫
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
    /// 移대뱶 ?쒕줈??湲곕뒫
    /// 紐뉗옣 ?쒕줈???좎? ?ㅼ젙?댁꽌 ?쒕줈???????덉쓬
    /// </summary>
    public void Draw(int drawCard)
    {

        for (int i = 0; i < drawCard; i++)
        {
            if (countOfDeck > 0)
            {
                //癒쇱? ?⑥뿉???깆쓽 移대뱶瑜??몄텧?섍퀬 ???ㅼ쓬 ?깆쓽 移대뱶瑜??쒓굅?섎룄濡??쒖꽌瑜?二쇱쓽?쒕떎.

                //if (HandManager.Instance.DrawCard(useDeck[0]))
                if (PlayerActionManager.instance.AddHandCard(useDeck[0]))
                {
                    useDeck.Remove(useDeck[0]);
                    //GameManager.Instance.photonView.RPC("EnemyCardChange", RpcTarget.Others, HandManager.Instance.GetHandCardNum());
                }
                else
                {
                    Debug.LogError("?먰뙣媛 媛??李쇱뒿?덈떎.");
                }
            }
            else
            {
                Debug.Log(countOfDeck);
                Debug.Log("移대뱶媛 ?놁뒿?덈떎");
                //GameManager.Instance.Lose();
            }
        }
        RefreshDeckCount();
    }

    /// <summary>
    /// ?꾨뱶??紐ъ뒪?곕? ?깆쑝濡??섎룎由щ뒗 湲곕뒫
    /// </summary>
    public void Refill(Card card)
    {

        useDeck.Add(card);

        RefreshDeckCount();
    }

    public bool RemoveDeckCard(Card card)
    {
        for(int i  = 0; i < useDeck.Count; i++)
        {
            if (useDeck[i] == card)
            {
                useDeck.RemoveAt(i);
                RefreshDeckCount();
                return true;
            }
        }
        Debug.LogError("?깆뿉 ?녿뒗 移대뱶瑜??쒓굅?섎젮怨??덉뒿?덈떎.");
        RefreshDeckCount();
        return false;
    }

    /// <summary>
    /// ?깆뿉 ?⑥? 移대뱶???뺤씤???⑥닔
    /// </summary>
    public void RefreshDeckCount()
    {
        countOfDeck = useDeck.Count;
    }
    public void RefreshGraveCount()
    {
        _countOfGrave = grave.Count;
    }

    /// <summary>
    /// ?뺣젹??移대뱶???꾩씠?붽? ?섏????깆쓽 ?쒖꽌? ?곴??놁씠 ?깆뿉 ?⑥? 移대뱶??醫낅쪟瑜??????덈떎.
    /// ?대? ?쒖슜?댁꽌 臾섏????덈뒗 移대뱶 由ъ뒪?몃? 諛쏆븘 ?묎컳? 肄붾뱶瑜?吏꾪뻾?쒕떎?대룄 寃곌낵瑜??살쓣 ???덈떎.
    /// ?쒖꽌源뚯? ?뚭퀬 ?띕떎硫?Sort ?⑥닔瑜??쒓굅?섍퀬 吏꾪뻾?섎㈃ ?쒕떎.
    /// </summary>
    /// <param name="deck"></param>
    /// <returns></returns>
    public void SortDeck()
    {
        List<int> idList = new List<int>();
        foreach (Card card in useDeck)
        {
            Card cardScript = card;
            if (cardScript != null)
            {
                idList.Add(cardScript.frontDamage);
            }
        }
        idList.Sort();
        //return idList;

        for (int i = 0; i < idList.Count; i++)
            Debug.Log(idList[i]);
    }

    /// <summary>
    /// ?쒖옉??援먯껜 湲곕뒫
    /// </summary>
    /// <param name="cards"></param>
    void Mulligan(Card[] cards)
    {

        for (int i = 0; i < cards.Length; i++)
        {
            Refill(cards[i]);
            HandManager.Instance.RemoveHand();
        }

        Draw(cards.Length);
    }

    public void Mulligan(HandCardObject[] handCardList)
    {

    }

    /// <summary>
    /// ?쒕줈???섏씠利덉뿉留??쒕줈?곌? 媛?ν븳 湲곕뒫
    /// </summary>
    void OneDraw()
    {
        switch (GameManager.Instance.gamePhase)
        {
            case GamePhaseOld.DrawPhase:
                Draw(1);
                break;

            default:
                break;
        }
    }
}

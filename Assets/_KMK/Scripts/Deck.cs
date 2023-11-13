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
    public Card myHero = new Hero();

    int countOfDeck
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
    /// 덱 셔플 기능
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
    /// 카드 드로우 기능
    /// 몇장 드로우 할지 설정해서 드로우 할 수 있음
    /// </summary>
    public void Draw(int drawCard)
    {

        for (int i = 0; i < drawCard; i++)
        {
            if (countOfDeck > 0)
            {
                //먼저 패에서 덱의 카드를 호출하고 난 다음 덱의 카드를 제거하도록 순서를 주의한다.

                //if (HandManager.Instance.DrawCard(useDeck[0]))
                if (PlayerActionManager.instance.AddHandCard(useDeck[0]))
                {
                    useDeck.Remove(useDeck[0]);
                    //GameManager.Instance.photonView.RPC("EnemyCardChange", RpcTarget.Others, HandManager.Instance.GetHandCardNum());
                }
                else
                {
                    Debug.LogError("손패가 가득 찼습니다.");
                }
            }
            else
            {
                Debug.Log(countOfDeck);
                Debug.Log("카드가 없습니다");
                //GameManager.Instance.Lose();
            }
        }
        RefreshDeckCount();
    }

    /// <summary>
    /// 필드의 몬스터를 덱으로 되돌리는 기능
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
        Debug.LogError("덱에 없는 카드를 제거하려고 했습니다.");
        RefreshDeckCount();
        return false;
    }

    /// <summary>
    /// 덱에 남은 카드수 확인용 함수
    /// </summary>
    public void RefreshDeckCount()
    {
        countOfDeck = useDeck.Count;
    }

    /// <summary>
    /// 정렬된 카드의 아이디가 나와서 덱의 순서와 상관없이 덱에 남은 카드의 종류를 알 수 있다.
    /// 이를 활용해서 묘지에 있는 카드 리스트를 받아 똑같은 코드를 진행한다해도 결과를 얻을 수 있다.
    /// 순서까지 알고 싶다면 Sort 함수를 제거하고 진행하면 된다.
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
    /// 시작패 교체 기능
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

    /// <summary>
    /// 드로우 페이즈에만 드로우가 가능한 기능
    /// </summary>
    void OneDraw()
    {
        switch (GameManager.Instance.gamePhase)
        {
            case GamePhase.DrawPhase:
                Draw(1);
                break;

            default:
                break;
        }
    }
}

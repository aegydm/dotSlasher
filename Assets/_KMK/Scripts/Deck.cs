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
        OnDeckCountChanged += RenderDeckCount;
        if(BuildManager.instance.Load(NetworkManager.instance.deckName, out originDeck) == false)
        {
            GameManager.Instance.Lose();
        }
        foreach(var card in originDeck)
        {
            Debug.Log(card.cardName);
            if(card.cardCategory != CardCategory.hero)
            {
                useDeck.Add(card);
            }
            else
            {
                myHero = card;
            }
        }
        //for (int i = 0; i < CardDB.instance.cards.Count; i++)
        //{
        //    deck.Add(CardDB.instance.cards[i]);
        //}
        RefreshDeckCount();
    }

    void RenderDeckCount()
    {
        deckCountUI.text = countOfDeck.ToString();
    }

    /// <summary>
    /// �� ���� ���
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
    /// ī�� ��ο� ���
    /// ���� ��ο� ���� �����ؼ� ��ο� �� �� ����
    /// </summary>
    public void Draw(int drawCard)
    {

        for (int i = 0; i < drawCard; i++)
        {
            if (countOfDeck > 0)
            {
                //���� �п��� ���� ī�带 ȣ���ϰ� �� ���� ���� ī�带 �����ϵ��� ������ �����Ѵ�.

                if (HandManager.Instance.DrawCard(useDeck[0]))
                {
                    useDeck.Remove(useDeck[0]);
                    GameManager.Instance.photonView.RPC("EnemyCardChange", RpcTarget.Others, HandManager.Instance.GetHandCardNum());
                }
                else
                {
                    Debug.LogError("���а� ���� á���ϴ�.");
                }
            }
            else
            {
                Debug.Log(countOfDeck);
                Debug.Log("ī�尡 �����ϴ�");
                GameManager.Instance.Lose();
            }
        }
        RefreshDeckCount();
    }

    /// <summary>
    /// �ʵ��� ���͸� ������ �ǵ����� ���
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
        Debug.LogError("���� ���� ī�带 �����Ϸ��� �߽��ϴ�.");
        RefreshDeckCount();
        return false;
    }

    /// <summary>
    /// ���� ���� ī��� Ȯ�ο� �Լ�
    /// </summary>
    public void RefreshDeckCount()
    {
        countOfDeck = useDeck.Count;
    }

    /// <summary>
    /// ���ĵ� ī���� ���̵� ���ͼ� ���� ������ ������� ���� ���� ī���� ������ �� �� �ִ�.
    /// �̸� Ȱ���ؼ� ������ �ִ� ī�� ����Ʈ�� �޾� �Ȱ��� �ڵ带 �����Ѵ��ص� ����� ���� �� �ִ�.
    /// �������� �˰� �ʹٸ� Sort �Լ��� �����ϰ� �����ϸ� �ȴ�.
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
    /// ������ ��ü ���
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
    /// ��ο� ������� ��ο찡 ������ ���
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

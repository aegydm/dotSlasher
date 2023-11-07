using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<Card> deck = new List<Card>();
    [SerializeField] int countOfDeck;
    [SerializeField] public List<int> sortedDeck;

    private void Awake()
    {
        
    }

    private void Start()
    {
        for(int i = 0; i < CardDB.instance.cards.Count; i++)
        {
            deck.Add(CardDB.instance.cards[i]);
        }
        RefreshDeckCount();
    }

    /// <summary>
    /// �� ���� ���
    /// </summary>
    public void Shuffle()
    {
        List<Card> list = new List<Card>();
        int listCount = deck.Count;
        for (int i = 0; i < listCount; i++)
        {
            int rand =Random.Range(0, deck.Count);
            list.Add(deck[rand]);
            deck.RemoveAt(rand);
        }
        deck = list;

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

                if (HandManager.Instance.DrawCard(deck[0]))
                {
                    deck.Remove(deck[0]);
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
            }
        }
        RefreshDeckCount();
    }

    /// <summary>
    /// �ʵ��� ���͸� ������ �ǵ����� ���
    /// </summary>
    public void Refill(Card card)
    {

        deck.Add(card);

        RefreshDeckCount();
    }

    /// <summary>
    /// ������ ī�带 �����ϴ� ���
    /// </summary>
    /// <param name="idx"></param>
    public void RemoveDeckCard(int idx)
    {
        deck.Remove(deck[idx]);

        RefreshDeckCount();
    }

    /// <summary>
    /// ���� ���� ī��� Ȯ�ο� �Լ�
    /// </summary>
    public void RefreshDeckCount()
    {
        countOfDeck = deck.Count;
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
        foreach (Card card in deck)
        {
            Card cardScript = card;
            if (cardScript != null)
            {
                idList.Add(cardScript.frontDamage);
            }
        }
        idList.Sort();
        //return idList;

        for (int i = 0;i < idList.Count;i++)
            Debug.Log(idList[i]);
    }

    /// <summary>
    /// ������ ��ü ���
    /// </summary>
    /// <param name="cards"></param>
    void Mulligan(Card[] cards)
    {

        for(int i = 0; i < cards.Length; i++)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<GameObject> deck = new List<GameObject>();
    [SerializeField] List<GameObject> sortedDeck;
    [SerializeField] int countOfDeck;

    /// <summary>
    /// �� ���� ���
    /// </summary>
    public void Shuffle()
    {
        List<GameObject> list = new List<GameObject>();
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
            if (deck.Count > 0)
            {
                //���� �п��� ���� ī�带 ȣ���ϰ� �� ���� ���� ī�带 �����ϵ��� ������ �����Ѵ�.

                deck.Remove(deck[0]);
            }
            else
            {
                Debug.Log("ī�尡 �����ϴ�");
            }
        }

        RefreshDeckCount();
    }

    /// <summary>
    /// �ʵ��� ���͸� ������ �ǵ����� ���
    /// </summary>
    public void Refill(GameObject card)
    {
        deck.Add(card);

        RefreshDeckCount();
    }

    /// <summary>
    /// ������ ī�带 �����ϴ� ���
    /// </summary>
    /// <param name="idx"></param>
    public void RemoveCard(int idx)
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

    ///<summary>
    /// ���� ���� ī�带 Ȯ���ϱ� ���� �ѹ� �������ִ� �Լ�
    /// </summary>
    public void SortDeck()
    {
        sortedDeck = deck;

        sortedDeck.Sort();

        sortedDeck.Reverse();
    }
}

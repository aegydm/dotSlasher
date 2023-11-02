using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<GameObject> deck = new List<GameObject>();
    [SerializeField] int countOfDeck;
    [SerializeField] public List<int> sortedDeck;

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
        foreach (GameObject card in deck)
        {
            TestCardScript cardScript = card.GetComponent<TestCardScript>(); // TestCardScript ���� ��ũ��Ʈ �̸����� �����ؾ� �մϴ�.
            if (cardScript != null)
            {
                idList.Add(cardScript.id);
            }
        }
        idList.Sort();
        //return idList;

        for (int i = 0;i < idList.Count;i++)
            Debug.Log(idList[i]);

    }
}

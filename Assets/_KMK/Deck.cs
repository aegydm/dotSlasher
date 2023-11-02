using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<GameObject> deck = new List<GameObject>();
    [SerializeField] List<GameObject> sortedDeck;
    [SerializeField] int countOfDeck;

    /// <summary>
    /// 덱 셔플 기능
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
    /// 카드 드로우 기능
    /// 몇장 드로우 할지 설정해서 드로우 할 수 있음
    /// </summary>
    public void Draw(int drawCard)
    {
        for (int i = 0; i < drawCard; i++)
        {
            if (deck.Count > 0)
            {
                //먼저 패에서 덱의 카드를 호출하고 난 다음 덱의 카드를 제거하도록 순서를 주의한다.

                deck.Remove(deck[0]);
            }
            else
            {
                Debug.Log("카드가 없습니다");
            }
        }

        RefreshDeckCount();
    }

    /// <summary>
    /// 필드의 몬스터를 덱으로 되돌리는 기능
    /// </summary>
    public void Refill(GameObject card)
    {
        deck.Add(card);

        RefreshDeckCount();
    }

    /// <summary>
    /// 덱에서 카드를 제거하는 기능
    /// </summary>
    /// <param name="idx"></param>
    public void RemoveCard(int idx)
    {
        deck.Remove(deck[idx]);

        RefreshDeckCount();
    }

    /// <summary>
    /// 덱에 남은 카드수 확인용 함수
    /// </summary>
    public void RefreshDeckCount()
    {
        countOfDeck = deck.Count;
    }

    ///<summary>
    /// 덱에 남은 카드를 확인하기 위해 한번 정렬해주는 함수
    /// </summary>
    public void SortDeck()
    {
        sortedDeck = deck;

        sortedDeck.Sort();

        sortedDeck.Reverse();
    }
}

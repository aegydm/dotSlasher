using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] List<Card> deck = new List<Card>();
    [SerializeField] int countOfDeck;
    [SerializeField] public List<int> sortedDeck;
    [SerializeField] CardDB cardDB;

    private void Start()
    {
        for(int i = 0; i < cardDB.cards.Count; i++)
        {
            deck.Add(cardDB.cards[i]);
        }
    }

    /// <summary>
    /// 덱 셔플 기능
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

                if (HandManager.Instance.DrawCard(deck[0]))
                {
                    deck.Remove(deck[0]);
                }
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
    public void Refill(Card card)
    {

        deck.Add(card);

        RefreshDeckCount();
    }

    /// <summary>
    /// 덱에서 카드를 제거하는 기능
    /// </summary>
    /// <param name="idx"></param>
    public void RemoveDeckCard(int idx)
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

    void Mulligan(Card[] cards)
    {

        for(int i = 0; i < cards.Length; i++)
        {
            HandManager.Instance.RemoveHand();
            Refill(cards[i]);
        }

        Draw(cards.Length);
    }
}

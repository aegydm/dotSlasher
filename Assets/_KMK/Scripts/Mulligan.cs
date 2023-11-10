using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

public class Mulligan : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] List<HandCard> selectedHand;
    [SerializeField] Deck deck;
    [SerializeField] HandManager handManager = HandManager.Instance;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        //if (GameManager.Instance.gamePhase != GamePhase.DrawPhase) return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            DoMulligan(hit);
        }
    }

    List<HandCard> DoMulligan(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            HandCard clickedHand = hit.collider.gameObject.GetComponent<HandCard>();

            if (selectedHand.Contains(clickedHand))
            {
                selectedHand.Remove(clickedHand);
            }
            else
            {
                selectedHand.Add(clickedHand);
            }
        }

        return selectedHand;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    //오브젝트에서 card 컴포넌트 추출
        //    Card clickedCard = hit.collider.gameObject.GetComponent<Card>();

        //    foreach (Card card in selectedCard)
        //    {
        //        if (clickedCard == card)
        //        {
        //            //배열에 카드 추가
        //            selectedCard.Remove(clickedCard);
        //        }
        //        else if ( clickedCard != card)
        //        {
        //            selectedCard.Add(clickedCard);
        //        }
        //    }
        //}
        //return selectedCard;
    }
    
    public void DoProcess()
    {
        for ( int i = 0; i < selectedHand.Count; i++)
        {
            //패의 카드를 덱 밑으로 돌린다.
            deck.Refill(selectedHand[i].card);

            //여기서 패의 카드를 제거하는 함수를 수행하여야 한다.
            handManager.RemoveHand(selectedHand[i]);
            
            //돌린 만큼 덱에서 카드를 뽑는다.
            if( i == selectedHand.Count)
            {
                deck.Draw(i);
            }
        }
        deck.Draw(selectedHand.Count);
        EndProcess();
    }

    void EndProcess()
    {
        selectedHand.Clear();
    }
}

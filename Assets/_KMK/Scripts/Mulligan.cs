using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;

public class Mulligan : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] List<Card> selectedCard;
    [SerializeField] Deck deck;
    [SerializeField] HandManager handManager;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        //ray 와 raycast 선언
        RaycastHit hit = new RaycastHit();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        DoMulligan(hit, ray);
    }

    List<Card> DoMulligan(RaycastHit hit , Ray ray)
    {
        if (Physics.Raycast(ray, out hit))
        {
            //오브젝트에서 card 컴포넌트 추출
            Card clickedCard = hit.collider.gameObject.GetComponent<Card>();

            foreach (Card card in selectedCard)
            {
                if (clickedCard == card)
                {
                    //배열에 카드 추가
                    selectedCard.Remove(clickedCard);
                }
                else if ( clickedCard != card)
                {
                    selectedCard.Add(clickedCard);
                }
            }
        }
        return selectedCard;
    }

    void DoProcess(List<Card> selectedCard)
    {

        for ( int i = 0; i < selectedCard.Count; i++)
        {
            //패의 카드를 덱 밑으로 돌린다.
            deck.Refill(selectedCard[i]);

            //여기서 패의 카드를 제거하는 함수를 수행하여야 한다.
            handManager.RemoveHand();
            
            //돌린 만큼 덱에서 카드를 뽑는다.
            if( i == selectedCard.Count)
            {
                deck.Draw(i);
            }
        }
    }
}

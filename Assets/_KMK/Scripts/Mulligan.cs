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
        //    //������Ʈ���� card ������Ʈ ����
        //    Card clickedCard = hit.collider.gameObject.GetComponent<Card>();

        //    foreach (Card card in selectedCard)
        //    {
        //        if (clickedCard == card)
        //        {
        //            //�迭�� ī�� �߰�
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
            //���� ī�带 �� ������ ������.
            deck.Refill(selectedHand[i].card);

            //���⼭ ���� ī�带 �����ϴ� �Լ��� �����Ͽ��� �Ѵ�.
            handManager.RemoveHand(selectedHand[i]);
            
            //���� ��ŭ ������ ī�带 �̴´�.
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

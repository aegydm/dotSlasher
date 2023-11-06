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
        //ray �� raycast ����
        RaycastHit hit = new RaycastHit();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        DoMulligan(hit, ray);
    }

    List<Card> DoMulligan(RaycastHit hit , Ray ray)
    {
        if (Physics.Raycast(ray, out hit))
        {
            //������Ʈ���� card ������Ʈ ����
            Card clickedCard = hit.collider.gameObject.GetComponent<Card>();

            foreach (Card card in selectedCard)
            {
                if (clickedCard == card)
                {
                    //�迭�� ī�� �߰�
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
            //���� ī�带 �� ������ ������.
            deck.Refill(selectedCard[i]);

            //���⼭ ���� ī�带 �����ϴ� �Լ��� �����Ͽ��� �Ѵ�.
            handManager.RemoveHand();
            
            //���� ��ŭ ������ ī�带 �̴´�.
            if( i == selectedCard.Count)
            {
                deck.Draw(i);
            }
        }
    }
}

using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class BildManager : MonoBehaviour
{
    [SerializeField] CardDB cardDB;
    [SerializeField] Camera cam;
    public List<int> myDeck = new List<int>(); //최종적으로 사용할 덱
    Card clickedCard;
    

    public static BildManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    /*private void Update()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            AddCard(hit, ray);
        }
        //우클릭
        if (Input.GetMouseButtonUp(1))
        {
            RemoveCard(hit, ray);
        }        
    }*/

    void AddCard(RaycastHit hit, Ray ray)
    {
        GameObject clickedObject = hit.collider.gameObject;

        clickedCard = gameObject.GetComponent<Card>();

        int cardID = clickedCard.cardID;

        int count = myDeck.Count(item => item == cardID);

        if (count <= 2)
        {
            myDeck.Add(cardID);
        }
        else
        {
            print("카드가 더 이상 들어가지 않습니다");
        }
    }

    void RemoveCard(RaycastHit hit, Ray ray)
    {
        GameObject clickedObject = hit.collider.gameObject;

        clickedCard = gameObject.GetComponent<Card>();

        int cardID = clickedCard.cardID;

        int count = myDeck.Count(item => item == cardID);

        if (count >= 0)
        {
            myDeck.Remove(cardID);
        }
        else
        {
            print("더 이상 카드가 존재하지 않습니다");
        }
    }
}

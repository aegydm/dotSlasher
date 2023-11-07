using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BildManager : MonoBehaviour
{
    [SerializeField] CardDB cardDB;
    [SerializeField] Camera cam;
    public List<int> myDeck; //최종적으로 사용할 덱
    

    public static BildManager instance;


    private void Update()
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
    }

    void AddCard(RaycastHit hit, Ray ray)
    {

    }

    void RemoveCard(RaycastHit hit, Ray ray)
    {

    }
}

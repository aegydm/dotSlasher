using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BildManager : MonoBehaviour
{
    [SerializeField] CardDB cardDB;
    [SerializeField] Camera cam;
    public List<int> myDeck; //���������� ����� ��
    

    public static BildManager instance;


    private void Update()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //��Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            AddCard(hit, ray);
        }
        //��Ŭ��
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

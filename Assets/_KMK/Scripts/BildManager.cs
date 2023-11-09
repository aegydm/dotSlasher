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
    public List<int> myDeck = new List<int>(); //���������� ����� ��
    public List<int> IDList = new List<int>();
    Card clickedCard;
    public SaveTest saveData;
    public DataFrame list;
    public DataFrame deckID = new DataFrame();
    

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

    private void Start()
    {
        saveData = GameObject.FindObjectOfType<SaveTest>();
    }

    /*private void Update()
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
            print("ī�尡 �� �̻� ���� �ʽ��ϴ�");
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
            print("�� �̻� ī�尡 �������� �ʽ��ϴ�");
        }
    }

    public void DeckLoad()
    {
        deckID = saveData.LoadData(saveData.path.ToString());
        deckID = list;
        List<int> IDList = list.ID;
        Debug.Log(IDList.Count);
        Debug.Log(list.ID.Count);
        IDList.Sort();

        for(int i = 0; i < IDList.Count; i++)
        {
            Debug.Log(IDList[i]);
            break;
        }
    }
}

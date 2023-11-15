using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

[System.Serializable]
public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public List<Card> deck;
    public string deckName;
    Card clickedCard;
    private string path;
    BinaryFormatter binaryFormatter = new();
    bool trigger = false;

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

    private void SetPathByDeckName(string deckName)
    {
        string name = deckName + ".data";
        path = Path.Combine(Application.dataPath, name);
    }

    public void Save(string deckName)
    {
        SetPathByDeckName(deckName);
        //List<Card>를 카드 ID를 가진 List<ID>로 변환해서 진행할 예정;
        //
        List<int> myDeck = new List<int>();
        for(int i = 0; i < deck.Count; i++)
        {
            myDeck.Add(deck[i].cardID);
        }

        OnSave(myDeck);

        //LoadData(path);
    }

    private void OnSave(List<int> dataFrame)
    {
        try
        {
            using (Stream ws = new FileStream(path, FileMode.Create))
            {
                binaryFormatter.Serialize(ws, dataFrame);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public List<Card> LoadData(string path)
    {
        List<int> loadDeck = new();

        try
        {
            using (Stream rs = new FileStream(path, FileMode.Open))
            {
                loadDeck = (List<int>)binaryFormatter.Deserialize(rs);
            }

            Debug.Log(loadDeck.ToString());
            trigger = true;
        }
        catch (Exception e)
        {
            //
            for (int i = 0; i < CardDB.instance.cards.Count; i++)
            {
                deck.Add(CardDB.instance.cards[i]);
            }
            Save("1");
            trigger = true;
            //빌드용 임시 스타터덱 코드
            //trigger = false;
            Debug.Log(e);
        }

        print(loadDeck.Count);

        List<Card> tmpDeck = new();

        //List<int>를 List<Card>로 변환시킬 예정;
        foreach(var data in loadDeck)
        {
            tmpDeck.Add(CardDB.instance.FindCardFromID(data));
        }
        return tmpDeck;
    }

    public void Load()
    {
        SetPathByDeckName(deckName);
        deck = LoadData(path);
    }

    public bool Load(string deckName, out List<Card> inputDeck)
    {
        SetPathByDeckName(deckName);
        inputDeck = deck = LoadData(path);
        return trigger;
    }

    //void AddCard(RaycastHit hit, Ray ray)
    //{
    //    GameObject clickedObject = hit.collider.gameObject;

    //    clickedCard = gameObject.GetComponent<Card>();

    //    int cardID = clickedCard.cardID;

    //    int count = myDeck.Count(item => item == cardID);

    //    if (count <= 2)
    //    {
    //        myDeck.Add(cardID);
    //    }
    //    else
    //    {
    //        print("카드가 더 이상 들어가지 않습니다");
    //    }
    //}

    //void RemoveCard(RaycastHit hit, Ray ray)
    //{
    //    GameObject clickedObject = hit.collider.gameObject;

    //    clickedCard = gameObject.GetComponent<Card>();

    //    int cardID = clickedCard.cardID;

    //    int count = myDeck.Count(item => item == cardID);

    //    if (count >= 0)
    //    {
    //        myDeck.Remove(cardID);
    //    }
    //    else
    //    {
    //        print("더 이상 카드가 존재하지 않습니다");
    //    }
    //}
}

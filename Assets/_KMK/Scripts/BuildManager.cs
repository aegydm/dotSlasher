using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;

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

    public SavedDeck SelectedSavedDeck;

    public GameObject gridLayout;

    public GameObject textObject;

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
        Delete("Assets/2.data");
    }


    private void SetPathByDeckName(string deckName)
    {
        string name = deckName + ".data";
        path = Path.Combine(Application.dataPath, name);
    }

    public void Save(string deckName)
    {
        SetPathByDeckName(deckName);
        //List<Card>??燁삳?諭?ID??揶쎛筌?List<ID>嚥?癰궰??묐퉸??筌욊쑵六????됱젟;
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

    public void Delete(string deckName)
    {
        if (File.Exists(deckName))
        {
            File.Delete(deckName);
            Debug.Log("File is Deleted");
        }
        else
        {
            Debug.Log("File doesn't exist");
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
            //??슢諭???袁⑸뻻 ????怨뺣쑔 ?꾨뗀諭?
            //trigger = false;
            Debug.Log(e);
        }

        print(loadDeck.Count);

        List<Card> tmpDeck = new();

        //List<int>??List<Card>嚥?癰궰??뤿뻻????됱젟;
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

    public void PrintSavedDeck()
    {
        GameObject GO;
        foreach(Card cards in SelectedSavedDeck.deck)
        {
            //GO = Instantiate()
        }
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
    //        print("燁삳?諭뜹첎? ????곴맒 ??쇰선揶쎛筌왖 ??녿뮸??덈뼄");
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
    //        print("????곴맒 燁삳?諭뜹첎? 鈺곕똻???? ??녿뮸??덈뼄");
    //    }
    //}
}

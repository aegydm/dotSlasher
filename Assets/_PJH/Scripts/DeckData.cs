using CCGCard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DeckData : MonoBehaviour
{
    public List<Card> deck;
    public string deckName;

    BinaryFormatter binaryFormatter = new();

    public void Update()
    {
        if(deck.Count == 0)
        {
            deck = LoadData("Assets/1.data");
            deckName = "1";
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
        }
        catch (Exception e)
        {
            //
            for (int i = 0; i < CardDB.instance.cards.Count; i++)
            {
                deck.Add(CardDB.instance.cards[i]);
            }
            //????????????獄쏅챶留덌┼???猿녿퉲??????????袁④뎬???????諛몃마????
            //trigger = false;
            Debug.Log(e);
        }

        print(loadDeck.Count);

        List<Card> tmpDeck = new();

        //List<int>??List<Card>???????곕츥?????轅붽틓??????됰뾼??????μ떜媛?걫???
        foreach (var data in loadDeck)
        {
            tmpDeck.Add(CardDB.instance.FindCardFromID(data));
        }
        return tmpDeck;
    }
}

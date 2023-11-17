using CCGCard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public List<Card> deck
    {
        get { return _deck; }
        set
        {
            _deck = value;
            DeckChanged?.Invoke(_deck);
        }
    }
    [SerializeField] private List<Card> _deck;
    public string deckName;
    Card clickedCard;
    private string path;
    BinaryFormatter binaryFormatter = new();
    bool trigger = false;

    [HideInInspector]
    public SavedDeck SelectedSavedDeck
    {
        get { return _SelectedSavedDeck; }
        set
        {
            if(value == null)
            {
                _SelectedSavedDeck = null;
                deck = null;
                return;
            }
            if (_SelectedSavedDeck == null)
            {
                _SelectedSavedDeck = value;
                deck = _SelectedSavedDeck.deck;
            }
            else
            {
                if(_SelectedSavedDeck != value)
                {
                    _SelectedSavedDeck = value;
                    foreach (GameObject text in texts)
                    {
                        Destroy(text);
                    }
                    texts.Clear();
                    deck = _SelectedSavedDeck.deck;
                }
            }
        }
    }
    private SavedDeck _SelectedSavedDeck;

    public GameObject gridLayout;

    public GameObject textObject;

    List<GameObject> texts = new();

    public event Action<List<Card>> DeckChanged;

    bool isDeckDisplayed = false;

    public SavedDeck[] saveDecks;

    const int FULL_DECK_COUNT = 6;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DeckChanged += PrintDeck;
    }


    private void Start()
    {
        LoadAll();
    }

    public void Update()
    {
        
    }

    private void SetPathByDeckName(string deckName)
    {
        string name = deckName + ".data";
        path = Path.Combine(Application.dataPath, name);
    }

    public void Save(string deckName)
    {
        SetPathByDeckName(deckName);
        //List<Card>???????몃뱥????ID???????ル뒌????List<ID>???????곕츥???????????耀붾굝??????癲ル슢?????????μ떜媛?걫???
        //
        List<int> myDeck = new List<int>();
        for(int i = 0; i < deck.Count; i++)
        {
            myDeck.Add(deck[i].cardID);
        }

        OnSave(myDeck);

        //LoadData(path);
    }

    public void Save()
    {
        if (IsDeckFull()) return;
        Debug.Log("Save Start");
        SavedDeck newDeck;
        if(SelectedSavedDeck == null)
        {
            newDeck = EnableEmptyDeck();
            SelectedSavedDeck = newDeck;
            SelectedSavedDeck.deck = DeckMaker.instance.deck;
        }
        else
        {
            if (!DeckExists(SelectedSavedDeck.deckName))
            {
                newDeck = EnableEmptyDeck();
                SelectedSavedDeck = newDeck;
                SelectedSavedDeck.deck = DeckMaker.instance.deck;
            }
        }
        string deckName = SelectedSavedDeck.deckName;
        SetPathByDeckName(deckName);
        //List<Card>???????몃뱥????ID???????ル뒌????List<ID>???????곕츥???????????耀붾굝??????癲ル슢?????????μ떜媛?걫???
        //
        List<int> myDeck = new List<int>();
        for (int i = 0; i < deck.Count; i++)
        {
            myDeck.Add(deck[i].cardID);
        }

        OnSave(myDeck);
        DeckMaker.instance.ErasePanel();
        DeckMaker.instance.isDeckMaking = false;
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

    public void Delete()
    {
        if (SelectedSavedDeck == null) return;
        string deckName = SelectedSavedDeck.deckName;
        ResetSelection();
        if (File.Exists($"Assets/{deckName}.data"))
        {
            File.Delete($"Assets/{deckName}.data");
            Debug.Log("File is Deleted");
        }
        else
        {
            Debug.Log("File doesn't exist");
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

    public void LoadAll()
    {
        int i = 1;
        foreach(SavedDeck deck in saveDecks)
        {
            if (File.Exists($"Assets/{i}.data"))
            {
                if (!DeckExists(i.ToString()))
                {
                    deck.gameObject.SetActive(true);
                }
                deck.deck = LoadData($"Assets/{i}.data");
                deck.deckName = i.ToString();
            }
            i++;
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
                _deck.Add(CardDB.instance.cards[i]);
            }
            Save("1");
            trigger = true;
            //????????????獄쏅챶留덌┼???猿녿퉲??????????袁④뎬???????諛몃마????
            //trigger = false;
            Debug.Log(e);
        }

        print(loadDeck.Count);

        List<Card> tmpDeck = new();

        //List<int>??List<Card>???????곕츥?????轅붽틓??????됰뾼??????μ떜媛?걫???
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

    public void PrintDeck(List<Card> deck)
    {
        if (deck == null) return;
        if (isDeckDisplayed)
        {
            foreach (GameObject text in texts)
            {
                Destroy(text);
            }
            texts.Clear();
        }
        foreach(Card card in deck)
        {
            GameObject GO;
            GO = Instantiate(textObject, gridLayout.transform);
            GO.GetComponentInChildren<TMP_Text>().text = card.cardName;
            texts.Add(GO);
        }
        isDeckDisplayed = true;
    }

    public SavedDeck EnableEmptyDeck()
    {
        foreach(SavedDeck deck in saveDecks)
        {
            if(deck.gameObject.activeSelf == false)
            {
                deck.gameObject.SetActive(true);
                return deck;
            }
        }
        return null;
    }

    public void ResetSelection()
    {
        for(int i = 0; i< texts.Count; i++)
        {
            Destroy(texts[i]);
        }
        texts.Clear();
        SelectedSavedDeck = null;
        isDeckDisplayed = false;
    }

    bool IsDeckFull()
    {
        int count = saveDecks.Where(deck => deck.gameObject.activeSelf).Count();
        return count == FULL_DECK_COUNT;
    }

    bool DeckExists(string deckName)
    {
        foreach(SavedDeck deck in saveDecks.Where(deck => deck.gameObject.activeSelf))
        {
            if(deck.deckName == deckName)
            {
                return true;
            }
        }
        return false;
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
    //        print("?????몃뱥???????猷몃??? ?????????????????????????ル뒌???耀붾굝????? ?????????????곸죩");
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
    //        print("??????????????몃뱥???????猷몃??? ????⑥ル??????? ?????????????곸죩");
    //    }
    //}
}

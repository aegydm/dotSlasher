using CCGCard;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class DeckMaker : MonoBehaviour
{
    public static DeckMaker instance;
    [Header("-----")]
    public List<Card> deck;

    public GameObject heroSelect;
    public GameObject gridLayout;
    public GameObject cardObject;
    public GameObject popUI;
    public Card selectCard
    {
        get
        {
            return _selectCard;
        }
        set
        {
            _selectCard = value;
            if (_selectCard != null && _selectCard != new Card())
            {
                selectCardChanged?.Invoke(selectCard);
            }
        }
    }

    public GameObject panel;

    public event Action<Card> selectCardChanged;

    [SerializeField] private Card _selectCard = null;

    [HideInInspector] public bool isDeckMaking = false;

    public GameObject selectObject
    {
        get
        {
            return _selectObject;
        }
        set
        {
            _selectObject = value;
        }
    }

    private GameObject _selectObject = null;

    [SerializeField] public GameObject selectingUI;
    [SerializeField] public GameObject editingUI;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    public void LoadCardFromDB()
    {
        if (panel != null)
        {
            if (panel.activeSelf == false)
                panel.SetActive(true);
        }
        if (deck == new List<Card>() || deck == null)
        {
            deck = new();
        }
        GameObject go;
        foreach (Card card in CardDB.instance.cards)
        {
            if (card.cardCategory != CardCategory.hero)
            {
                if (deck != null && deck[0].cardColor == CardType.f1)
                {
                    if (card.cardColor != CardType.f4)
                    {

                        go = Instantiate(cardObject, gridLayout.transform);
                        go.GetComponent<UICard>().cardData = card;
                        go.GetComponent<UICard>().spriteRenderer.sprite = card.cardSprite;
                    }
                }
                else if (deck != null && deck[0].cardColor == CardType.f4)
                {
                    if (card.cardColor != CardType.f1)
                    {

                        go = Instantiate(cardObject, gridLayout.transform);
                        go.GetComponent<UICard>().cardData = card;
                        go.GetComponent<UICard>().spriteRenderer.sprite = card.cardSprite;
                    }
                }
            }
        }
    }

    public void LoadCardFromDeck(List<Card> cards)
    {
        if (panel != null)
        {
            if (panel.activeSelf == false)
                panel.SetActive(true);
        }
        deck = cards;
        GameObject go;
        foreach (Card card in cards)
        {
            go = Instantiate(cardObject, gridLayout.transform);
            go.GetComponent<UICard>().cardData = card;
            go.GetComponent<Image>().sprite = go.GetComponent<SpriteRenderer>().sprite = card.cardSprite;
        }
    }

    public void NewDeckMaking()
    {
        heroSelect.SetActive(true);
        isDeckMaking = true;
        BuildManager.instance.ResetSelection();
        deck = new();
        //LoadCardFromDB();
        selectingUI.SetActive(false);
        editingUI.SetActive(true);
    }

    public void LoadCardFromDBF1()
    {
        if (panel != null)
        {
            if (panel.activeSelf == false)
                panel.SetActive(true);
        }
        if (deck == new List<Card>() || deck == null)
        {
            deck = new();
        }
        GameObject go;
        foreach (Card card in CardDB.instance.cards)
        {
            if (card.cardCategory != CardCategory.hero && card.cardColor != CardType.f4)
            {
                go = Instantiate(cardObject, gridLayout.transform);
                go.GetComponent<UICard>().cardData = card;
                go.GetComponent<UICard>().spriteRenderer.sprite = card.cardSprite;
            }
        }
        deck.Add(CardDB.instance.FindCardFromID(111000));
        if (BuildManager.instance != null)
        {
            BuildManager.instance.deck = deck;
        }
        heroSelect.SetActive(false);
    }

    public void LoadCardFromDBF4()
    {
        if (panel != null)
        {
            if (panel.activeSelf == false)
                panel.SetActive(true);
        }
        if (deck == new List<Card>() || deck == null)
        {
            deck = new();
        }
        GameObject go;
        foreach (Card card in CardDB.instance.cards)
        {
            if (card.cardCategory != CardCategory.hero && card.cardColor != CardType.f1)
            {
                go = Instantiate(cardObject, gridLayout.transform);
                go.GetComponent<UICard>().cardData = card;
                go.GetComponent<UICard>().spriteRenderer.sprite = card.cardSprite;
            }
        }
        deck.Add(CardDB.instance.FindCardFromID(141000));
        if (BuildManager.instance != null)
        {
            BuildManager.instance.deck = deck;
        }
        heroSelect.SetActive(false);
    }

    public void SavedDeckMaking()
    {
        if (BuildManager.instance.SelectedSavedDeck == null) return;
        isDeckMaking = true;
        deck = BuildManager.instance.deck;
        LoadCardFromDB();
        selectingUI.SetActive(false);
        editingUI.SetActive(true);
    }

    public void CancelDeckMaking()
    {
        isDeckMaking = false;
        ErasePanel();
#if UNITY_EDITOR
        if (BuildManager.instance.SelectedSavedDeck != null && System.IO.File.Exists($"Assets/" + BuildManager.instance.SelectedSavedDeck.name + ".data"))
#endif
#if UNITY_STANDALONE_WIN
            if (BuildManager.instance.SelectedSavedDeck != null && System.IO.File.Exists($"{Application.dataPath}/{BuildManager.instance.SelectedSavedDeck.name}.data"))
#endif
            {
                if (BuildManager.instance.DeckExists(BuildManager.instance.SelectedSavedDeck.name))
                {
                    BuildManager.instance.Delete();
                }
            }
        BuildManager.instance.ResetSelection();
        selectingUI.SetActive(true);
        editingUI.SetActive(false);
    }

    public void ErasePanel()
    {
        if (panel != null)
        {
            UICard[] UICards = panel.GetComponentsInChildren<UICard>();
            for (int i = 0; i < UICards.Length; i++)
            {
                Destroy(UICards[i].gameObject);
            }
            if (panel.activeSelf)
                panel.SetActive(false);
        }
    }
}

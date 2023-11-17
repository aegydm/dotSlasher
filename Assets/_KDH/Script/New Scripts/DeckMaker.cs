using CCGCard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckMaker : MonoBehaviour
{
    public static DeckMaker instance;
    [Header("?????밸븶??????蹂κ텤??????????뽯쨦?? DB????????怨쀫뎐???????ル늅獄???????덈쟿? ??μ떜媛?걫???????怨쀫뎐???????ル늅獄???μ떜媛?걫?繹먮씮異??戮?츐?????蹂κ텠嶺??????낆젵.")]
    public List<Card> deck;

    public GameObject gridLayout;
    public GameObject cardObject;
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

    [SerializeField] private GameObject selectingUI;
    [SerializeField] private GameObject editingUI;

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
        if(deck == new List<Card>() || deck == null)
        {
            deck = new();
        }
        GameObject go;
        foreach (Card card in CardDB.instance.cards)
        {
            go = Instantiate(cardObject, gridLayout.transform);
            go.GetComponent<UICard>().cardData = card;
            go.GetComponent<Image>().sprite = go.GetComponent<SpriteRenderer>().sprite = card.cardSprite;
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
        isDeckMaking = true;
        BuildManager.instance.ResetSelection();
        deck = new();
        LoadCardFromDB();
        selectingUI.SetActive(false);
        editingUI.SetActive(true);
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

using CCGCard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckMaker : MonoBehaviour
{
    public static DeckMaker instance;
    [Header("현재 나의 덱 입니다. DB에서 불러오면 빈칸, 덱에서 불러오면 덱으로 나옵니다.")]
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

    public event Action<Card> selectCardChanged;

    [SerializeField] private Card _selectCard = null;

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

    private void Awake()
    {
        instance = this;
    }

    public void LoadCardFromDB()
    {
        deck = new();
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
        deck = cards;
        GameObject go;
        foreach (Card card in CardDB.instance.cards)
        {
            go = Instantiate(cardObject, gridLayout.transform);
            go.GetComponent<UICard>().cardData = card;
            go.GetComponent<Image>().sprite = go.GetComponent<SpriteRenderer>().sprite = card.cardSprite;
        }
    }
}

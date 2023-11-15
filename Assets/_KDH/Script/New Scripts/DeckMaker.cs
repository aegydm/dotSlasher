using CCGCard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckMaker : MonoBehaviour
{
    public static DeckMaker instance;
    [Header("?袁⑹삺 ??륁벥 ????낅빍?? DB?癒?퐣 ?븍뜄???삠늺 ??뜆萸? ?源녿퓠???븍뜄???삠늺 ?源놁몵嚥???륁긿??덈뼄.")]
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
        if(panel != null)
        {
            if (panel.activeSelf == false)
                panel.SetActive(true);
        }
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

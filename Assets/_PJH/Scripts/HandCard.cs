using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;
using TMPro;

public class HandCard : MonoBehaviour
{
    [Header("Ä«µå")]
    [HideInInspector] public Card card;

    [Header("")]
    public bool isEmpty = true;
    public TMP_Text frontDamageText;
    public TMP_Text BackDamageText;

    private SpriteRenderer rend;
    public bool isSelected;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        RemoveCard();
    }

    private void Start()
    {
        //DrawCardData();
    }
    private void Update()
    {

    }

    public void SetCard(Card card)
    {
        if (isEmpty)
        {
            this.card = card;
            isEmpty = false;
            DrawCardData();
        }
    }

    public void RemoveCard()
    {
        card = null;
        isEmpty = true;
        frontDamageText.text = "";
        BackDamageText.text = "";
        rend.sprite = null;
    }

    void DrawCardData()
    {
        frontDamageText.text = card.frontDamage.ToString();
        BackDamageText.text = card.backDamage.ToString();

        if(rend != null)
        {
            rend.sprite = card.cardSprite;
        }
    }
}

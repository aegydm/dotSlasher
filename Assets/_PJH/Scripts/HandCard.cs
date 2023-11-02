using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;
using TMPro;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    [Header("Ä«µå")]
    public Card card;

    [Header("")]
    public bool isEmpty = true;
    public TMP_Text frontDamageText;
    public TMP_Text BackDamageText;

    private GameObject canvas;
    private SpriteRenderer rend;
    public bool isSelected;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>().gameObject;
    }

    private void Start()
    {
        DrawCardData();
        RemoveCard();
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
        canvas.SetActive(false);
        frontDamageText.text = "";
        BackDamageText.text = "";
        rend.sprite = null;
    }

    void DrawCardData()
    {
        canvas.SetActive(true);
        frontDamageText.text = card.frontDamage.ToString();
        BackDamageText.text = card.backDamage.ToString();

        if(rend != null)
        {
            rend.sprite = card.cardSprite;
        }
    }
}

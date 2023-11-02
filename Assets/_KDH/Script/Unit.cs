using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;

[System.Serializable]
public class Unit : MonoBehaviour
{
    public Card cardData;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public Unit()
    {
        this.cardData = new Card();
    }

    public void CardChange(Card newCard)
    {
        cardData = newCard;
        OnCardDataChanged?.Invoke(newCard);
    }

    public event Action<Card> OnCardDataChanged;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        OnCardDataChanged = null;
        OnCardDataChanged += Setting;
    }

    public void Setting(Card card)
    {
        spriteRenderer.sprite = card.cardSprite;
    }
}

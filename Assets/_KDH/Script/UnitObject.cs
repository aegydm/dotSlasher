using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;

[System.Serializable]
public class UnitObject : MonoBehaviour
{
    public Unit cardData;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    public Sprite squareSprite;
    public bool lookingLeft;
    public string playername;

    public UnitObject()
    {
        this.cardData = new Unit();
    }

    public void CardChange(Unit newCard)
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
        if(card.cardName != string.Empty || card.cardSprite == null)
        {
            spriteRenderer.sprite = card.cardSprite;
            if (lookingLeft)
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            spriteRenderer.sprite = squareSprite;
            spriteRenderer.flipX = false;
        }
    }
}

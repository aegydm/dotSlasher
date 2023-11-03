using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;

[RequireComponent(typeof(Field))]
[System.Serializable]
public class UnitObject : MonoBehaviour
{
    public Card cardData;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    public Sprite squareSprite;
    public bool lookingLeft
    {
        get
        {

            return _lookingLeft;
        }
        set
        {
            _lookingLeft = value;
            OnCardDataChanged?.Invoke(cardData);
        }
    }
    private bool _lookingLeft;


    public string playername;


    public void CardChange(Card newCard)
    {
        cardData = newCard;
        OnCardDataChanged?.Invoke(newCard);
    }

    public event Action<Card> OnCardDataChanged;

    private void Awake()
    {
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
            spriteRenderer.sprite = null;
            spriteRenderer.flipX = false;
        }
    }
}

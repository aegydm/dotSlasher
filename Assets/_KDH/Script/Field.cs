using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using TMPro;

public class Field : MonoBehaviour
{
    public bool isEmpty = true;
    public Card card;
    public SpriteRenderer spriteRenderer;
    public Field prevField;
    public Field nextField;

    public TMP_Text frontDamageText;
    public TMP_Text backDamageText;

    private void Awake()
    {
    }
    public void SetCard(Card newCard)
    {
        if (isEmpty)
        {
            card = newCard;
            isEmpty = false;
            spriteRenderer.sprite = card.cardSprite;
            frontDamageText.text = card.frontDamage.ToString();
            backDamageText.text = card.backDamage.ToString();
        }
    }
}

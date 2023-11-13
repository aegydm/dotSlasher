using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;

//[RequireComponent(typeof(Field))]
[System.Serializable]
public class UnitObject : MonoBehaviour
{
    public Card cardData;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
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

    public string playerID
    {
        get
        {
            return _playerID;
        }
        set
        {
            _playerID = value;
            //if(_playerID != "-1")
            //{
            //    GetComponent<FieldCardObjectTest>().playerColor.color = new Color(255 - (255 * int.Parse(playerID)), 255 * int.Parse(playerID), 0);
            //}
            //else
            //{
            //    GetComponent<FieldCardObjectTest>().playerColor.color = new Color(255, 255, 255);
            //}
        }
    }

    [SerializeField] private string _playerID;

    public void CardChange(Card newCard)
    {
        cardData = newCard;
        OnCardDataChanged?.Invoke(newCard);
    }

    public event Action<Card> OnCardDataChanged;

    private void Awake()
    {
        OnCardDataChanged = null;
        OnCardDataChanged += Setting;
    }
    public void Setting(Card card)
    {
        cardData = card;
        if(card.cardName != string.Empty)
        {
            spriteRenderer.sprite = card.cardSprite;
            if (lookingLeft)
            {
                spriteRenderer.flipX = true;
            }
            if(animator != null)
            {
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(card.animator);
            }
        }
        else
        {
            animator.runtimeAnimatorController = null;
            spriteRenderer.sprite = null;
            spriteRenderer.flipX = false;
        }
    }
}

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
    public Animator animator;
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


    public string playerName
    {
        get
        {
            return _playerName;
        }
        set
        {
            _playerName = value;
            if(_playerName != "-1")
            {
                GetComponent<Field>().playerColor.color = new Color(255 - (255 * int.Parse(playerName)), 255 * int.Parse(playerName), 0);
            }
            else
            {
                GetComponent<Field>().playerColor.color = new Color(255, 255, 255);
            }
        }
    }

    [SerializeField] private string _playerName;

    public void CardChange(Card newCard)
    {
        cardData = newCard;
        OnCardDataChanged?.Invoke(newCard);
    }

    public event Action<Card> OnCardDataChanged;

    public void Func1()
    {
        Func2();
        Func3();
        Func4();
    }

    public void Func2()
    {

    }

    public void Func3()
    {

    }

    public void Func4()
    {

    }

    private void Awake()
    {
        OnCardDataChanged = null;
        OnCardDataChanged += Setting;
    }

    public void Setting(Card card)
    {
        cardData = card;
        if(card.cardName != string.Empty || card.cardSprite == null)
        {
            spriteRenderer.sprite = card.cardSprite;
            if (lookingLeft)
            {
                spriteRenderer.flipX = true;
            }
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(card.animator);
        }
        else
        {
            animator = null;
            spriteRenderer.sprite = null;
            spriteRenderer.flipX = false;
        }
    }
}

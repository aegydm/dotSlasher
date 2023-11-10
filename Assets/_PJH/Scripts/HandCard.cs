using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using System;
using TMPro;
using UnityEngine.UI;

public class HandCard : MonoBehaviour
{
    [Header("ī��")]
    public Card card;

    [Header("")]
    public bool isEmpty = true;
    public TMP_Text frontDamageText;
    public TMP_Text BackDamageText;
    private Animator backAnimator;

    private GameObject canvas;
    private SpriteRenderer rend;
    public bool isSelected;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>().gameObject;
        backAnimator = GetComponent<Animator>();
    }

    private void Start()
    {

    }
    private void Update()
    {

    }

    public void SetCard(Card card)
    {
        if (card == null)
        {
            Debug.Log("ī�尡���̷�");
            return;
        }
        if (isEmpty)
        {
            this.card = card;
            Debug.Log($"{gameObject.name}: {this.card.cardName}");
            isEmpty = false;
            DrawCardData();
        }
    }

    public void RemoveCard()
    {
        backAnimator.runtimeAnimatorController = null;
        card = null;
        isEmpty = true;
        canvas.SetActive(false);
        isSelected = false;
        rend.sprite = null;
    }

    void DrawCardData()
    {
        Debug.Log($"Drawing {gameObject.name}: {this.card.cardName}");
        canvas.SetActive(true);
        frontDamageText.text = card.frontDamage.ToString();
        BackDamageText.text = card.backDamage.ToString();

        if(rend != null)
        {
            rend.sprite = card.cardSprite;
            backAnimator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(card.animator);
        }
    }
}

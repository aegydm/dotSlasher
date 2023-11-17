using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCGCard;
using TMPro;

public class UICard : MonoBehaviour
{
    public Card cardData
    {
        get
        {
            return _cardData;
        }
        set
        {
            _cardData = value;
            spriteRenderer.sprite = cardData.cardSprite;
            cardNameTXT.text = cardData.cardName;
            cardDescriptionTXT.text = cardData.skill + "\n" + cardData.skillContents;
            frontATKText.text = cardData.frontDamage.ToString();
            backATKText.text = cardData.backDamage.ToString();
        }
    }
    [SerializeField] Card _cardData;
    public HandCardObject handCardObject = null;
    public bool isSelected = false;
    public Image backGroundRenderer;
    public Image spriteRenderer;
    public TMP_Text cardNameTXT;
    public TMP_Text cardDescriptionTXT;
    public TMP_Text frontATKText;
    public TMP_Text backATKText;

    private void OnMouseOver()
    {
        if (isSelected == false)
        {
            backGroundRenderer.color = Color.red;

        }
    }

    private void OnMouseExit()
    {
        if (isSelected == false)
        {
            backGroundRenderer.color = Color.white;
        }
    }

    private void OnMouseDown()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.selectObject = gameObject;
            UIManager.Instance.selectCard = cardData;
        }
        else
        {
            DeckMaker.instance.selectObject = gameObject;
            DeckMaker.instance.selectCard = cardData;
        }
        if (handCardObject != null)
        {
            isSelected = !isSelected;
            if (isSelected)
            {
                backGroundRenderer.color = Color.gray;
            }
            else
            {
                backGroundRenderer.color = Color.white;
            }
            Debug.Log(handCardObject.name + "is " + isSelected);
        }
        else if (DeckMaker.instance != null)
        {
            DeckMaker.instance.deck.Add(cardData);
        }
    }
}

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
            cardDescriptionTXT.text = cardData.skillContents;
            frontATKText.text = cardData.frontDamage.ToString();
            backATKText.text = cardData.backDamage.ToString();
            if (cardData.skill != string.Empty)
            {
                switch (cardData.skill[0].ToString())
                {
                    case "1":
                        gemImage.color = Color.red;
                        break;
                    case "2":
                        gemImage.color = Color.green;
                        break;
                    case "3":
                        gemImage.color = Color.yellow;
                        break;
                    case "4":
                        gemImage.color = Color.cyan;
                        break;
                    case "5":
                        gemImage.color = Color.white;
                        break;
                }
                switch (cardData.skill[1].ToString())
                {
                    case "1":
                        rankText.text = "Ⅰ";
                        break;
                    case "2":
                        rankText.text = "Ⅱ";
                        break;
                    case "3":
                        rankText.text = "Ⅲ";
                        break;
                    case "4":
                        rankText.text = "Ⅳ";
                        break;
                    case "5":
                        rankText.text = "Ⅴ";
                        break;
                }
            }
            else
            {
                gemImage.color = Color.black;
                rankText.text = string.Empty;
            }
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
    public UIScript script;

    public Image gemImage;
    public TMP_Text rankText;

    private void Start()
    {
        script = FindAnyObjectByType<UIScript>();
    }
    private void OnMouseOver()
    {
        if (isSelected == false && script.optionWindow.activeSelf == false)
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
        if (script.optionWindow.activeSelf == false)
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
}

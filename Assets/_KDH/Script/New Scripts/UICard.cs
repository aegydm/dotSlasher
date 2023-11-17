using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCGCard;

public class UICard : MonoBehaviour
{
    public Card cardData;
    public HandCardObject handCardObject = null;
    public bool isSelected = false;
    public SpriteRenderer spriteRenderer;

    private void OnMouseOver()
    {
        if (isSelected == false)
        {
            GetComponent<Image>().color = Color.red;

        }
    }

    private void OnMouseExit()
    {
        if (isSelected == false)
        {
            GetComponent<Image>().color = Color.white;
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
                GetComponent<Image>().color = Color.gray;
            }
            else
            {
                GetComponent<Image>().color = Color.white;
            }
            Debug.Log(handCardObject.name + "is " + isSelected);
        }
        else if (DeckMaker.instance != null)
        {
            DeckMaker.instance.deck.Add(cardData);
            if (BuildManager.instance != null)
            {
                BuildManager.instance.deck = DeckMaker.instance.deck;
            }
        }
    }
}

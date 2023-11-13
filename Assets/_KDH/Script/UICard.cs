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
        GetComponent<Image>().color = Color.red;
    }

    private void OnMouseExit()
    {
        GetComponent<Image>().color = Color.white;
    }

    private void OnMouseDown()
    {
        UIManager.Instance.selectObject = gameObject;
        UIManager.Instance.selectCard = cardData;
        if(handCardObject != null )
        {
            isSelected = !isSelected;
            Debug.Log(handCardObject.name + "is " + isSelected);
        }
    }
}

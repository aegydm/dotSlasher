using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    public HandCard connectedHand;

    bool isSelected = false;    
    private void OnMouseOver()
    {
        GetComponent<Image>().color = Color.red;
    }

    private void OnMouseExit()
    {
        GetComponent<Image>().color = isSelected ? Color.red : Color.white;
    }

    private void OnMouseDown()
    {
        UIManager.Instance.selectObject = gameObject;
        UIManager.Instance.selectCard = GetComponent<UnitObject>().cardData;
        isSelected = !isSelected;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
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
        UIManager.Instance.selectCard = GetComponent<UnitObject>().cardData;
    }
}

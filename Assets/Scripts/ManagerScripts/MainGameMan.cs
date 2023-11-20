using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGameMan : MonoBehaviour
{
    public GameObject mulliganButton;
    public GameObject exitButton;
    public GameObject popupUI;
    public GameObject gridLayout;
    public GameObject cardObject;
    public Deck deck;
    public GameObject cardSelectPopUI;

    private void Awake()
    {
        UIManager.Instance.mulliganButton = mulliganButton;
        UIManager.Instance.exitButton = exitButton;
        UIManager.Instance.popUpUi = popupUI;
        UIManager.Instance.gridLayout = gridLayout;
        UIManager.Instance.cardObject = cardObject;
        UIManager.Instance.deck = deck;
        UIManager.Instance.cardSelectPopUI = cardSelectPopUI;
        UIManager.Instance.mulliganButton.GetComponent<Button>().onClick.AddListener(() => UIManager.Instance.EndMulligan());
        UIManager.Instance.exitButton.GetComponent<Button>().onClick.AddListener(() => UIManager.Instance.ClosePopup());
    }
}

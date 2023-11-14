using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public bool isPopUI = false;
    List<GameObject> cardList = new();
    [SerializeField] GameObject mulliganButton;
    [SerializeField] GameObject ExitButton;
    [SerializeField] GameObject popUpUi;
    [SerializeField] GameObject gridLayout;
    [SerializeField] GameObject cardObject;
    [SerializeField] Deck deck;
    public Card selectCard
    {
        get
        {
            return _selectCard;
        }
        set
        {
            _selectCard = value;
            if(_selectCard != null && _selectCard != new Card())
            {
                selectCardChanged?.Invoke(selectCard);
            }
        }
    }

    public void RemoveSelectObject()
    {
        Destroy(selectObject);
        selectCard = null;
    }

    public event Action<Card> selectCardChanged;

    [SerializeField] private Card _selectCard = null;

    public GameObject selectObject
    {
        get
        {
            return _selectObject;
        }
        set
        {
            _selectObject = value;
        }
    }

    private GameObject _selectObject = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("UIManager는 1개만 존재해야 합니다.");
            Destroy(this);
        }
    }

    public void TestPopUp()
    {
        PopupCard(deck.useDeck);
    }

    public void StartMulligan()
    {
        Debug.Log("StartMulligan");
        PopupCard(PlayerActionManager.instance.handCardObjectArray);
    }

    public void EndMulligan()
    {
        mulliganButton.SetActive(false);
        ExitButton.SetActive(true);
        ClosePopup();
        for(int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].GetComponent<UICard>().isSelected)
            {
                deck.Refill(cardList[i].GetComponent<UICard>().cardData);
                cardList[i].GetComponent<UICard>().handCardObject.cardData = null;
                deck.Draw(1);
                Destroy(cardList[i]);
            }
        }
        GameManager.instance.playerEnd = true;
    }

    public void PopupCard(List<Card> cardList)
    {
        if (isPopUI == false)
        {
            popUpUi.SetActive(true);
            isPopUI = true;
            GameObject go;
            foreach (Card card in cardList)
            {
                go = Instantiate(cardObject, gridLayout.transform);
                go.GetComponent<UICard>().cardData = card;
                go.GetComponent<Image>().sprite = go.GetComponent<SpriteRenderer>().sprite = card.cardSprite;
            }
        }
    }

    public void PopupCard(HandCardObject[] handList)
    {
        Debug.Log("PopupCard");
        if (isPopUI == false)
        {
            popUpUi.SetActive(true);
            isPopUI = true;
            GameObject go;
            foreach (var card in handList)
            {

                go = Instantiate(cardObject, gridLayout.transform);
                cardList.Add(go);
                go.GetComponent<UICard>().cardData = card.cardData;
                go.GetComponent<Image>().sprite = go.GetComponent<UICard>().spriteRenderer.sprite = card.cardSprite.sprite;
                go.GetComponent<UICard>().handCardObject = card;
            }
            mulliganButton.SetActive(true);
            ExitButton.SetActive(false);
        }
    }

    public void ClosePopup()
    {
        if (isPopUI)
        {
            foreach(Transform child in gridLayout.transform)
            {
                Destroy(child.gameObject);
            }
            isPopUI=false;
            popUpUi.SetActive(false);
        }
    }

    public void PopupSettingWindow()
    {
        if (isPopUI == false)
        {
            isPopUI = true;
        }
    }
}

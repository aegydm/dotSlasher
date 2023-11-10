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
    [SerializeField] GameObject popUpUi;
    [SerializeField] GameObject gridLayout;
    [SerializeField] GameObject cardObject;
    Deck deck;
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
        List<Card> tmpDeck;
        BuildManager.instance.Load("1", out tmpDeck);
        PopupCard(tmpDeck);
    }

    //private void Update()
    //{
    //    if (isPopUI)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Escape))
    //        {
    //            foreach (Transform child in gridLayout.transform)
    //            {
    //                Destroy(child.gameObject);
    //                isPopUI = false;
    //                popUpUi.SetActive(false);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (Input.GetKeyDown(KeyCode.Alpha9))
    //        {
    //            popUpUi.SetActive(true);
    //            if(deck == null)
    //            {
    //                deck = GetComponent<Deck>();
    //            }
    //            PopupCard(deck.useDeck);
    //        }
    //    }
    //}

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
                go.GetComponent<UnitObject>().Setting(card);
                go.GetComponent<Image>().sprite = go.GetComponent<SpriteRenderer>().sprite;
            }
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

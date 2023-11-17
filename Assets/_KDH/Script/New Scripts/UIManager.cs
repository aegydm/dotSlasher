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
    public List<GameObject> cardList = new();
    public List<GameObject> uiCardList = new();
    [SerializeField] GameObject mulliganButton;
    public GameObject exitButton;
    [SerializeField] GameObject popUpUi;
    [SerializeField] GameObject gridLayout;
    [SerializeField] GameObject cardObject;
    [SerializeField] Deck deck;
    [SerializeField] GameObject cardSelectPopUI;
    [SerializeField] GameObject settingPopUI;

    [Header("Sounds")]
    [SerializeField] private AudioClip PopUpSound;
    [SerializeField] private AudioClip CloseSound;
    [SerializeField] private AudioClip WindowPopupSound;
    public Card selectCard
    {
        get
        {
            return _selectCard;
        }
        set
        {
            _selectCard = value;
            if (_selectCard != null && _selectCard != new Card())
            {
                selectCardChanged?.Invoke(selectCard);
            }
        }
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

    public void RemoveSelectObject()
    {
        Destroy(selectObject);
        selectCard = null;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            //Debug.LogError("UIManager??1媛쒕쭔 議댁옱?댁빞 ?⑸땲??");
            Destroy(this);
        }
    }

    public void TestPopUp()
    {
        PopupCard(deck.useDeck);
    }

    public void StartMulligan()
    {
        //Debug.Log("StartMulligan");
        PopupCard(PlayerActionManager.instance.handCardObjectArray);
        exitButton.SetActive(false);
    }

    public void EndMulligan()
    {
        mulliganButton.SetActive(false);
        exitButton.SetActive(true);
        ClosePopup();
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].GetComponent<UICard>().isSelected)
            {
                deck.Refill(cardList[i].GetComponent<UICard>().cardData);
                cardList[i].GetComponent<UICard>().handCardObject.cardData = null;
                deck.Draw(1);
                Destroy(cardList[i]);
            }
        }
        FindObjectOfType<Timer>().StopTimer();
        GameManager.instance.playerEnd = true;
        uiCardList.Clear();
    }

    public void PopupCard(List<Card> cardList)
    {
        //Please Input UI On Sound Code
        //SoundManager.instance.PlayEffSound(PopUpSound);
        if (isPopUI == false)
        {
            popUpUi.SetActive(true);
            isPopUI = true;
            GameObject go;
            foreach (Card card in cardList)
            {
                go = Instantiate(cardObject, gridLayout.transform);
                go.GetComponent<UICard>().cardData = card;
                uiCardList.Add(go);
            }
        }
    }

    public void PopupCard(HandCardObject[] handList)
    {
        //Please Input UI On Sound Code
        //SoundManager.instance.PlayEffSound(PopUpSound);
        //Debug.Log("PopupCard");
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
                go.GetComponent<UICard>().handCardObject = card;
                uiCardList.Add(go);
            }
            mulliganButton.SetActive(true);
            exitButton.SetActive(false);
        }
    }

    public void ClosePopup()
    {
        //Please Input UI Off Sound Code
        //SoundManager.instance.PlayEffSound(CloseSound);
        if (isPopUI)
        {
            foreach (Transform child in gridLayout.transform)
            {
                Destroy(child.gameObject);
            }
            isPopUI = false;
            popUpUi.SetActive(false);
            uiCardList.Clear();
        }
    }

    public void PopupSettingWindow()
    {
        //Please Input UI On Sound Code
        //SoundManager.instance.PlayEffSound(WindowPopupSound);
        if (isPopUI == false)
        {
            isPopUI = true;
            popUpUi.SetActive(true);
            cardSelectPopUI.SetActive(false);
            settingPopUI.SetActive(true);
        }
    }

    public void CloseSettingWindow()
    {
        //SoundManager.instance.PlayEffSound(CloseSound);
        isPopUI = false;
        popUpUi.SetActive(false);
        cardSelectPopUI.SetActive(true);
        settingPopUI.SetActive(false);
    }
}

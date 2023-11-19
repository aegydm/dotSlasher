using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCGCard;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public bool isPopUI = false;
    public List<GameObject> cardList = new();
    public List<GameObject> uiCardList = new();
    public GameObject mulliganButton;
    public GameObject exitButton;
    public GameObject popUpUi;
    public GameObject gridLayout;
    public GameObject cardObject;
    public Deck deck;
    public GameObject cardSelectPopUI;
    public GameObject settingPopUI;
    public GameObject gameSettingPopUI;
    public GameObject soundPopUI;
    [Header("Sounds")]
    [SerializeField] Toggle masterToggle;
    [SerializeField] Slider masterSlider;
    [SerializeField] Toggle BGMToggle;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Toggle effToggle;
    [SerializeField] Slider effSlider;
    [SerializeField] GameObject endUI;
    public TMP_Text endText;

    //[SerializeField] private AudioClip PopUpSound;
    //[SerializeField] private AudioClip CloseSound;
    //[SerializeField] private AudioClip WindowPopupSound;

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
        uiCardList.Remove(selectObject);
        Destroy(selectObject);
        selectCard = null;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
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
        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            gameSettingPopUI.SetActive(true);
        }
        else
        {
            settingPopUI.SetActive(true);
        }
    }

    public void CloseSettingWindow()
    {
        //SoundManager.instance.PlayEffSound(CloseSound);
        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            gameSettingPopUI.SetActive(false);
        }
        else
        {
            settingPopUI.SetActive(false);
        }
    }

    public void PopupSoundWindow()
    {
        soundPopUI.SetActive(true);
    }

    public void CloseSoundWindow()
    {
        soundPopUI.SetActive(false);
    }

    public void SoundSetting()
    {
        SoundManager.instance.MasterMute(masterToggle.isOn);
        SoundManager.instance.MasterVolume(masterSlider.value);
        SoundManager.instance.BGMMute(BGMToggle.isOn);
        SoundManager.instance.BGMVolume(BGMSlider.value);
        SoundManager.instance.EffMute(effToggle.isOn);
        SoundManager.instance.EffVolume(effSlider.value);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void TurnOnEndUI()
    {
        endUI.SetActive(true);
    }

    public void TurnOffEndUI()
    {
        endUI.SetActive(false);
    }
}

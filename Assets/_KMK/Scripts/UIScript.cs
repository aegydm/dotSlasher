using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("UiWindow")]
    public GameObject optionWindow;
    public GameObject soundUIBackGround;
    public GameObject optionUIBackGround;
    public GameObject blank;
    public bool windowOn = false;
    bool soundWindowOn = false;
    [Header("Button")]
    public Button soundMenuCloseButton;
    public Button optionMenuCloseButton;
    public Button soundWindowSwitchButton;
    [Header("Slider")]
    public Slider mVol;
    public Slider bgmVol;
    public Slider effVol;
    [Header("Toggle")]
    public Toggle mToggle;
    public Toggle bgmToggle;
    public Toggle effToggle;
    [Header("AudioSource")]
    public AudioSource bgmPlayer;
    public AudioSource effPlayer;

    void Start()
    { 

        UIConnect();
    }

    void Update()
    {
        UIClose();
    }

    private void UIClose()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopSoundWindow();
        }
    }

    public void PopSoundWindow()
    {
        if (optionWindow.activeSelf)
        {
            if (soundUIBackGround != null && soundUIBackGround.activeSelf)
            {
                if (soundUIBackGround.activeSelf)
                {
                    //SoundManager.instance.SoundWindowSwich();
                }
                optionWindow.SetActive(false);
            }
        }
    }

    private void UIOpen()
    {       
        if(!optionWindow.activeSelf)
        {
            optionWindow.SetActive(true);
        }
        
    }

    public void UIConnect()
    {

        //soundWindowSwitchButton.onClick.AddListener(() => SoundManager.instance.SoundWindowSwich());
    }
}
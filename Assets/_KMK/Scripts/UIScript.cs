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
    public Button button;

    void Start()
    { 
        soundUIBackGround = SoundManager.instance.soundWindow;

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
                    SoundManager.instance.SoundWindowSwich();
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

        button.onClick.AddListener(() => SoundManager.instance.SoundWindowSwich());
    }
}
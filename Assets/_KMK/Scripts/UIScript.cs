using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("UI 메뉴창")]
    public GameObject optionWindow;
    public GameObject soundUIBackGround;
    public Button button;

    private void Awake()
    {
        soundUIBackGround = SoundManager.instance.soundWindow;

        UIConnect();
    }

    void Start()
    { 
    }

    void Update()
    {
        UIClose();
    }

    private void UIClose()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionWindow.activeSelf)
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
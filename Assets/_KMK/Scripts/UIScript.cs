using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("UI Window")]
    public GameObject optionWindow;
    public GameObject soundUIBackGround;
    public Button button;

    [Header("AudioClip")]
    [SerializeField] AudioClip windowOpenSound;
    [SerializeField] AudioClip windowCloseSound;


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

                SoundManager.instance.PlayEffSound(windowCloseSound);
            }
        }
    }

    private void UIOpen()
    {       
        if(!optionWindow.activeSelf)
        {
            optionWindow.SetActive(true);
            SoundManager.instance.PlayEffSound(windowOpenSound);
        }
        
    }

    public void UIConnect()
    {

        button.onClick.AddListener(() => SoundManager.instance.SoundWindowSwich());
    }
}
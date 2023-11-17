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

    void Start()
    { 
        soundUIBackGround = SoundManager.instance.soundWindow;

        UIConnect();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
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
                SoundManager.instance.SoundWindowOn();
            }
            optionWindow.SetActive(false);
        }
        else
        {
            optionWindow.SetActive(true);
        }
    }

    public void UIConnect()
    {

        button.onClick.AddListener(() => SoundManager.instance.SoundWindowOn());
    }
}
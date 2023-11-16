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
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (optionWindow.activeSelf)
            {
                if(soundUIBackGround != null && soundUIBackGround.activeSelf)
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
    }

    public void UIConnect()
    {

        button.onClick.AddListener(() => SoundManager.instance.SoundWindowOn());
    }
}
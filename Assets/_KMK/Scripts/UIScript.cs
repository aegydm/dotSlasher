using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("UI 메뉴")]
    public GameObject soundUIBackGround;
    public GameObject optionUIBackGround;
    public GameObject blank;
    public bool windowOn = false;
    bool soundWindowOn = false;
    [Header("닫기 버튼")]
    public Button soundMenuCloseButton;
    public Button optionMenuCloseButton;
    [Header("볼륨 슬라이더")]
    public Slider mVol;
    public Slider bgmVol;
    public Slider effVol;
    [Header("음소거 버튼")]
    public Toggle mToggle;
    public Toggle bgmToggle;
    public Toggle effToggle;
    [Header("오디오 소스")]
    public AudioSource bgmSource;
    public AudioSource efffSource;


    // Update is called once per frame
    void Update()
    {
        BGMVolChanger();
        EffVolChanger();

        if (optionUIBackGround !=null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (windowOn)
            {
                if (soundWindowOn)
                {
                    soundUIBackGround.SetActive(false);
                    soundWindowOn = false;
                }
                soundUIBackGround.SetActive(false );
                blank.SetActive(false);
                windowOn = false;
            }
            else if(!windowOn)
            {
                blank.SetActive(true); 
                windowOn = true;
            }
        }
        else if (optionUIBackGround == null && Input.GetKeyDown(KeyCode.Escape))
        {
            //사운드 관련 UI를 끄고 키는 함수.
            SoundUIOn();
        }
    }

    public void BGMVolChanger()
    {
        if (!mToggle.isOn)
        {
            if (!bgmToggle.isOn)
            {
                bgmSource.volume = bgmVol.value * mVol.value;
            }
            else
            {
                bgmSource.volume = 0;
            }
        }
        else
        {
            mVol.value = 0;
            bgmSource.volume = 0;
        }
    }

    public void EffVolChanger()
    {
        if (!mToggle.isOn)
        {
            if (!effToggle.isOn)
            {
                efffSource.volume = effVol.value * mVol.value;
            }
            else
            {
                efffSource.volume = 0;
            }
        }
        else
        {
            mVol.value = 0;
            efffSource.volume = 0;
        }
    }

    public void BgmPlay()
    {
        bgmSource.Play();
    }

    public void SoundBoolChange(bool isOn)
    {
        if (isOn)
        {
            isOn = false;
        }
        else
        {
            isOn = true;
        }
    }

    public void OptionBoolChange()
    {
        if (windowOn)
        {
            windowOn = false;
        }
        else
        {
            windowOn = true;
        }
    }

    public void SoundUIOn()
    {
        if (!soundWindowOn)
        {
            soundUIBackGround.SetActive(true);
            soundWindowOn = true;
        }
        else
        {
            soundUIBackGround.SetActive(false);
            soundWindowOn = false;
        }
    }
}
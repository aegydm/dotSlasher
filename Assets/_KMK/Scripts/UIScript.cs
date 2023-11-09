using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("�޴� â")]
    public GameObject soundUIBackGround;
    public GameObject optionUIBackGround;
    public GameObject blank;
    public bool windowOn = false;
    public bool soundWindowOn;
    [Header("â �ݱ�")]
    public Button soundMenuCloseButton;
    public Button optionMenuCloseButton;
    [Header("���� �����̴�")]
    public Slider mVol;
    public Slider bgmVol;
    public Slider effVol;
    [Header("���Ұ� ��ư")]
    public Toggle mToggle;
    public Toggle bgmToggle;
    public Toggle effToggle;
    [Header("����� �ҽ�")]
    public AudioSource bgmPlayer;
    public AudioSource effPlayer;

    // Start is called before the first frame update
    void Start()
    {
        bgmPlayer.clip = SoundManager.instance.BGM;
        effPlayer.clip = SoundManager.instance.effectSound;
    }

    // Update is called once per frame
    void Update()
    {
        BGMVolChanger();
        EffVolChanger();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (windowOn)
            {
                if (soundWindowOn)
                {
                    BoolChange();
                    soundUIBackGround.SetActive(false);
                }
                blank.SetActive(false);
                windowOn = false;
            }
            else if(!windowOn)
            {
                blank.SetActive(true); 
                windowOn = true;
            }
        }
    }

    public void BGMVolChanger()
    {
        if (mToggle.isOn)
        {
            if (bgmToggle.isOn)
            {
                bgmPlayer.volume = bgmVol.value * mVol.value;
            }
            else
            {
                bgmPlayer.volume = 0;
            }
        }
        else
        {
            mVol.value = 0;
            bgmPlayer.volume = 0;
        }
    }

    public void EffVolChanger()
    {
        if (mToggle.isOn)
        {
            if (bgmToggle.isOn)
            {
                effPlayer.volume = effVol.value * mVol.value;
            }
            else
            {
                effPlayer.volume = 0;
            }
        }
        else
        {
            mVol.value = 0;
            bgmPlayer.volume = 0;
        }
    }

    public void BgmPlay()
    {
        bgmPlayer.Play();
    }

    public void BoolChange()
    {
        if (soundWindowOn)
        {
            soundWindowOn = false;
        }
        else if (!soundWindowOn)
        {
            soundWindowOn = true;
        }
    }
}
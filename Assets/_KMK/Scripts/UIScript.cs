using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("硫붾돱 李?")]
    public GameObject soundUIBackGround;
    public GameObject optionUIBackGround;
    public GameObject blank;
    public bool windowOn = false;
    bool soundWindowOn = false;
    [Header("李??リ린")]
    public Button soundMenuCloseButton;
    public Button optionMenuCloseButton;
    [Header("?뚮웾 ?щ씪?대뜑")]
    public Slider mVol;
    public Slider bgmVol;
    public Slider effVol;
    [Header("?뚯냼嫄?踰꾪듉")]
    public Toggle mToggle;
    public Toggle bgmToggle;
    public Toggle effToggle;
    [Header("?ㅻ뵒???뚯뒪")]
    public AudioSource bgmPlayer;
    public AudioSource effPlayer;

    // Start is called before the first frame update
    void Start()
    {

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
}
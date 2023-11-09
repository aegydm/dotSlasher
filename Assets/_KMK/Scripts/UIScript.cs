using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("메뉴 창")]
    public Image soundUIBackGround;
    public Image optionUIBackGround;
    [Header("창 닫기")]
    public Button soundMenuCloseButton;
    public Button optionMenuCloseButton;
    [Header("음량 슬라이더")]
    public Slider mVol;
    public Slider bgmVol;
    public Slider effVol;
    [Header("음소거 버튼")]
    public Toggle mToggle;
    public Toggle bgmToggle;
    public Toggle effToggle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BGMVolChanger()
    {
        if (mToggle.isOn)
        {
            if (bgmToggle.isOn)
            {

                SoundManager.instance.BGM.volume = bgmVol.value;
            }
        }
    }

    private void EffVolChanger()
    {
        if (mToggle.isOn)
        {
            if (bgmToggle.isOn)
            {

                SoundManager.instance.effectSound.volume = effVol.value;
            }
        }
    }
}
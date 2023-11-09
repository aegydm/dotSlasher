using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [Header("�޴� â")]
    public Image soundUIBackGround;
    public Image optionUIBackGround;
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
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }

    [Header("사운드 슬라이드")]
    public Slider mVol;
    public Slider bgmVol;
    public Slider effVol;
    [Header("음소거 토글")]
    public Toggle mToggle;
    public Toggle bgmToggle;
    public Toggle effToggle;
    [Header("AudioSource")]
    public AudioSource bgmAudio;
    public AudioSource effAudio;

    public GameObject soundWindow;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("FieldManager is already exist.");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        BGMVolChanger();
        EffVolChanger();
        
    }
    public void BGMVolChanger()
    {
        if (!mToggle.isOn)
        {
            bgmAudio.mute = false;

            if (!bgmToggle.isOn)
            {
                bgmAudio.volume = bgmVol.value * mVol.value;
            }
            else
            {
                if (bgmAudio.mute == false)
                {
                    bgmAudio.mute = true;
                }
                else if (bgmAudio.mute == true)
                {
                    bgmAudio.mute = false;
                }
            }
        }
        else
        {
            bgmAudio.mute = true;
        }
    }

    public void EffVolChanger()
    {
        if (!mToggle.isOn)
        {
            effAudio.mute = false;

            if (!effToggle.isOn)
            {
                effAudio.volume = effVol.value * mVol.value;
            }
            else
            {
                if (effAudio.mute == false)
                {
                    effAudio.mute = true;
                }
                else if (effAudio.mute == true)
                {
                    effAudio.mute = false;
                }
            }
        }
        else
        {
            effAudio.mute = true;
        }
    }

    public void PlayBGMSound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("BGM");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = mVol.value * bgmVol.value;
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }

    public void PlayEffSound(AudioClip clip)
    {
        GameObject soundObject = new GameObject("Effect");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = mVol.value * effVol.value;
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }

    public void PauseSound()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    //soundUI 호출용 함수
    public void SoundWindowSwich()
    {
        if (soundWindow.activeSelf)
        {
            soundWindow.SetActive(false);
        }
        else
        {
            soundWindow.SetActive(true);
        }
    }
}

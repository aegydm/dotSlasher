using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }

    [Header("?뚮웾 ?щ씪?대뜑")]
    public Slider mVol;
    public Slider bgmVol;
    public Slider effVol;
    [Header("?뚯냼嫄?踰꾪듉")]
    public Toggle mToggle;
    public Toggle bgmToggle;
    public Toggle effToggle;

    public GameObject soundWindow;

    AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        if (soundWindow.activeSelf)
        {
            if (mToggle.isOn == false)
            {
                mVol.value = 0;
            }
            if (bgmToggle.isOn == false)
            {
                bgmVol.value = 0;
            }
            if (effToggle.isOn == false)
            {
                effVol.value = 0;
            }
        }
    }

    public void PlayBGMSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = mVol.value * bgmVol.value;
        audioSource.Play();
    }

    public void PlayEffSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = mVol.value * effVol.value;
        audioSource.Play();
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
}

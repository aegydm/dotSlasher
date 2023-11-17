using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }

    [Header("?????????????")]
    public Slider mVol;
    public Slider bgmVol;
    public Slider effVol;
    [Header("???爰뽪ㅀ??뺢퀗???")]
    public Toggle mToggle;
    public Toggle bgmToggle;
    public Toggle effToggle;

    public GameObject soundWindow;

    [SerializeField] AudioSource BGMAudioSource;
    [SerializeField] AudioSource EffAudioSource;

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
        BGMAudioSource.clip = clip;
        BGMAudioSource.volume = mVol.value * bgmVol.value;
        BGMAudioSource.Play();
    }

    public void PlayEffSound(AudioClip clip)
    {
        EffAudioSource.clip = clip;
        EffAudioSource.volume = mVol.value * effVol.value;
        EffAudioSource.Play();
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

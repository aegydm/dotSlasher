using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }


    float masterVol = 0.5f;
    float bgmVol = 1;
    float effVol = 1;
    bool masterMute;
    bool bgmMute;
    bool effMute;
    [Header("AudioSource")]
    public AudioSource bgmAudio;
    public AudioSource effAudio;
    [Header("-------------")]
    [Header("Sound Clips")]
    [Header("-------------")]

    [Header("  BGM")]
    public AudioClip mainBGM;
    public AudioClip deckBGM;
    public AudioClip matchBGM;
    public AudioClip gameBGM;

    [Header("  Deck Sounds")]
    public AudioClip addCard;
    public AudioClip removeCard;
    public AudioClip deckComplete;
    public AudioClip deckError;
    public AudioClip deckButton;

    [Header("  RSP Sounds")]
    public AudioClip handSelect;
    public AudioClip victoryRSP;
    public AudioClip loseRSP;
    public AudioClip gameStart;

    [Header("  Game Sounds")]
    public AudioClip turnStart;
    public AudioClip phaseStart;
    public AudioClip victoryGame;
    public AudioClip cardDraw;
    public AudioClip battleStart;

    [Header("  UI Sounds")]
    public AudioClip buttonClick;
    public AudioClip mouseClick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError("SoundManager is already exist.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBGMSound(mainBGM);
        EffVolChanger();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayEffSound(mouseClick);
        }
    }

    public void MasterMute(bool mute)
    {
        masterMute = mute;
        BGMVolChanger();
        EffVolChanger();
    }

    public void BGMMute(bool mute)
    {
        bgmMute = mute;
        BGMVolChanger();
    }

    public void EffMute(bool mute)
    {
        effMute = mute;
        EffVolChanger();
    }

    public void MasterVolume(float volume)
    {
        masterVol = volume;
        BGMVolChanger();
        EffVolChanger();
    }

    public void BGMVolume(float volume)
    {
        bgmVol = volume;
        BGMVolChanger();
    }

    public void EffVolume(float volume)
    {
        effVol = volume;
        EffVolChanger();
    }



    public void BGMVolChanger()
    {
        if(masterMute || bgmMute)
        {
            bgmAudio.mute = true;
        }
        else
        {
            bgmAudio.mute = false;
        }

        bgmAudio.volume = masterVol * bgmVol;
    }

    public void EffVolChanger()
    {
        if (masterMute || effMute)
        {
            effAudio.mute = true;
        }
        else
        {
            effAudio.mute = false;
        }

        effAudio.volume = masterVol * effVol;
    }

    public void PlayBGMSound(AudioClip clip)
    {
        bgmAudio.clip = clip;
        BGMVolChanger();
        bgmAudio.Play();
    }

    public void PlayEffSound(AudioClip clip)
    {
        effAudio.PlayOneShot(clip);
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

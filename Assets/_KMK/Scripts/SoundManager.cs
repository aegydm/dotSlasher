using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }

    public AudioClip BGM;
    public AudioClip effectSound;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

    }

    public void BGMPlay()
    {

    }

    public void EffectSoundPlay()
    {

    }
}

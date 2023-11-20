using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneNames
{
    Title = 0,
    Main,
    DeckBuild,
    MatchMaking,
    MainGame,
}


public class SceneLoadManager : MonoBehaviourPunCallbacks
{
    public static SceneLoadManager instance;
    [SerializeField] GameObject[] LoadingCanvas;
    [SerializeField] SceneNames nextSceneNames;
    [SerializeField] Slider progressSlider;
    [SerializeField] TextMeshProUGUI textProgressData;
    [SerializeField] float progressTime;
    [SerializeField] GameObject isConnectText;
    [SerializeField] GameObject settingUI;

    private bool isConnected;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            Debug.LogError("SceneLoadManager can't over 1");
        }
    }

    private void Start()
    {
        SystemSetup();
        SceneManager.sceneLoaded += SceneLoaded;
    }

    public override void OnConnected()
    {
        base.OnConnected();
        if (SceneManager.GetActiveScene().name == "Title")
        {
            isConnected = true;
            isConnectText.gameObject.SetActive(true);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        isConnected = false;
        isConnectText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isConnected && Input.GetMouseButtonDown(0) && GetActiveScene() == "Title")
        {
            LoadSceneAsync("Main");
        }
    }

    public void LoadSceneAsync()
    {
        foreach (GameObject go in LoadingCanvas)
        {
            go.SetActive(true);
        }
        if (nextSceneNames != SceneNames.Title)
        {
            isConnectText.SetActive(false);
        }
        else
        {
            isConnectText.SetActive(true);
        }
        Play(LoadNextScene);
    }

    public void LoadSceneAsync(string nextSceneName)
    {
        nextSceneNames = (SceneNames)Enum.Parse(typeof(SceneNames), nextSceneName);
        LoadSceneAsync();
    }

    private void SystemSetup()
    {
        Application.runInBackground = true;

        int width = 1920;
        int height = 1080;
        Screen.SetResolution(width, height, false);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public static string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadNextScene()
    {
        LoadScene(nextSceneNames);
    }

    public static void LoadScene(string sceneName = "")
    {
        if (sceneName == "")
        {
            SceneManager.LoadScene(GetActiveScene());
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public static void LoadScene(SceneNames sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }
    public void Play(UnityAction action = null)
    {
        StartCoroutine(OnProgress(action));
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject go in LoadingCanvas)
        {
            go.SetActive(false);
        }
        if (scene.name != "Title" && scene.name != "MatchMaking")
        {
            settingUI.SetActive(true);
        }
        else
        {
            UIManager.Instance.CloseSoundWindow();
            UIManager.Instance.CloseSettingWindow();
            settingUI.SetActive(false);
        }

        if (scene.name == "Title" || scene.name == "Main")
        {
            SoundManager.instance.PlayBGMSound(SoundManager.instance.mainBGM);
        }
        else if (scene.name == "MatchMaking")
        {
            SoundManager.instance.PlayBGMSound(SoundManager.instance.matchBGM);
            NetworkManager.instance.CreateAndJoinRoom();
        }
        else if (scene.name == "DeckBuild")
        {
            SoundManager.instance.PlayBGMSound(SoundManager.instance.deckBGM);
        }
        else if (scene.name == "MainGame")
        {
            SoundManager.instance.PlayBGMSound(SoundManager.instance.gameBGM);
        }

        if (scene.name == "DeckBuild")
        {
            BuildManager.instance.LoadAll();
        }
    }

    private IEnumerator OnProgress(UnityAction action)
    {
        SoundManager.instance.bgmAudio.clip = null;
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / progressTime;

            textProgressData.text = $"Now Loading... {progressSlider.value * 100:F0}%";
            progressSlider.value = Mathf.Lerp(0, 1, percent);

            yield return null;
        }

        action?.Invoke();
    }
}

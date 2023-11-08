using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public TMP_Text userCount;
    public bool first;

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
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom == false)
        {
            userCount.text = PhotonNetwork.CountOfPlayers.ToString();
        }
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void CreateAndJoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(1);
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public void SetPlayerID()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for (int i = 0; i < sortedPlayers.Length; i++)
        {
            if (sortedPlayers[i].ActorNumber == actorNumber)
            {
                GameManager.Instance.playerID = i.ToString();
                break;
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(GameManager.Instance == null)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            if(GameManager.Instance.isGameEnd == false)
            {
                GameManager.Instance.GameSet();
            }
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
        userCount = FindObjectOfType<TMP_Text>();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions opt = new();
        opt.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, opt, null);
    }

    public void StartGame(bool first)
    {
        this.first = first;
        Invoke("GameLoad", 5);
    }

    public void GameLoad()
    {
        SceneManager.LoadScene(2);
    }
}

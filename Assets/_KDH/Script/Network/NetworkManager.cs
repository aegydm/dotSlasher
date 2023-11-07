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
        if(PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Update()
    {
        userCount.text = PhotonNetwork.CountOfPlayers.ToString();
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void CreateAndJoinRoom()
    {
        SceneManager.LoadScene(1);
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for(int i = 0; i < sortedPlayers.Length; i++)
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
        PhotonNetwork.LeaveRoom();        
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom(); SceneManager.LoadScene(0);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions opt = new();
        opt.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(null, opt, null);
    }
}
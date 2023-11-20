using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public TMP_Text userCount;
    public bool first;
    [SerializeField] TMP_Dropdown dropdown;
    public string deckName = "1";

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
            userCount.gameObject.SetActive(true);
            userCount.text = PhotonNetwork.CountOfPlayers.ToString();
        }
        else
        {
            userCount.gameObject.SetActive(false);
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
                deckName = "1";
            //else if (dropdown.value == 1)
            {
            //    deckName = "F4_Demo";
            }
            //else
            {
            //    deckName = "1";
            }
            //Debug.Log(deckName);
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
                GameManager.instance.playerID = i.ToString();
                break;
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(GameManager.instance == null)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            if(GameManager.instance.isGameEnd == false)
            {
                GameManager.instance.Win();
            }
        }
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
        userCount = FindObjectOfType<TMP_Text>();
        dropdown = FindObjectOfType<TMP_Dropdown>();
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
        SceneLoadManager.LoadScene("MainGame");
    }
}

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
        PhotonNetwork.ConnectUsingSettings();
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
        PhotonNetwork.JoinRandomOrCreateRoom();
        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //GameManager.Instance.photonView = PhotonNetwork.Instantiate("player",Vector3.zero,Quaternion.identity).GetComponent<PhotonView>();
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Player[] sortedPlayers = PhotonNetwork.PlayerList;

        for(int i = 0; i < sortedPlayers.Length; i++)
        {
            if (sortedPlayers[i].ActorNumber == actorNumber)
            {
                GameManager.Instance.playerID = i.ToString();
                Debug.LogError(GameManager.Instance.playerID);
                break;
            }
        }
    }
}

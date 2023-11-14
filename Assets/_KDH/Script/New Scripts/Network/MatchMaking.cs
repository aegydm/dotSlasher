using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchMaking : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text m_Text;
    [SerializeField] GameObject rockScissorPaper;
    public PhotonView photon;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckRoomIsFull();
    }

    public override void OnJoinedRoom()
    {
        CheckRoomIsFull();
    }

    public void CheckRoomIsFull()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            m_Text.text = "MatchMaking Successfully";
            Invoke("StartRSP", 5f);
        }
    }

    public void StartRSP()
    {
        rockScissorPaper.SetActive(true);
    }

    [PunRPC]
    public void SetEnemyHand(Hand hand)
    {
        RSP.instance.enemyHand = hand;
    }

    [PunRPC]
    public void SetFirst(bool startFirst)
    {
        if (startFirst)
        {
            RSP.instance.startFirst = true;
            RSP.instance.chooseText.text = "Start First";
            NetworkManager.instance.StartGame(RSP.instance.startFirst);
        }
        else
        {
            RSP.instance.startFirst = false;
            RSP.instance.chooseText.text = "Start Last";
            NetworkManager.instance.StartGame(RSP.instance.startFirst);
        }
    }
}

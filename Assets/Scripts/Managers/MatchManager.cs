using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MatchManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button matchButton;

    private readonly string gameVersion = "1";

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        matchButton.interactable = false;
    }

    public override void OnConnectedToMaster()
    {
        matchButton.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        matchButton.interactable = false;
    }

    public void Match()
    {
        matchButton.interactable = false;
        PhotonNetwork.AutomaticallySyncScene = true;        // 씬 자동 동기화

        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {

        Debug.Log("방 입장완료. 상대방 대기중...");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("MultiDungeon");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

/*
                    MatchManager : 던전 빠른매칭 시스템
            
            - NPC와 상호작용하여 매칭시작
            - 빈방이 없을경우 : 방 생성후 다른플레이어 참가 대기
            - 빈방이 있을경우 : 방 참가후 함께 씬 이동
 */

public class MatchManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button matchButton;
    [SerializeField] private MatchingTextEffect effect;

    private readonly string gameVersion = "1";

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "TempPlayer";

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
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.NickName = "Host Player";
        }
        else
        {
            PhotonNetwork.NickName = "Player";
        }
        Debug.Log("방 입장완료. 상대방 대기중...");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
        {
            effect.StopAnim();
            PhotonNetwork.LoadLevel("MultiDungeon");
        }
    }

    public override void OnLeftRoom()
    {
        GameManager.Instance.isMultiPlaying = false;
    }
}

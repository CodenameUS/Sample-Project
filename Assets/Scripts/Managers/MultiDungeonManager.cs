using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MultiDungeonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public Transform[] spawnPositions;

    private static MultiDungeonManager instance;

    public static MultiDungeonManager Instance
    {
        get
        {
            if (Instance == null) instance = FindObjectOfType<MultiDungeonManager>();

            return instance;
        }
    }

    private void Start()
    {
        SpawnPlayer();

        if(PhotonNetwork.IsMasterClient)
        {
            
        }
    }

    // 플레이어 오브젝트 생성
    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;                       // 플레이어 넘버
        var spawnPosition = spawnPositions[localPlayerIndex];           // 플레이어 위치 설정

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);

        // 플레이어 관련 초기화
        GameManager.Instance.FindPlayerObject();
        GameManager.Instance.FindCameraObject();
        DataManager.Instance.LoadPlayerData();
    }
}

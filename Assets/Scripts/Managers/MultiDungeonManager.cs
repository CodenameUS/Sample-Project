using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MultiDungeonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] playerSpawnPositions;              // 플레이어 스폰위치
    [SerializeField] private Transform[] monsterSpawnPositions;             // 몬스터 스폰위치
    [SerializeField] private Transform bossSpawnPosition;                   // 보스모늣터 스폰위치

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
        SpawnMonster();
    }


    // 플레이어 오브젝트 생성
    private void SpawnPlayer()
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;                    // 플레이어 넘버
        var spawnPosition = playerSpawnPositions[localPlayerIndex];                          // 플레이어 위치 설정

        PhotonNetwork.Instantiate("Player/MultiPlayer", spawnPosition.position, spawnPosition.rotation);

        // 플레이어 관련 초기화
        GameManager.Instance.FindPlayerObject();
        GameManager.Instance.FindCameraObject();
        DataManager.Instance.LoadPlayerData();
    }

    // 몬스터 생성
    private void SpawnMonster()
    {
        // 마스터 클라이언트만 
        if (!PhotonNetwork.IsMasterClient) return;

        for(int i = 0;i<monsterSpawnPositions.Length;i++)
        {
            PhotonNetwork.Instantiate("Monsters/TurtleShell_Multi",
                monsterSpawnPositions[i].position, monsterSpawnPositions[i].rotation);
        }
    }

}

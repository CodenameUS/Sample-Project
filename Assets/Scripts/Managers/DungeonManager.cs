using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                * 던전 진행도
                * 몬스터 소환
 */
public class DungeonManager : Singleton<DungeonManager>
{
    [SerializeField] private GameObject startingPoint;              // 플레이어 시작 위치
    [SerializeField] private GameObject[] monsters;                 

    private void Start()
    {
        // 플레이어 위치 설정
        GameManager.Instance.player.transform.position = startingPoint.transform.position;
        GameManager.Instance.player.transform.rotation = Quaternion.identity;

        Spawn();
    }

    // 몬스터 스폰
    private void Spawn()
    {
        for(int i = 0;i<monsters.Length;i++)
        {
            monsters[i].SetActive(true);
        }
    }
}

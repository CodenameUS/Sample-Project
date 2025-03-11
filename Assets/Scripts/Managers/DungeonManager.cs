using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                * ���� ���൵
                * ���� ��ȯ
 */
public class DungeonManager : Singleton<DungeonManager>
{
    [SerializeField] private GameObject startingPoint;              // �÷��̾� ���� ��ġ
    [SerializeField] private GameObject[] monsters;                 

    private void Start()
    {
        // �÷��̾� ��ġ ����
        GameManager.Instance.player.transform.position = startingPoint.transform.position;
        GameManager.Instance.player.transform.rotation = Quaternion.identity;

        Spawn();
    }

    // ���� ����
    private void Spawn()
    {
        for(int i = 0;i<monsters.Length;i++)
        {
            monsters[i].SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                WeaponItemData ����ȭ ���� Ŭ����
 */

[System.Serializable]
public class WeaponItemDTO
{
    public int id;                      // ������ id
    public string itemName;             // ������ �̸�
    public string itemToolTip;          // ������ ����
    public int damage;                  // ���� ������
    public float rate;                  // ���ݼӵ�
}

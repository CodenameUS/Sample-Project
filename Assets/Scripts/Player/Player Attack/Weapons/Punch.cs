using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Punch : ����(�ָ�) Ŭ����
 */
public class Punch : Weapon
{
    private void Awake()
    {
        // ���� Ÿ�� ����
        type = WeaponType.None;
    }
    private void Start()
    {
        // �⺻ ���� ���� ����
        attackStrategy = new PunchAttack();
    }
}

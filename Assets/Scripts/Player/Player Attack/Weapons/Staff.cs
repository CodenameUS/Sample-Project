using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Staff : ����(������) Ŭ����
 */
public class Staff : Weapon
{
    private void Awake()
    {
        // ���� Ÿ�� ����
        type = WeaponType.Staff;
    }
    private void Start()
    {
        // �⺻ ���� ���� ����
        attackStrategy = new NormalAttack();
    }
}

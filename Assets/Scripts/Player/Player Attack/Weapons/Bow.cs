using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Bow : ����(Ȱ) Ŭ����
 */
public class Bow : Weapon
{
    private void Awake()
    {
        // ���� Ÿ�� ����
        type = WeaponType.Bow;
    }
    private void Start()
    {
        // �⺻ ���� ���� ����
        attackStrategy = new NormalAttack();
    }
}

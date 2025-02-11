using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Sword : ����(��) Ŭ����
 */
public class Sword : Weapon
{
    private void Awake()
    {
        // ���� Ÿ�� ����
        type = WeaponType.Sword;
    }
    private void Start()
    {
        // �⺻ ���� ���� ����
        attackStrategy = new NormalAttack();        
    }

    public void ChargeAttack()
    {
        Debug.Log("�⸦ ��� ����");
    }
}

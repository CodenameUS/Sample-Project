using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Sword : ����(��) Ŭ����

            - Collider�� ����ؼ� �������� ����
            - SetHitBox() : Collider On/Off - �ִϸ��̼� �̺�Ʈ�� ���
 */
public class Sword : Weapon
{
    private BoxCollider hitBox;                 // ���� ����

    private void Awake()
    {
        // ���� Ÿ�� ����
        type = WeaponType.Sword;

        hitBox = GetComponent<BoxCollider>();
    }

    // ���� ������ ��
    public override void Attack()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            Debug.Log("���͸� ���߾���");
            Monster monster = other.GetComponent<Monster>();
            monster.GetDamaged(DataManager.Instance.GetPlayerData().Damage);
        }
    }

    public override void SetHitBox(bool isEnabled)
    {
        hitBox.enabled = isEnabled;
    }
}

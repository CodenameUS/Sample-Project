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
    private TrailRenderer effect;               // ���� ����Ʈ

    private void Awake()
    {
        // ���� Ÿ�� ����
        type = WeaponType.Sword;

        hitBox = GetComponent<BoxCollider>();
        effect = GetComponentInChildren<TrailRenderer>();
    }

    public override void Attack()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            monster.GetDamaged(DataManager.Instance.GetPlayerData().Damage);
        }
        else if(other.CompareTag("BossMonster"))
        {
            BossMonster boss = other.GetComponent<BossMonster>();
            boss.GetDamaged(DataManager.Instance.GetPlayerData().Damage);
        }
    }

    public override void SetHitBox(bool isEnabled)
    {
        hitBox.enabled = isEnabled;
    }

    public override void SetEffect(bool isEnabled)
    {
        effect.enabled = isEnabled;
    }
}

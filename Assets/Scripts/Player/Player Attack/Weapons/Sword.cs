using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Sword : ����(��) Ŭ����

            - BoxCollider�� ����ؼ� �������� ����
            
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
        Debug.Log("���� ���ݼ���");
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            Attack();
        }
    }
    public override void SetHitBox(bool isEnabled)
    {
        hitBox.enabled = isEnabled;
    }
}

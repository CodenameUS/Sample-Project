using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
                    Punch : ����(�ָ�) Ŭ����

            - RayCast�� ����ؼ� �������� ����
 */
public class Punch : Weapon
{

    private void Awake()
    {
        // ���� Ÿ�� ����
        type = WeaponType.None;
    }

    public override void Attack()
    {
       
    }

    private  void OnTriggerEnter(Collider other)
    {
        
    }

    public override void SetHitBox(bool isEnabled)
    {
        
    }
}

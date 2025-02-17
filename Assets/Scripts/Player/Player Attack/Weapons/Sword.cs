using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Sword : 무기(검) 클래스

            - BoxCollider를 사용해서 공격판정 구현
            
 */
public class Sword : Weapon
{
    private BoxCollider hitBox;                 // 공격 판정

    private void Awake()
    {
        // 무기 타입 설정
        type = WeaponType.Sword;

        hitBox = GetComponent<BoxCollider>();
    }

    // 공격 데미지 등
    public override void Attack()
    {
        Debug.Log("몬스터 공격성공");
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

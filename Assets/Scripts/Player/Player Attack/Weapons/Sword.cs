using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Sword : 무기(검) 클래스

            - Collider를 사용해서 공격판정 구현
            - SetHitBox() : Collider On/Off - 애니메이션 이벤트에 사용
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
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            Debug.Log("몬스터를 맞추었음");
            Monster monster = other.GetComponent<Monster>();
            monster.GetDamaged(DataManager.Instance.GetPlayerData().Damage);
        }
    }

    public override void SetHitBox(bool isEnabled)
    {
        hitBox.enabled = isEnabled;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
                    Punch : 무기(주먹) 클래스

            - RayCast를 사용해서 공격판정 구현
 */
public class Punch : Weapon
{

    private void Awake()
    {
        // 무기 타입 설정
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

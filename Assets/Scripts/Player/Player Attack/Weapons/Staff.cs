using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Staff : 무기(스태프) 클래스
 */
public class Staff : Weapon
{
    private void Awake()
    {
        // 무기 타입 설정
        type = WeaponType.Staff;
    }
    private void Start()
    {
        // 기본 공격 전략 설정
        attackStrategy = new NormalAttack();
    }
}

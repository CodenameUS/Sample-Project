using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Punch : 무기(주먹) 클래스
 */
public class Punch : Weapon
{
    private void Awake()
    {
        // 무기 타입 설정
        type = WeaponType.None;
    }
    private void Start()
    {
        // 기본 공격 전략 설정
        attackStrategy = new PunchAttack();
    }
}

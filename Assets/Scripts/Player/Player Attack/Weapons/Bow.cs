using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Bow : 무기(활) 클래스
 */
public class Bow : Weapon
{
    private void Awake()
    {
        // 무기 타입 설정
        type = WeaponType.Bow;
    }
    private void Start()
    {
        // 기본 공격 전략 설정
        attackStrategy = new NormalAttack();
    }
}

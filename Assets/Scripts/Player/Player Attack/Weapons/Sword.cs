using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Sword : 무기(검) 클래스
 */
public class Sword : Weapon
{
    private void Awake()
    {
        // 무기 타입 설정
        type = WeaponType.Sword;
    }
    private void Start()
    {
        // 기본 공격 전략 설정
        attackStrategy = new NormalAttack();        
    }

    public void ChargeAttack()
    {
        Debug.Log("기를 모아 공격");
    }
}

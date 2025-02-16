using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                NormalAttack : Concrete Attack Strategy - 기본 공격
            
            - 공격 판정의 구현
 */
public class NormalAttack : IAttackStrategy
{
    // 공격
    public void Attack()
    {
        Debug.Log("기본 공격 수행!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : IAttackStrategy
{
    // 공격
    public void Attack()
    {
        Debug.Log("주먹 공격 수행");
    }
}

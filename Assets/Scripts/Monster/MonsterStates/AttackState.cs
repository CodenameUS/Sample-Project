using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                Monster State - Attack (공격상태)

        - 공격 애니메이션 설정
 */

public class AttackState<T> : BaseState<T> where T : Monster
{
    public AttackState(T monster) : base(monster) { }

    public override void OnStateEnter()
    {

    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}

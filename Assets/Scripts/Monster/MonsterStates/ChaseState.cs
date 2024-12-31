using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                Monster State - Chase (추격상태)

        - 추격 애니메이션 설정
 */
public class ChaseState<T> : BaseState<T> where T : Monster
{
    public ChaseState(T monster) : base(monster) { }

    public override void OnStateEnter()
    {
        if (monster.Anim != null) monster.Anim.SetBool("Walk", true);

    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}

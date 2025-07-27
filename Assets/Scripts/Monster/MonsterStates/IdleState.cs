using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                            << IdleState >>

        - Idle 상태 : 초기상태. Idle 애니메이션 수행
 */
public class IdleState<T> : BaseState<T> where T : Monster
{
    public IdleState(T monster) : base(monster) { }

    public override void OnStateEnter()
    {
        monster.Anim.SetBool("Walk", false);
    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}

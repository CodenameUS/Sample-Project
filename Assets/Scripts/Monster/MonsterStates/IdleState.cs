using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                Monster State - Idle (기본상태)
        
           - 기본상태 애니메이션 설정
 */
public class IdleState<T> : BaseState<T> where T : Monster
{
    public IdleState(T monster) : base(monster) { }

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

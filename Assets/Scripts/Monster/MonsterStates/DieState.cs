using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                Monster State - Die (죽음상태)

        - 죽음 애니메이션 설정
 */

public class DieState<T> : BaseState<T> where T : Monster
{
    public DieState(T monster) : base(monster) { }

    public override void OnStateEnter()
    {
        
    }

    public override void OnStateExit()
    {
        
    }

    public override void OnStateUpdate()
    {
        
    }
}

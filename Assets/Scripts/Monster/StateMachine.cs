using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
            StateMachine은 Monster의 AI 상태를 관리
 */

public class StateMachine 
{
    private BaseState curState;

    public StateMachine(BaseState init)
    {
        // 현재상태 초기화
        curState = init;
    }

    // 상태 변경
    public void ChangeState(BaseState nextState)
    {
        // 다른상태로만 변경
        if (nextState == curState)
            return;

        if (curState != null)
            curState.OnStateExit();

        // 현재상태를 nextState로 설정
        curState = nextState;
        curState.OnStateEnter();
    }

    public void UpdateState()
    {
        if (curState != null)
            curState.OnStateUpdate();
    }
}

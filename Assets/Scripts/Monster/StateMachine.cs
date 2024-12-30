using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
            StateMachine�� Monster�� AI ���¸� ����
 */

public class StateMachine 
{
    private BaseState curState;

    public StateMachine(BaseState init)
    {
        // ������� �ʱ�ȭ
        curState = init;
    }

    // ���� ����
    public void ChangeState(BaseState nextState)
    {
        // �ٸ����·θ� ����
        if (nextState == curState)
            return;

        if (curState != null)
            curState.OnStateExit();

        // ������¸� nextState�� ����
        curState = nextState;
        curState.OnStateEnter();
    }

    public void UpdateState()
    {
        if (curState != null)
            curState.OnStateUpdate();
    }
}

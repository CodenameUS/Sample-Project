using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : Monster
{
    // TurtleShell이 가지는 상태
    private enum States
    {
        Idle,
        Chase,
        Attack
    }

    private States curState;                    // 현재 상태
    private StateMachine stateMachine;

    private void Awake()
    {
        InitValues();
    }

    private void InitValues()
    {
        // TurtleShell Stat 초기화
        maxHp = 100;
        maxMp = 100;
        curHp = maxHp;
        curMp = maxMp;
        speed = 5f;

        // 초기상태는 Idle
        curState = States.Idle;
        // StateMachine 객체 생성(Idle상태)
        stateMachine = new StateMachine(new IdleState(this));
    }

    private void Update()
    {
        switch(curState)
        {
            case States.Idle:
                // Idle 상태 구현
                break;
            case States.Chase:
                // Chase 상태 구현
                break;
            case States.Attack:
                //Attack 상태 구현
                break;
        }
    }
    
    // 상태 변경
    private void ChangeState(States nextState)
    {
        curState = nextState;
        switch(curState)
        {
            case States.Idle:
                stateMachine.ChangeState(new IdleState(this));
                break;
            case States.Chase:
                stateMachine.ChangeState(new ChaseState(this));
                break;
            case States.Attack:
                stateMachine.ChangeState(new AttackState(this));
                break;
        }
    }
    // 상태 구현에 필요한 함수들..
}

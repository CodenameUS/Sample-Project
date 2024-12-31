using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Monster - TurtleShell
        - 가지는 상태 : Idle, Chase, Attack, Die
 */
public class TurtleShell : Monster
{
    // TurtleShell이 가지는 상태
    private enum States
    {
        Idle,
        Chase,
        Attack,
        Die
    }

    private States curState;                             // 현재 상태
    private StateMachine<TurtleShell> stateMachine;


    private void Awake()
    {
        // 부모(Monster)의 초기화
        base.Awake();
        InitValues();
    }

    private void InitValues()
    {
        // TurtleShell 능력치 초기화
        maxHp = 100;
        maxMp = 100;
        curHp = maxHp;
        curMp = maxMp;
        speed = 5f;

        // 초기상태는 Idle
        curState = States.Idle;
        // StateMachine 객체 생성(Idle상태)
        stateMachine = new StateMachine<TurtleShell>(new IdleState<TurtleShell>(this));
    }

    private void Update()
    {
        
    }
    
    // 상태 변경
    private void ChangeState(States nextState)
    {
        curState = nextState;
        switch(curState)
        {
            case States.Idle:
                stateMachine.ChangeState(new IdleState<TurtleShell>(this));
                break;
            case States.Chase:
                stateMachine.ChangeState(new ChaseState<TurtleShell>(this));
                break;
            case States.Attack:
                stateMachine.ChangeState(new AttackState<TurtleShell>(this));
                break;
            case States.Die:
                stateMachine.ChangeState(new DieState<TurtleShell>(this));
                break;
        }
    }
    // 상태 구현에 필요한 함수들..
}

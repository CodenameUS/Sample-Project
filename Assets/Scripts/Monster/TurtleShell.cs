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
        curHp = maxHp;
        speed = 1f;
        maxDistance = 5f;
        idleThreshold = 0.3f;

        // 초기상태는 Idle
        curState = States.Idle;
        // StateMachine 객체 생성(Idle상태)
        stateMachine = new StateMachine<TurtleShell>(new IdleState<TurtleShell>(this));

        Nav.speed = speed;
    }

    private void Update()
    {
        stateMachine.curState.OnStateUpdate();

        if (isReset)
        {
            ChangeState(States.Idle);
            isReset = false;
        }
    }

    // Scan Range에 플레이어가 들어왔을경우
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        ChangeState(States.Chase);
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

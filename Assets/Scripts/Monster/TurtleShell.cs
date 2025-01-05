using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    Monster - TurtleShell
        - ������ ���� : Idle, Chase, Attack, Die
 */
public class TurtleShell : Monster
{
    // TurtleShell�� ������ ����
    private enum States
    {
        Idle,
        Chase,
        Attack,
        Die
    }

    private States curState;                             // ���� ����
    private StateMachine<TurtleShell> stateMachine;

    private void Awake()
    {
        // �θ�(Monster)�� �ʱ�ȭ
        base.Awake();
        InitValues();
    }

    private void InitValues()
    {
        // TurtleShell �ɷ�ġ �ʱ�ȭ
        maxHp = 100;
        curHp = maxHp;
        speed = 1f;
        maxDistance = 5f;
        idleThreshold = 0.3f;

        // �ʱ���´� Idle
        curState = States.Idle;
        // StateMachine ��ü ����(Idle����)
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

    // Scan Range�� �÷��̾ ���������
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        ChangeState(States.Chase);
    }

    // ���� ����
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
    // ���� ������ �ʿ��� �Լ���..
}

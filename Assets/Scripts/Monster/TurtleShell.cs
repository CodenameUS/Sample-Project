using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : Monster
{
    // TurtleShell�� ������ ����
    private enum States
    {
        Idle,
        Chase,
        Attack
    }

    private States curState;                    // ���� ����
    private StateMachine stateMachine;

    private void Awake()
    {
        InitValues();
    }

    private void InitValues()
    {
        // TurtleShell Stat �ʱ�ȭ
        maxHp = 100;
        maxMp = 100;
        curHp = maxHp;
        curMp = maxMp;
        speed = 5f;

        // �ʱ���´� Idle
        curState = States.Idle;
        // StateMachine ��ü ����(Idle����)
        stateMachine = new StateMachine(new IdleState(this));
    }

    private void Update()
    {
        switch(curState)
        {
            case States.Idle:
                // Idle ���� ����
                break;
            case States.Chase:
                // Chase ���� ����
                break;
            case States.Attack:
                //Attack ���� ����
                break;
        }
    }
    
    // ���� ����
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
    // ���� ������ �ʿ��� �Լ���..
}

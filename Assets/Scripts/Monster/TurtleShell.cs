using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                             Monster - TurtleShell

        - ������ ���� : Idle, Chase, Attack, Die

        - TurtleShell�� �ɷ�ġ ���� �� ���� �ʱ�ȭ

        - ���ǿ����� ������ȯ
            - ScanRange �ȿ� �÷��̾ �����°�� -> ChaseState ����
            - �÷��̾ �Ѵٰ� ������ ��� -> IdleState ����
            - ���ݹ����� �÷��̾ �ִ°�� -> AttackState ����
            - ü���� 0���� -> DieState ����
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
        attackDelay = 2f;

        // �ʱ���´� Idle
        curState = States.Idle;
        // StateMachine ��ü ����(Idle����)
        stateMachine = new StateMachine<TurtleShell>(new IdleState<TurtleShell>(this));

        Nav.speed = speed;
    }

    private void Update()
    {
        stateMachine.curState.OnStateUpdate();

        DecideState();
    }

    // Scan Range�� �÷��̾ ��������� Chase ���� ����
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

    // ���ǿ����� ������ȯ ����
    private void DecideState()
    {
        // �÷��̾� <-> ���� �Ÿ�
        float distanceToPlayer = Vector3.Distance(transform.position, Target.transform.position);
        
        // Chase���¿��� �������� ���ͿϷ� -> Idle���� ����
        if (isReset)
        {
            ChangeState(States.Idle);
            isReset = false;
        }

        // ���ݹ����� ���� Attack ���� ����
        if (distanceToPlayer <= Nav.stoppingDistance && !isDead)
        {
            ChangeState(States.Attack);
        }

        // ���ݻ��¿��� �÷��̾ �־����� Chase ���� ����
        else if (curState == States.Attack && distanceToPlayer > Nav.stoppingDistance && !isDead)
        {
            ChangeState(States.Chase);
        }

        // ü���� 0���Ϸ� �������� ����
        if (curHp <= 0 && !isDead)
        {
            isDead = true;
            ChangeState(States.Die);
        }
    }

   
}

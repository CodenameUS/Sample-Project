using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                             Monster - TurtleShell

        - 가지는 상태 : Idle, Chase, Attack, Die

        - TurtleShell의 능력치 설정 및 상태 초기화

        - 조건에따른 상태전환
            - ScanRange 안에 플레이어가 들어오는경우 -> ChaseState 돌입
            - 플레이어를 쫓다가 복귀한 경우 -> IdleState 돌입
            - 공격범위에 플레이어가 있는경우 -> AttackState 돌입
            - 체력이 0이하 -> DieState 돌입
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
        attackDelay = 2f;

        // 초기상태는 Idle
        curState = States.Idle;
        // StateMachine 객체 생성(Idle상태)
        stateMachine = new StateMachine<TurtleShell>(new IdleState<TurtleShell>(this));

        Nav.speed = speed;
    }

    private void Update()
    {
        stateMachine.curState.OnStateUpdate();

        DecideState();
    }

    // Scan Range에 플레이어가 들어왔을경우 Chase 상태 돌입
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

    // 조건에따른 상태전환 결정
    private void DecideState()
    {
        // 플레이어 <-> 몬스터 거리
        float distanceToPlayer = Vector3.Distance(transform.position, Target.transform.position);
        
        // Chase상태에서 원점으로 복귀완료 -> Idle상태 톨입
        if (isReset)
        {
            ChangeState(States.Idle);
            isReset = false;
        }

        // 공격범위에 들어서면 Attack 상태 돌입
        if (distanceToPlayer <= Nav.stoppingDistance && !isDead)
        {
            ChangeState(States.Attack);
        }

        // 공격상태에서 플레이어가 멀어지면 Chase 상태 돌입
        else if (curState == States.Attack && distanceToPlayer > Nav.stoppingDistance && !isDead)
        {
            ChangeState(States.Chase);
        }

        // 체력이 0이하로 떨어지면 죽음
        if (curHp <= 0 && !isDead)
        {
            isDead = true;
            ChangeState(States.Die);
        }
    }

   
}

using UnityEngine;
using Photon.Pun;

/*
                            << TurtleShell >>

        - 몬스터 "TurtleShell" 스탯초기화, 상태전환 조건처리, 공격 로직처리
            - FSM 패턴으로 상태 전환 처리

        - 조건에따른 상태전환
            1. Range 안에 플레이어가 들어오는경우 : To Chase
            2. 플레이어를 쫓다가 멀어져 원래자리로 복귀한 경우 : To Idle
            3. 플레이어가 공격범위에 있는 경우 : To Attack
            4. 체력이 0이하로 감소 : To Die
*/

public class TurtleShell : Monster
{
    // 가질수있는 상태
    public enum States
    {
        Idle,
        Chase,
        Attack,
        Die
    }

    // 현재 상태
    public States curState;                             
    // 상태머신 인스턴스
    private StateMachine<TurtleShell> stateMachine;
    
    protected override void Awake()
    {
        // 부모(Monster)의 초기화
        base.Awake();
        Init();
    }

    private void Init()
    {
        // 스탯 설정
        maxHp = 50;
        curHp = maxHp;
        speed = 1f;
        maxDistance = 5f;
        idleThreshold = 0.1f;
        attackDelay = 2f;
        damage = 5f;
        attackRange = 1.3f;
        Nav.speed = speed;

        // StateMachine 인스턴스 생성(최초 Idle상태)
        curState = States.Idle;
        stateMachine = new StateMachine<TurtleShell>(new IdleState<TurtleShell>(this));


    }

    private void Update()
    {
        if (GameManager.Instance.isMultiPlaying && !PhotonNetwork.IsMasterClient) return;

        stateMachine.curState.OnStateUpdate();

        // 2초에 한번씩 상태결정
        InvokeRepeating(nameof(DecideState), 0f, 2f);
    }

    // 상태 변경
    public void ChangeState(States nextState)
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
        FindClosestPlayer();

        // 플레이어 <-> 몬스터 거리
        float distanceToPlayer = Vector3.Distance(transform.position, Target.transform.position);

        // Chase상태에서 원점으로 복귀완료 -> Idle상태 톨입
        if (curState == States.Chase && isReset && !isDead)
        {
            ChangeState(States.Idle);
        }

        // 공격범위에 들어서면 Attack 상태 돌입
        if (distanceToPlayer <= attackRange && !isDead)
        {
            ChangeState(States.Attack);
        }
        // 공격상태에서 플레이어가 멀어지면 Chase 상태 돌입
        else if (curState == States.Attack && distanceToPlayer > attackRange && !isDead)
        {
            ChangeState(States.Chase);
        }

        // 체력이 0이하로 떨어지면 죽음
        if (curHp <= 0 && !isDead)
        {
            isDead = true;
            AudioManager.Instance.PlaySFX("TurtleShell_Die");
            ChangeState(States.Die);
        }
    }

    // 공격판정
    public override void Attack()
    {
        if (GameManager.Instance.isMultiPlaying && !PhotonNetwork.IsMasterClient)
            return;

        // 공격방향 : 몬스터 기준 앞
        Vector3 origin = transform.position + new Vector3(0, 0.5f, 0);
        Vector3 direction = transform.forward;

        // 히트데미지 처리
        if (Physics.SphereCast(origin, 0.5f, direction, out RaycastHit hit, 1f, LayerMask.GetMask("Player")))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (GameManager.Instance.isMultiPlaying)
                {
                    PhotonView targetView = hit.collider.GetComponent<PhotonView>();
                    targetView.RPC("GetDamaged", targetView.Owner, damage);
                    
                }
                else
                {
                    PlayerData playerData = DataManager.Instance.GetPlayerData();
                    playerData.GetDamaged(damage);
                }
            }
        }
    }
}

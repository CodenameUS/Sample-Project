using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
            Monster 클래스는 몬스터의 공통데이터를 관리

 */
public class Monster : MonoBehaviour
{
    #region **Monster Stats**
    [Header("#Monster Stats")]
    public int maxHp;
    public int curHp;
    public float speed;
    public float maxDistance;                       // 플레이어와의 거리(복귀하기위한 최대거리)
    public float idleThreshold;                     // 복귀후 처음 위치와의 차이
    public float attackDelay;                       // 공격속도
    #endregion

    private Vector3 startPosition;                  // 몬스터의 첫 위치

    public bool isReset;                            // 원점으로 복귀했는지 여부
    public bool isAttackReady;                      // 공격 가능 여부

    protected PlayerController targetPlayer;        // 타깃 플레이어

    private BoxCollider scanRange;                  // 플레이어 탐지범위
    private BoxCollider hitBoxCol;                  // 몬스터 히트박스
    private Animator anim;                          // 몬스터 애니메이터
    private NavMeshAgent nav;                       // 몬스터 네비게이션

    #region ** Properties **

    public Animator Anim => anim;
    public NavMeshAgent Nav => nav;
    public PlayerController Target => targetPlayer;
    public Vector3 StartPosition => startPosition;
    #endregion
    
    protected void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        targetPlayer = GameManager.Instance.player;
        startPosition = transform.position;

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hitBoxCol = GetComponent<BoxCollider>();
        scanRange = GetComponent<BoxCollider>();
    }

    // 공격 가능상태로 전환
    public void ReadyToAttack()
    {
        isAttackReady = true;
    }
}

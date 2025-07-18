using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

/*
                    Monster - 몬스터의 공통 데이터를 관리

        - Monster Status & Flags

        - 컴포넌트 초기화

        - 공통 함수
            - ReadyToAttack : 공격가능여부를 나타내는 isAttackReady 플래그를 True로
            - DeactiveGameObject : 몬스터 오브젝트를 비활성화
            - GetDamaged : 받은 데미지만큼 Hp 감소
 */

public class Monster : MonoBehaviourPun
{
    [SerializeField] private Transform damageTextPos; // 데미지 텍스트 표시 위치

    #region ** Monster Status **
    [Header("#Monster Stats")]
    public float maxHp;
    public float curHp;
    public float speed;
    public float maxDistance;                       // 플레이어와의 거리(복귀하기위한 최대거리)
    public float idleThreshold;                     // 복귀후 처음 위치와의 차이
    public float attackDelay;                       // 공격속도
    public float damage;                            // 공격력
    public float attackRange;                       // 공격가능한 범위
    #endregion

    #region ** Flags **
    //[HideInInspector]
    public bool isReset;                            // 원점으로 복귀했는지 여부
    [HideInInspector]
    public bool isAttackReady;                      // 공격 가능 여부
    [HideInInspector]
    public bool isDead;                             // 죽었는지 여부
    #endregion

    #region ** Private Fields **
    private Vector3 startPosition;                  // 몬스터의 첫 위치
    private PlayerController targetPlayer;          // 타깃 플레이어
    private BoxCollider hitBoxCol;                  // 몬스터 히트박스
    private Animator anim;                          // 몬스터 애니메이터
    private NavMeshAgent nav;                       // 몬스터 네비게이션
    #endregion

    #region ** Properties **
    public Animator Anim => anim;
    public NavMeshAgent Nav => nav;
    public PlayerController Target => targetPlayer;
    public Vector3 StartPosition => startPosition;
    public BoxCollider HitBox => hitBoxCol;
    #endregion
    
    protected virtual void Awake()
    {
        Init();
    }

    private void Init()
    {
        FindClosestPlayer();
        startPosition = transform.position;

        isAttackReady = true;
        isReset = true;

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hitBoxCol = GetComponent<BoxCollider>();
    }

    // 타깃플레이어할당 - 가장 가까운 플레이어 
    protected void FindClosestPlayer()
    {
        float minDistance = float.MaxValue;
        PlayerController closest = null;

        foreach(var player in FindObjectsOfType<PlayerController>())
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }

        if (closest != null)
            targetPlayer = closest;
    }

    // 공격 가능상태로 전환
    public void ReadyToAttack()
    {
        isAttackReady = true;
    }

    // 죽음
    public void DeactiveGameObject()
    {
        Destroy(this.gameObject);
    }

    // 공격받음
    [PunRPC]
    public void GetDamaged(float damage)
    {
        float minDamage = damage * 0.8f;
        float maxDamage = damage * 1.2f;
        int randomDamage = (int)Random.Range(minDamage, maxDamage);

        DamageTextManager.Instance.ShowDamage(damageTextPos, randomDamage);

        curHp -= randomDamage;
    }

    [PunRPC]
    public void RPC_TriggerAttackAnim()
    {
        TriggerAttackAnim();
    }

    public void TriggerAttackAnim()
    {
        anim.SetTrigger("Attack");
    }

    [PunRPC]
    public void RPC_TriggerDieAnim()
    {
        TriggerDieAnim();
    }

    public void TriggerDieAnim()
    {
        anim.SetTrigger("Die");
    }

    // 공격판정
    public virtual void Attack()
    {
        
    }
}

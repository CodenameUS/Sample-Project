using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

/*
                    BossMonster - 보스몬스터의 공통 데이터를 관리

        - Boss Monster 컴포넌트 정보 가져오기

        - 공통함수
            - GetDamaged : 받은 데미지만큼 Hp 감소
 */

public class BossMonster : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform damageTextPos; // 데미지 텍스트 표시 위치

    #region ** Events **
    public event System.Action OnBossDied;

    #endregion
    #region ** Monster Status **
    [Header("#Boss Monster Stats")]
    public float maxHp;
    public float curHp;
    public float speed;
    public float damage;                            // 공격력
    public float attackRange;                       // 공격가능한 범위
    #endregion

    #region ** Private Fields **
    protected PlayerController targetPlayer;             // 타깃 플레이어
    protected BoxCollider hitBoxCol;                  // 몬스터 히트박스
    protected Animator anim;                          // 몬스터 애니메이터
    protected NavMeshAgent nav;                       // 몬스터 네비게이션
    #endregion

    #region ** Flags **
    [HideInInspector]
    public bool isAttackReady;                      // 공격 가능 여부
    [HideInInspector]
    protected bool isDead;                             // 죽었는지 여부
    [HideInInspector]
    public bool isAttacking = false;                // 공격중인지 여부
    #endregion

    #region ** Properties **
    public NavMeshAgent Nav => nav;

    public PlayerController TargetPlayer
    {
        get => targetPlayer;
        set => targetPlayer = value;
    }
    #endregion

    protected virtual void Awake()
    {
        Init();
    }

    private void Init()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hitBoxCol = GetComponent<BoxCollider>();
    }

    // 타깃플레이어할당 - 가장 가까운 플레이어 
    protected void FindClosestPlayer()
    {
        float minDistance = float.MaxValue;
        PlayerController closest = null;

        foreach (var player in FindObjectsOfType<PlayerController>())
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player;
            }
        }

        if (closest != null)
        {
            if (GameManager.Instance.isMultiPlaying && PhotonNetwork.IsMasterClient)
            {
                int viewID = closest.GetComponent<PhotonView>().ViewID;
                photonView.RPC(nameof(SetTargetPlayer), RpcTarget.All, viewID);
            }
            else
            {
                targetPlayer = closest;
            }
        }
    }

    #region ** RPC Methods **
    // 공격받음
    [PunRPC]
    public void GetDamaged(float damage)
    {
        DamageTextManager.Instance.ShowDamage(damageTextPos, (int)damage);

        curHp -= damage;
    }

    // 타깃 플레이어 설정
    [PunRPC]
    public void SetTargetPlayer(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if(pv != null)
        {
            TargetPlayer = pv.GetComponent<PlayerController>();
        }
    }
    #endregion

    protected virtual void Die()
    {
        OnBossDied?.Invoke();
    }

 
}

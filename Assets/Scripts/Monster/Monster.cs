using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

/*
                            << Monster >>

        - 몬스터 초기화 및 공통데이터 관리
        
        - GetDamaged() : 데미지만큼의 체력감소 및 데미지폰트 표시

        - FindClosestPlayer() : 가장 가까운 플레이어를 타깃으로 설정
*/

public class Monster : MonoBehaviourPun
{
    [SerializeField] private Transform damageTextPos; // 데미지 텍스트 표시 위치

    #region ** Monster Status **
    [Header("#Monster Stats")]
    public float maxHp;                             // 최대체력
    public float curHp;                             // 현재체력
    public float speed;                             // 이동속도
    public float maxDistance;                       // 플레이어와의 거리(복귀하기위한 최대거리)
    public float idleThreshold;                     // 복귀후 처음 위치와의 차이
    public float attackDelay;                       // 공격속도
    public float damage;                            // 공격력
    public float attackRange;                       // 공격가능한 범위
    #endregion

    #region ** Flags **
    [HideInInspector]
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
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        hitBoxCol = GetComponent<BoxCollider>();

        FindClosestPlayer();
        startPosition = transform.position;

        isAttackReady = true;
        isReset = true;
    }

    // 타깃플레이어 - 가장 가까운 플레이어 
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

    // 공격판정
    public virtual void Attack()
    {

    }

    #region ** Public Methods**
    // 공격 가능상태로 전환
    public void ReadyToAttack()
    {
        isAttackReady = true;
    }

    // 죽음후 오브젝트 파괴
    public void DeactiveGameObject()
    {
        Destroy(this.gameObject);

        if (GameManager.Instance.isMultiPlaying)
            MultiDungeonManager.Instance.currentMonsterCount -= 1;
    }
    #endregion

    #region ** Animations **
    public void TriggerAttackAnim()
    {
        anim.SetTrigger("Attack");
    }


    public void TriggerDieAnim()
    {
        anim.SetTrigger("Die");
    }
    #endregion

    #region ** RPC Methods **
    [PunRPC]
    public void RPC_TriggerDieAnim()
    {
        TriggerDieAnim();
    }

    [PunRPC]
    public void RPC_TriggerAttackAnim()
    {
        TriggerAttackAnim();
    }

    [PunRPC]
    public void RPC_DeactiveGameObject()
    {
        Invoke(nameof(DeactiveGameObject), 3);
    }
    
    // 공격받음
    [PunRPC]
    public void GetDamaged(float damage)
    {
        DamageTextManager.Instance.ShowDamage(damageTextPos, (int)damage);

        curHp -= damage;
    }
    #endregion
}

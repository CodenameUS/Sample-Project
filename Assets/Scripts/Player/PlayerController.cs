using UnityEngine;
using Photon.Pun;

/*
                            << PlayerController >>

        - Move() : 플레이어 조작 입력 처리
        - Turn() : 플레이어 회전 입력 처리
        - Attack() : 플레이어 공격 애니메이션 처리
        - ComboAttack() : 플레이어 콤보 공격 애니메이션 처리
        - DoSkill() : 플레이어 스킬 입력 처리
        - Dead() : 플레이어 죽음 상태 및 애니메이션 처리

        * 애니메이션 이벤트 관리
 */

public class PlayerController : MonoBehaviourPun
{
    private PlayerData playerData;
    
    readonly private int hashSpeed = Animator.StringToHash("Speed");
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");
    readonly private int hashSkillTrigger = Animator.StringToHash("Skill");
    readonly private int hashDeadTrigger = Animator.StringToHash("Dead");
    readonly private int hashSkillId = Animator.StringToHash("SkillId");
    readonly private int hashComboCount = Animator.StringToHash("ComboCount");

    private float hAxis;
    private float vAxis;
    private float baseSpeed = 4f;

    private bool isAttackKeyDown;                       // 공격키입력여부(C)
    private bool isAttacking = false;                   // 공격중여부
    private bool isDead = false;                        // 생존여부
    private bool isComboAllowed = false;                // 콤보가능여부
    public bool isCutscenePlaying = false;              // 컷신플레잉 여부

    private Vector3 moveVec;
    private Rigidbody rigid;
    private Animator anim;

    public Animator Anim => anim;
    public Transform WeaponPoint { get; private set; }  // 무기 생성 위치

    public int CurComboCount
    {
        get => anim.GetInteger(hashComboCount);
        set => anim.SetInteger(hashComboCount, value);
    }

    #region ** Unity Events **
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        if (GameManager.Instance.isMultiPlaying)
            return;

        // 플레이어 위치 로딩
        Vector3 loadedPosition = new Vector3
        (
            DataManager.Instance.GetPlayerData().PosX,
            DataManager.Instance.GetPlayerData().PosY,
            DataManager.Instance.GetPlayerData().PosZ
        );

        transform.position = loadedPosition;
    }

    private void Update()
    {
        playerData = DataManager.Instance.GetPlayerData();

        if (GameManager.Instance.isMultiPlaying && !photonView.IsMine)
            return;

        GetInput();
        DoSkill();
        Move();
        Turn();
        Attack();
        ComboAttack();
        Dead();

        // 컷씬동안에는 Idle 애니메이션
        if (isCutscenePlaying)
            anim.SetFloat(hashSpeed, 0);
    }
    #endregion

    #region ** Private Methods **
    
    private void Init()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        WeaponPoint = GetComponentInChildren<WeaponPointMarker>().gameObject.transform;
        GameManager.Instance.isMultiPlaying = PhotonNetwork.InRoom;

        if ((GameManager.Instance.isMultiPlaying && photonView.IsMine) || !GameManager.Instance.isMultiPlaying)
            GameManager.Instance.player = this;
    }

    // 키입력
    private void GetInput()
    {
        if (isDead || isCutscenePlaying || GameManager.Instance.isChatting)
            return;

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        isAttackKeyDown = Input.GetButtonDown("Attack");
    }

 
    // 플레이어 이동로직
    private void Move()
    {
        if (isAttacking || isDead || isCutscenePlaying || GameManager.Instance.isChatting)
            return;

        Vector3 inputDir = new Vector3(hAxis, 0, vAxis);
        Quaternion terrainRotation = Quaternion.Euler(0, -90f, 0);

        //moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        moveVec = (terrainRotation * inputDir).normalized;

        rigid.position += moveVec * (baseSpeed + playerData.Speed) * Time.deltaTime;
        anim.SetFloat(hashSpeed, moveVec == Vector3.zero ? 0 : (baseSpeed + playerData.Speed));
    }
    
    // 플레이어 회전로직
    private void Turn()
    {
        if (isAttacking || moveVec == Vector3.zero || isDead || isCutscenePlaying || GameManager.Instance.isChatting)
            return;

        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, playerData.RotateSpeed * Time.deltaTime);
    }

    // 플레이어 공격 
    private void Attack()
    {
        if (isAttackKeyDown && !isAttacking && !isDead && !isCutscenePlaying && !GameManager.Instance.isChatting)
        {
            if (GameManager.Instance.isMultiPlaying)
            {
                photonView.RPC(nameof(RPC_TriggerAttackAnim), RpcTarget.All);
            }
            else
            {
                TriggerAttackAnim();
            }
        }
    }

    // 플레이어 콤보 공격
    private void ComboAttack()
    {
        if(isAttackKeyDown && isAttacking && isComboAllowed && !GameManager.Instance.isChatting)
        {
            if (GameManager.Instance.isMultiPlaying)
            {
                photonView.RPC(nameof(RPC_TriggerAttackAnim), RpcTarget.All);
            }
            else
            {
                TriggerAttackAnim();
            }
        }
    }

    // 스킬사용
    private void DoSkill()
    {
        if (isAttacking || isDead || isCutscenePlaying || GameManager.Instance.isChatting)
            return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            SkillManager.Instance.skillSlots[0].UseSkill();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SkillManager.Instance.skillSlots[1].UseSkill();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SkillManager.Instance.skillSlots[2].UseSkill();
        }
    }

    // 플레이어 죽음
    private void Dead()
    {
        if(playerData.CurHp <= 0 && !isDead)
        {
            isDead = true;

            if(GameManager.Instance.isMultiPlaying)
            {
                photonView.RPC(nameof(RPC_TriggerDieAnim),RpcTarget.All);
            }
            else
            {
                TriggerDieAnim();
            }
        }
    }
    #endregion

    #region ** Public Methods **
    public void TriggerAttackAnim()
    {
        anim.SetTrigger(hashAttackTrigger);
    }

    public void TriggerDieAnim()
    {
        anim.SetTrigger(hashDeadTrigger);
    }

    public void TriggerSkillAnim(int skillId)
    {
        anim.SetInteger(hashSkillId, skillId);
        anim.SetTrigger(hashSkillTrigger);
    }
    #endregion

    #region ** RPC Methods **
    [PunRPC]
    public void GetDamaged(float damage)
    {
        playerData.GetDamaged(damage);
    }

    [PunRPC]
    public void RPC_TriggerAttackAnim()
    {
        TriggerAttackAnim();
    }

    [PunRPC]
    public void RPC_TriggerDieAnim()
    {
        TriggerDieAnim();
    }

    [PunRPC]
    public void RPC_TriggerSkillAnim(int skillId)
    {
        TriggerSkillAnim(skillId);
    }
    
    #endregion

    #region ** Animation Events **
    // 공격상태 돌입
    private void SetIsAttackingTrue() => isAttacking = true;

    // 공격상태 해제
    private void SetIsAttackingFalse() => isAttacking = false;

    // 콤보 가능
    private void SetIsComboAllowedTrue() => isComboAllowed = true;

    // 콤보 불가능
    private void SetIsComboAllowedFalse() => isComboAllowed = false;

    // 공격판정(Collider) On
    private void EnableAttackHitbox() => WeaponManager.Instance.currentWeapon.Attack(true);

    // 공격판정(Collider) Off
    private void DisableAttackHitbox() => WeaponManager.Instance.currentWeapon.Attack(false);

    // 공격판정(Raycast etc..)
    private void TriggerAttack() => WeaponManager.Instance.currentWeapon.Attack();

    // 공격이펙트 On
    private void EnableEffect() => WeaponManager.Instance.currentWeapon.SetEffect(true);

    // 공격이펙트 Off
    private void DisableEffect() => WeaponManager.Instance.currentWeapon.SetEffect(false);

    // 콤보 카운트 리셋
    private void ResetComboCount() => CurComboCount = 0;

    // 무기 공격효과음 실행
    private void PlayWeaponSfx()
    {
        if (GameManager.Instance.isMultiPlaying && !photonView.IsMine) return;
        
        WeaponManager.Instance.currentWeapon.PlayerSfx();
    }

    #endregion
}

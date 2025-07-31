using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

/*
                            << Grunt >>

        - 보스몬스터 "Grunt" 스탯초기화, 이동/공격/죽음 로직처리
            - 싱글/멀티 로직 분리
        
        - 공격수행과정
            1. 플레이어가 공격 사거리에 들어왔는지 확인(CanAttackTarget())
            2. 공격실행요청(ExecuteAttack()) : 무슨공격을 할것인지(attackType) 정하고, 공격실행(Attack())
            3. 공격(CoroutineAttack)처리 : AttackType에 따른 공격 수행
                - 일반근접공격(MeleeAttack()) 및 스킬공격(Thunderbolt(), Explosion()) 처리

        - Die() : 보스체력이 0이하가 되면 죽음
            - 애니메이션, 보스 UI, 히트박스등 처리 후 OnBossDied 이벤트 알림
            - 구독자(던전매니저)의 이벤트 실행
 */


public class Grunt : BossMonster
{
    [SerializeField] private GameObject thunderboltEffect;          // 스킬(썬더볼트) 이펙트 오브젝트
    [SerializeField] private GameObject explosionEffect;            // 스킬(익스플로전) 이펙트 오브젝트
    [SerializeField] private GameObject explosionAttacker;          // 스킬(익스플로전) 공격(충돌감지) 오브젝트

    // 애니메이터 파라미터 문자열 해싱
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");
    readonly private int hashAttackType = Animator.StringToHash("AttackType");
    readonly private int hashDeadTrigger = Animator.StringToHash("Dead");
    readonly private int hashSpeed = Animator.StringToHash("Speed");

    // 카메라 노이즈(흔들림)
    private CinemachineBasicMultiChannelPerlin noise;           

    protected override void Awake()
    {
        // 부모(Monster)의 초기화
        base.Awake();
        InitData();
    }

    private void Update()
    {
        if (targetPlayer == null || isDead || targetPlayer.isCutscenePlaying)
            return;

        if (GameManager.Instance.isMultiPlaying && !photonView.IsMine) return;
        
        if(CanAttackTarget())
        {
            ExecuteAttack();
        }
        else 
        {
            Move();
        }

        if(curHp <= 0)
        {
            Die();
        }
    }

    // 보스 데이터 초기화
    private void InitData()
    {
        maxHp = 250;
        curHp = maxHp;
        speed = 1.5f;
        damage = 15f;
        attackRange = 3f;

        nav.speed = speed;

        // 카메라 노이즈(화면 흔들림)
        noise = GameManager.Instance.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // 이동
    private void Move()
    {
        if (isAttacking)
            return;

        nav.SetDestination(targetPlayer.transform.position);
        anim.SetFloat("Speed", speed);
    }

    // 플레이어가 공격 사거리에 들어왔는지 여부
    private bool CanAttackTarget()
    {
        if (targetPlayer == null || isAttacking) return false;

        // 타깃 <-> 보스 거리
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;

        return distanceToPlayer <= attackRange;
    }

    // 공격실행
    private void ExecuteAttack()
    {
        if (isAttacking) return;

        int attackType = Random.Range(0, 4);

        if (PhotonNetwork.IsMasterClient && GameManager.Instance.isMultiPlaying)
        {
            photonView.RPC(nameof(Attack), RpcTarget.All, attackType);
        }
        else
            Attack(attackType);
    }

    // 공격
    [PunRPC]
    private void Attack(int attackType)
    {
        StartCoroutine(CoroutineAttack(attackType));
    }

    // 죽음
    protected override void Die()
    {
        if (isDead) return;
        
        if(GameManager.Instance.isMultiPlaying)
        {
            base.Die();
            photonView.RPC(nameof(RPC_OnDied), RpcTarget.All);
        }
        else
        {
            base.Die();
            OnDied();
        }
        
    }

    // 죽음 
    private void OnDied()
    {
        isDead = true;

        TriggerDieAnim();
        PlaySFX("Grunt_Die");
        var bossCanvas = GetComponentInChildren<Canvas>();
        bossCanvas.gameObject.gameObject.SetActive(false);
        hitBoxCol.enabled = false;
        nav.enabled = false;
    }

    #region ** Coroutines **
    // 공격실행
    private IEnumerator CoroutineAttack(int attackType)
    {
        // 보스몬스터 정지
        nav.isStopped = true;
        isAttacking = true;
        anim.SetFloat("Speed", 0);

        // 공격사이 간격
        yield return new WaitForSeconds(1f);

        // 공격타입에 따른 공격
        switch(attackType)
        {
            case 0:
                PlaySFX("Grunt_Attack01");
                anim.SetInteger("AttackType", attackType);

                if (GameManager.Instance.isMultiPlaying)
                {
                    RPC_TriggerAttackAnim();
                }
                else
                {
                    TriggerAttackAnim();
                }
                break;
            case 1:
                PlaySFX("Grunt_Attack02");
                anim.SetInteger("AttackType", attackType);
                if (GameManager.Instance.isMultiPlaying)
                {
                    RPC_TriggerAttackAnim();
                }
                else
                {
                    TriggerAttackAnim();
                }
                break;
            case 2:
                PlaySFX("Grunt_Attack03");
                anim.SetInteger("AttackType", attackType);

                if (GameManager.Instance.isMultiPlaying)
                {
                    RPC_TriggerAttackAnim();
                }
                else
                {
                    TriggerAttackAnim();
                }
                StartCoroutine(Thunderbolt());
                break;
            case 3:
                PlaySFX("Grunt_Attack04");
                anim.SetInteger("AttackType", attackType);

                if (GameManager.Instance.isMultiPlaying)
                {
                    RPC_TriggerAttackAnim();
                }
                else
                {
                    TriggerAttackAnim();
                }
                StartCoroutine(Explosion());
                break;
        }

        yield return null;
    }

    // 스킬 공격(썬더볼트)
    private IEnumerator Thunderbolt()
    {
        if (GameManager.Instance.isMultiPlaying && !PhotonNetwork.IsMasterClient)
            yield return null;

        thunderboltEffect.TryGetComponent<GruntThunderbolt>(out GruntThunderbolt hitbox);
        if (hitbox == null)
        {
            hitbox = thunderboltEffect.AddComponent<GruntThunderbolt>();
            hitbox.damage = damage;
        }
        noise.m_AmplitudeGain = 1f;             // 카메라 흔들림 ON
        thunderboltEffect.SetActive(true);

        yield return new WaitForSeconds(3f);
        noise.m_AmplitudeGain = 0f;             // 카메라 흔들림 Off
        thunderboltEffect.SetActive(false);

        yield return new WaitForSeconds(1f);
    }
    
    // 스킬 공격(익스플로전)
    private IEnumerator Explosion()
    {
        if (GameManager.Instance.isMultiPlaying && !PhotonNetwork.IsMasterClient)
            yield return null;

        // 공격(충돌감지) 오브젝트 데미지 설정
        explosionAttacker.TryGetComponent<GruntExplosion>(out GruntExplosion hitbox);
        if(hitbox == null)
        {
            hitbox = explosionAttacker.AddComponent<GruntExplosion>();
            hitbox.damage = damage * Random.Range(0.3f, 0.7f);
        }

        // 공격 이펙트 및 충돌판정 활성화
        if(GameManager.Instance.isMultiPlaying)
        {
            photonView.RPC(nameof(RPC_ActivteExplosionEffect), RpcTarget.All, targetPlayer.transform.position);

            yield return new WaitForSeconds(1f);

            photonView.RPC(nameof(RPC_ActivteExplosionAttacker), RpcTarget.All, targetPlayer.transform.position);

            yield return new WaitForSeconds(1f);

            photonView.RPC(nameof(RPC_DeactiveExplosion), RpcTarget.All);
        }
        else
        {
            explosionEffect.transform.position = GameManager.Instance.player.transform.position;
            explosionEffect.SetActive(true);

            yield return new WaitForSeconds(1f);
            explosionAttacker.transform.position = GameManager.Instance.player.transform.position + GameManager.Instance.player.transform.up * 2f;
            explosionAttacker.SetActive(true);

            yield return new WaitForSeconds(1f);
            explosionAttacker.SetActive(false);
            explosionEffect.SetActive(false);
        }

        // 공격 후딜레이
        yield return new WaitForSeconds(1f);
    }

    // 일반근접 공격 판정
    private void MeleeAttack()
    {
        if (GameManager.Instance.isMultiPlaying && !PhotonNetwork.IsMasterClient)
            return;

        // 공격방향 : 보스몬스터 기준 앞
        Vector3 origin = transform.position + new Vector3(0, 1f, 0);
        Vector3 direction = transform.forward;

        // 히트데미지 처리
        if(Physics.SphereCast(origin, 1f, direction, out RaycastHit hit, 2f, LayerMask.GetMask("Player")))
        {
            if(hit.collider.CompareTag("Player"))
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();

                if (GameManager.Instance.isMultiPlaying)
                {
                    PhotonView targetView = hit.collider.GetComponent<PhotonView>();
                    targetView.RPC("GetDamaged", targetView.Owner, Random.Range(damage * 0.8f, damage * 1.2f));
                }
                else
                {
                    player.GetDamaged(Random.Range(damage * 0.8f, damage * 1.2f));
                }
            }
        }
    }
    #endregion

    #region ** Animations **
    private void TriggerAttackAnim()
    {
        anim.SetTrigger(hashAttackTrigger);
    }

    private void TriggerDieAnim()
    {
        anim.SetTrigger(hashDeadTrigger);
    }
    #endregion

    #region ** Animation Events **
    // 공격 끝(애니메이션 이벤트)
    private void EndAttack()
    {
        isAttacking = false;
        nav.isStopped = false;
        FindClosestPlayer();
    }

    // 효과음 출력
    private void PlaySFX(string soundId)
    {
        AudioManager.Instance.PlaySFX(soundId);
    }
    #endregion

    #region ** RPC Methods **
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
    private void RPC_ActivteExplosionEffect(Vector3 pos)
    {
        explosionEffect.transform.position = pos;
        explosionEffect.SetActive(true);
    }

    [PunRPC]
    private void RPC_ActivteExplosionAttacker(Vector3 pos)
    {
        explosionAttacker.transform.position = pos + Vector3.up * 2f;
        explosionAttacker.SetActive(true);
    }

    [PunRPC]
    private void RPC_DeactiveExplosion()
    {
        explosionEffect.SetActive(false);
        explosionAttacker.SetActive(false);
    }

    [PunRPC]
    private void RPC_OnDied()
    {
        isDead = true;
        RPC_TriggerDieAnim();
        PlaySFX("Grunt_Die");
        var bossCanvas = GetComponentInChildren<Canvas>();
        bossCanvas.gameObject.gameObject.SetActive(false);
        hitBoxCol.enabled = false;
        nav.enabled = false;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class Grunt : BossMonster
{
    [SerializeField] private GameObject thunderboltEffect;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject explosionAttacker;

    
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");
    readonly private int hashAttackType = Animator.StringToHash("AttackType");
    readonly private int hashDeadTrigger = Animator.StringToHash("Dead");
    readonly private int hashSpeed = Animator.StringToHash("Speed");
    
    private CinemachineBasicMultiChannelPerlin noise;           // 카메라 노이즈(흔들림)

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
        if (!photonView.IsMine) return;
        
        if(CanAttackTarget())
        {
            ExecuteAttack();
        }
        else 
        {
            Move();
        }
        Die();
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
        StartCoroutine(DecideNextAttack(attackType));
    }

    // 죽음
    protected override void Die()
    {
        if(!isDead && curHp <= 0)
        {
            base.Die();
            isDead = true;
            TriggerDieAnim();
            PlaySFX("Grunt_Die");
            var bossCanvas = GetComponentInChildren<Canvas>();
            bossCanvas.gameObject.gameObject.SetActive(false);
            hitBoxCol.enabled = false;
            nav.isStopped = true;
        }
    }

    #region ** Coroutines **
    // 다음 공격 정하기
    private IEnumerator DecideNextAttack(int attackType)
    {
        nav.isStopped = true;
        isAttacking = true;
        anim.SetFloat("Speed", 0);

        // 공격사이 간격
        yield return new WaitForSeconds(1f);

        // 다음 공격을 랜덤하게 결정
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

    // 스킬 공격1
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
    }
    
    // 스킬 공격2
    private IEnumerator Explosion()
    {
        if (GameManager.Instance.isMultiPlaying && !PhotonNetwork.IsMasterClient)
            yield return null;

        explosionAttacker.TryGetComponent<GruntExplosion>(out GruntExplosion hitbox);
        if(hitbox == null)
        {
            hitbox = explosionAttacker.AddComponent<GruntExplosion>();
            hitbox.damage = damage * Random.Range(0.3f, 0.7f);
        }

        photonView.RPC(nameof(RPC_ActivteExplosionEffect), RpcTarget.All, targetPlayer.transform.position);

        yield return new WaitForSeconds(1f);

        photonView.RPC(nameof(RPC_ActivteExplosionAttacker), RpcTarget.All, targetPlayer.transform.position);

        yield return new WaitForSeconds(1f);

        photonView.RPC(nameof(RPC_DeactiveExplosion), RpcTarget.All);

        EndAttack();
    }

    // 근접 공격 판정(애니메이션 이벤트)
    private void MeleeAttack()
    {
        if (GameManager.Instance.isMultiPlaying && !PhotonNetwork.IsMasterClient)
            return;

        // Raycast할 위치, 방향
        Vector3 origin = transform.position + new Vector3(0, 1f, 0);
        Vector3 direction = transform.forward;

        RaycastHit hit;

        if(Physics.SphereCast(origin, 1f, direction, out hit, 2f, LayerMask.GetMask("Player")))
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

    private void TriggerAttackAnim()
    {
        anim.SetTrigger(hashAttackTrigger);
    }

    private void TriggerDieAnim()
    {
        anim.SetTrigger(hashDeadTrigger);
    }

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

    #endregion
}

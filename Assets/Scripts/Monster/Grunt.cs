using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : BossMonster
{
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");
    readonly private int hashAttackType = Animator.StringToHash("AttackType");
    readonly private int hashDeadTrigger = Animator.StringToHash("Dead");
    readonly private int hashSpeed = Animator.StringToHash("Speed");

    protected override void Awake()
    {
        // 부모(Monster)의 초기화
        base.Awake();
        InitData();
    }

    private void Update()
    {
        if (targetPlayer == null || isDead)
            return;

        Move();
        Attack();
        Die();
    }

    // 보스 데이터 초기화
    private void InitData()
    {
        maxHp = 15;
        curHp = maxHp;
        speed = 1.5f;
        damage = 15f;
        attackRange = 3f;

        nav.speed = speed;
    }

    // 이동
    private void Move()
    {
        if (isAttacking)
            return;

        nav.SetDestination(targetPlayer.transform.position);
        anim.SetFloat(hashSpeed, speed);
    }

    // 공격
    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;

        // 공격범위안에 들어왔을 때
        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            transform.LookAt(targetPlayer.transform);
            StartCoroutine(DecideNextAttack());
        }
    }

    // 죽음
    private void Die()
    {
        if(!isDead && curHp <= 0)
        {
            isDead = true;
            anim.SetTrigger(hashDeadTrigger);
            hitBoxCol.enabled = false;
            nav.isStopped = true;
        }
    }

    // 다음 공격 정하기
    private IEnumerator DecideNextAttack()
    {
        nav.isStopped = true;
        isAttacking = true;
        anim.SetFloat(hashSpeed, 0);

        int randomAct = Random.Range(0, 2);

        // 다음 공격을 랜덤하게 결정
        switch(randomAct)
        {
            case 0:
                anim.SetInteger(hashAttackType, randomAct);
                anim.SetTrigger(hashAttackTrigger);
                break;
            case 1:
                anim.SetInteger(hashAttackType, randomAct);
                anim.SetTrigger(hashAttackTrigger);
                break;
        }

        yield return null;
    }

    // 근접 공격 판정(애니메이션 이벤트)
    private void MeleeAttack()
    {
        // Raycast할 위치, 방향
        Vector3 origin = transform.position + new Vector3(0, 1f, 0);
        Vector3 direction = transform.forward;

        RaycastHit hit;

        if(Physics.SphereCast(origin, 1f, direction, out hit, 2f, LayerMask.GetMask("Player")))
        {
            if(hit.collider.CompareTag("Player"))
            {
                // 플레이어 데미지
                PlayerData playerData = DataManager.Instance.GetPlayerData();
                playerData.GetDamaged(Random.Range(damage * 0.8f, damage * 1.2f));
            }
        }
    }

    // 공격 끝(애니메이션 이벤트)
    private void EndAttack()
    {
        isAttacking = !isAttacking;
        nav.isStopped = false;
    }
}

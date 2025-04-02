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
        // �θ�(Monster)�� �ʱ�ȭ
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

    // ���� ������ �ʱ�ȭ
    private void InitData()
    {
        maxHp = 15;
        curHp = maxHp;
        speed = 1.5f;
        damage = 15f;
        attackRange = 3f;

        nav.speed = speed;
    }

    // �̵�
    private void Move()
    {
        if (isAttacking)
            return;

        nav.SetDestination(targetPlayer.transform.position);
        anim.SetFloat(hashSpeed, speed);
    }

    // ����
    private void Attack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;

        // ���ݹ����ȿ� ������ ��
        if (!isAttacking && distanceToPlayer <= attackRange)
        {
            transform.LookAt(targetPlayer.transform);
            StartCoroutine(DecideNextAttack());
        }
    }

    // ����
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

    // ���� ���� ���ϱ�
    private IEnumerator DecideNextAttack()
    {
        nav.isStopped = true;
        isAttacking = true;
        anim.SetFloat(hashSpeed, 0);

        int randomAct = Random.Range(0, 2);

        // ���� ������ �����ϰ� ����
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

    // ���� ���� ����(�ִϸ��̼� �̺�Ʈ)
    private void MeleeAttack()
    {
        // Raycast�� ��ġ, ����
        Vector3 origin = transform.position + new Vector3(0, 1f, 0);
        Vector3 direction = transform.forward;

        RaycastHit hit;

        if(Physics.SphereCast(origin, 1f, direction, out hit, 2f, LayerMask.GetMask("Player")))
        {
            if(hit.collider.CompareTag("Player"))
            {
                // �÷��̾� ������
                PlayerData playerData = DataManager.Instance.GetPlayerData();
                playerData.GetDamaged(Random.Range(damage * 0.8f, damage * 1.2f));
            }
        }
    }

    // ���� ��(�ִϸ��̼� �̺�Ʈ)
    private void EndAttack()
    {
        isAttacking = !isAttacking;
        nav.isStopped = false;
    }
}

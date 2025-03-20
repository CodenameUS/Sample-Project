using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �÷��̾� ����� ���õ� ���ۼ��� �� �ִϸ��̼�
/// 1. Move(������)
/// 2. Turn(ȸ��)
/// 3. Attack(����)
/// 4. Dead(����)
/// </summary>
public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;

    readonly private int hashSpeed = Animator.StringToHash("Speed");
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");
    readonly private int hashDeadTrigger = Animator.StringToHash("Dead");

    private float hAxis;
    private float vAxis;
    private float baseSpeed = 3f;

    private bool isAttackKeyDown;
    private bool isAttacking = false;
    private bool isDead = false;

    public bool isCutscenePlaying = false;

    
    private Vector3 moveVec;
    private Rigidbody rigid;
    private Animator anim;

    public Animator Anim => anim;
    
    private void Awake()
    {
        Init();
    }
    
    private void Update()
    {
        playerData = DataManager.Instance.GetPlayerData();

        GetInput();
        Move();
        Turn();
        Attack();
        Dead();

        // �ƾ����ȿ��� Idle �ִϸ��̼�
        if (isCutscenePlaying)
            anim.SetFloat(hashSpeed, 0);
    }

    private void Init()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void GetInput()
    {
        if (isDead || isCutscenePlaying)
            return;

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        isAttackKeyDown = Input.GetButtonDown("Attack");
    }

    // �÷��̾� �̵�����
    private void Move()
    {
        if (isAttacking || isDead || isCutscenePlaying)
            return;

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        rigid.position += moveVec * (baseSpeed + playerData.Speed) * Time.deltaTime;
        anim.SetFloat(hashSpeed, moveVec == Vector3.zero ? 0 : (baseSpeed + playerData.Speed));
    }
    
    // �÷��̾� ȸ������
    private void Turn()
    {
        if (isAttacking || moveVec == Vector3.zero || isDead || isCutscenePlaying)
            return;

        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, playerData.RotateSpeed * Time.deltaTime);
    }

    // �÷��̾� ����
    private void Attack()
    {
        if (isAttackKeyDown && !isAttacking && !isDead && !isCutscenePlaying)
        {
            anim.SetTrigger(hashAttackTrigger);
        }
    }

    // �÷��̾� ����
    private void Dead()
    {
        if(playerData.CurHp <= 0 && !isDead)
        {
            isDead = true;
            anim.SetTrigger(hashDeadTrigger);
        }
    }

    #region ** Animation Events **
    private void IsAttacking() => isAttacking = !isAttacking;

    private void EnableAttackHitbox() => WeaponManager.Instance.currentWeapon.SetHitBox(true);
    
    private void DisableAttackHitbox() => WeaponManager.Instance.currentWeapon.SetHitBox(false);

    private void TriggerAttack() => WeaponManager.Instance.currentWeapon.Attack();

    private void EnableEffect() => WeaponManager.Instance.currentWeapon.SetEffect(true);

    private void DisableEffect() => WeaponManager.Instance.currentWeapon.SetEffect(false);

    #endregion
}

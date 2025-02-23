using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �÷��̾� ����� ���õ� ���ۼ���
/// 1. Move(������)
/// 2. Turn(ȸ��)
/// 3. Attack(���� �ִϸ��̼�)
/// </summary>
public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;

    readonly private int hashSpeed = Animator.StringToHash("Speed");
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");

    private float hAxis;
    private float vAxis;
    private bool isAttackKeyDown;
    public bool isAttacking = false;
    private float baseSpeed = 3f;
    
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
    }

    private void Init()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        isAttackKeyDown = Input.GetButtonDown("Attack");
    }

    // �÷��̾� �̵�����
    private void Move()
    {
        if (isAttacking)
            return;

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        rigid.position += moveVec * (baseSpeed + playerData.Speed) * Time.deltaTime;
        anim.SetFloat(hashSpeed, moveVec == Vector3.zero ? 0 : (baseSpeed + playerData.Speed));
    }
    
    // �÷��̾� ȸ������
    private void Turn()
    {
        if (isAttacking || moveVec == Vector3.zero)
            return;

        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, playerData.RotateSpeed * Time.deltaTime);
    }

    // �÷��̾� ����
    private void Attack()
    {
        if (isAttackKeyDown && !isAttacking)
        {
            anim.SetTrigger(hashAttackTrigger);
        }
    }

    #region ** Animation Events **
    private void IsAttacking() => isAttacking = !isAttacking;

    private void EnableAttackHitbox() => WeaponManager.Instance.currentWeapon.SetHitBox(true);
    
    private void DisableAttackHitbox() => WeaponManager.Instance.currentWeapon.SetHitBox(false);

    private void TriggerAttack() => WeaponManager.Instance.currentWeapon.Attack();
    #endregion
}

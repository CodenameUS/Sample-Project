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
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        rigid.position += moveVec * (baseSpeed + playerData.Speed) * Time.deltaTime;
        anim.SetFloat(hashSpeed, moveVec == Vector3.zero ? 0 : (baseSpeed + playerData.Speed));
    }
    
    // �÷��̾� ȸ������
    private void Turn()
    {
        // ĳ���Ͱ� �������� ��
        if (moveVec == Vector3.zero)
            return;
        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, playerData.RotateSpeed * Time.deltaTime);
    }

    // �÷��̾� ����
    private void Attack()
    {
        if (isAttackKeyDown)
        {
            anim.SetTrigger(hashAttackTrigger);
        }
    }

    private void EnableHitbox()
    {
        WeaponManager.Instance.currentWeapon.SetHitBox(true);
    }

    private void DisableHitbox()
    {
        WeaponManager.Instance.currentWeapon.SetHitBox(false);
    }
}

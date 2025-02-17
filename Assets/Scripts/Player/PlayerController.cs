using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 플레이어 제어와 관련된 동작수행
/// 1. Move(움직임)
/// 2. Turn(회전)
/// 3. Attack(공격 애니메이션)
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

    // 플레이어 이동로직
    private void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        rigid.position += moveVec * (baseSpeed + playerData.Speed) * Time.deltaTime;
        anim.SetFloat(hashSpeed, moveVec == Vector3.zero ? 0 : (baseSpeed + playerData.Speed));
    }
    
    // 플레이어 회전로직
    private void Turn()
    {
        // 캐릭터가 정지했을 때
        if (moveVec == Vector3.zero)
            return;
        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, playerData.RotateSpeed * Time.deltaTime);
    }

    // 플레이어 공격
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

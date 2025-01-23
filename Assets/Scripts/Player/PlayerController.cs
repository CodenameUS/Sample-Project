using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 제어와 관련된 동작수행
/// 1. Move(움직임)
/// 2. Turn(회전)
/// </summary>
public class PlayerController : MonoBehaviour
{
    private PlayerData playerData;

    readonly private int hashSpeed = Animator.StringToHash("Speed");

    private float hAxis;
    private float vAxis;

    private Vector3 moveVec;
    private Rigidbody rigid;
    private Animator anim;

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
    }

    // 플레이어 이동로직
    public void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        rigid.position += moveVec * playerData.Speed * Time.deltaTime;
        anim.SetFloat(hashSpeed, moveVec == Vector3.zero ? 0 : playerData.Speed);
    }
    
    // 플레이어 회전로직
    public void Turn()
    {
        // 캐릭터가 정지했을 때
        if (moveVec == Vector3.zero)
            return;
        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, playerData.RotateSpeed * Time.deltaTime);
    }
}

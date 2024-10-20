using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput    playerInput;
    [SerializeField] private PlayerInfo     playerInfo;

    private Vector3 moveVec;

    private Rigidbody rigid;
    private Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Turn();
    }

    // 플레이어 이동로직
    void Move()
    {
        moveVec = new Vector3(playerInput.hAxis, 0, playerInput.vAxis).normalized;

        rigid.position += moveVec * playerInfo.speed * Time.deltaTime;
        anim.SetBool("Running", moveVec != Vector3.zero);
    }

    // 플레이어 회전로직
    void Turn()
    {
        // 캐릭터가 정지했을 때
        if (moveVec == Vector3.zero)
            return;

        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, playerInfo.rotateSpeed * Time.deltaTime);
    }
}

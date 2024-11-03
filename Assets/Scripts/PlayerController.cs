using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput    playerInput;
    [SerializeField] private PlayerInfo     playerInfo;

    readonly private int hashSpeed = Animator.StringToHash("Speed");

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

    // �÷��̾� �̵�����
    void Move()
    {
        moveVec = new Vector3(playerInput.hAxis, 0, playerInput.vAxis).normalized;

        rigid.position += moveVec * playerInfo.speed * Time.deltaTime;
        anim.SetFloat(hashSpeed, moveVec == Vector3.zero ? 0 : playerInfo.speed);
    }

    // �÷��̾� ȸ������
    void Turn()
    {
        // ĳ���Ͱ� �������� ��
        if (moveVec == Vector3.zero)
            return;

        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, playerInfo.rotateSpeed * Time.deltaTime);
    }
}

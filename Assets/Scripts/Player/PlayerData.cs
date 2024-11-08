using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private float maxHp;                      // �ִ�ü��
    private float curHp;                      // ����ü��
    private float maxMp;                      // �ִ븶��
    private float curMp;                      // ���縶��
    private float speed;                      // �̵��ӵ�
    private float rotateSpeed;                // ȸ���ӵ�

    public float MaxHp => maxHp;
    public float CurHp => curHp;
    public float MaxMp => maxMp;
    public float CurMp => curMp;
    public float Speed => speed;
    public float RotateSpeed => rotateSpeed;

    public PlayerData(float maxHp, float curHp, float maxMp, float curMp, float speed, float rotateSpeed)
    {
        this.maxHp = maxHp;
        this.curHp = curHp;
        this.maxMp = maxMp;
        this.curMp = curMp;
        this.speed = speed;
        this.rotateSpeed = rotateSpeed;
    }

    // �ӽ�(�÷��̾� Hp ����)
    public void ModifyPlayerCurHp()
    {
        // �ӽ�
        curHp -= 100f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private float maxHp;                      // �ִ�ü��
    [SerializeField] private float curHp;                      // ����ü��
    [SerializeField] private float maxMp;                      // �ִ븶��
    [SerializeField] private float curMp;                      // ���縶��
    [SerializeField] private float speed;                      // �̵��ӵ�
    [SerializeField] private float rotateSpeed;                // ȸ���ӵ�

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

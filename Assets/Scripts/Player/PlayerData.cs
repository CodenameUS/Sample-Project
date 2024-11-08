using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private float maxHp;                      // 최대체력
    private float curHp;                      // 현재체력
    private float maxMp;                      // 최대마나
    private float curMp;                      // 현재마나
    private float speed;                      // 이동속도
    private float rotateSpeed;                // 회전속도

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

    // 임시(플레이어 Hp 수정)
    public void ModifyPlayerCurHp()
    {
        // 임시
        curHp -= 100f;
    }
}

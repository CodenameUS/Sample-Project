using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    [SerializeField] private float maxHp;
    [SerializeField] private float curHp;
    [SerializeField] private float maxMp;
    [SerializeField] private float curMp;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float armor;


    public float MaxHp => maxHp;
    public float CurHp => curHp;
    public float MaxMp => maxMp;
    public float CurMp => curMp;
    public float Speed => speed;
    public float RotateSpeed => rotateSpeed;
    public float Damage => damage;
    public float Armor => armor;

    // 생성자 - Status 초기화
    public PlayerData(PlayerDataDTO.StatusDTO dto)
    {
        this.maxHp = dto.maxHp;
        this.curHp = dto.curHp;
        this.maxMp = dto.maxMp;
        this.curMp = dto.curMp;
        this.speed = dto.speed;
        this.rotateSpeed = dto.rotateSpeed;
        this.damage = dto.damage;
        this.armor = dto.armor;
    }

    // 임시(플레이어 Hp 수정)
    public void ModifyPlayerCurHp()
    {
        // 임시
        curHp -= 100f;
    }

    public void GetDamaged(float damage)
    {
        Debug.Log(damage + "만큼 데미지 입음");
        curHp -= damage;
    }
}

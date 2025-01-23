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

    // ������ - Status �ʱ�ȭ
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

    // �ӽ�(�÷��̾� Hp ����)
    public void ModifyPlayerCurHp()
    {
        // �ӽ�
        curHp -= 100f;
    }

    public void GetDamaged(float damage)
    {
        Debug.Log(damage + "��ŭ ������ ����");
        curHp -= damage;
    }
}

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
    [SerializeField] private float defense;
    [SerializeField] private int gold;

    public float MaxHp => maxHp;
    public float CurHp => curHp;
    public float MaxMp => maxMp;
    public float CurMp => curMp;
    public float Speed => speed;
    public float RotateSpeed => rotateSpeed;
    public float Damage => damage;
    public float Defense => defense;
    public int Gold => gold;

    // 플레이어 데이터 초기화
    public PlayerData(PlayerDataDTO.StatusDTO status)
    {
        this.maxHp = status.maxHp;
        this.curHp = status.curHp;
        this.maxMp = status.maxMp;
        this.curMp = status.curMp;
        this.speed = status.speed;
        this.rotateSpeed = status.rotateSpeed;
        this.damage = status.damage;
        this.defense = status.defense;
        this.gold = status.gold;
    }

    
    // 임시(플레이어 Hp 수정)
    public void ModifyPlayerCurHp()
    {
        // 임시
        curHp -= 100f;
    }

    // 포션 회복
    public void UsePortion(float value, string type)
    {
        switch(type)
        {
            case "Health":
                curHp += value;
                break;
            case "Mana":
                curMp += value;
                break;
        }
    }

    // 장비 장착
    public void EquipItem(float value, string type)
    {
        switch(type)
        {
            case "Weapon":
                damage += value;
                break;
            case "Armor":
                defense += value;
                break;
        }
    }

    // 장비 해제
    public void UnequipItem(float value, string type)
    {
        switch(type)
        {
            case "Weapon":
                damage -= value;
                break;
            case "Armor":
                defense -= value;
                break;
        }
    }

    // 데미지 입음
    public void GetDamaged(float damage)
    {
        Debug.Log(damage + "만큼 데미지 입음");
        curHp -= damage;
        if (curHp <= 0)
            curHp = 0;
    }

    // 골드 사용
    public void UseGold(int amount)
    {
        gold = (gold - amount) < 0 ? 0 : gold - amount;
    }
}

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

    // �÷��̾� ������ �ʱ�ȭ
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

    
    // �ӽ�(�÷��̾� Hp ����)
    public void ModifyPlayerCurHp()
    {
        // �ӽ�
        curHp -= 100f;
    }

    // ���� ȸ��
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

    // ��� ����
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

    // ��� ����
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

    // ������ ����
    public void GetDamaged(float damage)
    {
        Debug.Log(damage + "��ŭ ������ ����");
        curHp -= damage;
        if (curHp <= 0)
            curHp = 0;
    }

    // ��� ���
    public void UseGold(int amount)
    {
        gold = (gold - amount) < 0 ? 0 : gold - amount;
    }
}

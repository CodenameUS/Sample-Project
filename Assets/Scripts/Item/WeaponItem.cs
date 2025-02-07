using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                WeaponItem : ���� ������
                
                Equip() : ���� ����
                    - ���� �������� ��ġ��ŭ �÷��̾� �ɷ�ġ ���
                Unequip() : �� ����
                    - ���� �������� ��ġ��ŭ �÷��̾� �ɷ�ġ �϶�
 */

public class WeaponItem : EquipmentItem
{
    public WeaponItemData WeaponData { get; private set; }
    public WeaponItem(WeaponItemData data) : base(data) 
    {
        WeaponData = data;
    }

    // ����
    public override void Equip()
    {
        DataManager.Instance.GetPlayerData().EquipItem(WeaponData.Damage, WeaponData.Type);
    }

    // ���� ����
    public override void Unequip()
    {
        DataManager.Instance.GetPlayerData().UnequipItem(WeaponData.Damage, WeaponData.Type);
    }
}

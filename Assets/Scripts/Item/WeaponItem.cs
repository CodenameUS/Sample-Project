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

public class WeaponItem : EquipmentItem, IEquipableItem
{
    public WeaponItemData WeaponData { get; private set; }
    public WeaponItem(WeaponItemData data) : base(data) 
    {
        WeaponData = data;
    }

    // ����
    public bool Equip()
    {
        DataManager.Instance.GetPlayerData().EquipItem(WeaponData.Damage, WeaponData.Type);

        return true;
    }

    // ���� ����
    public void Unequip()
    {
        DataManager.Instance.GetPlayerData().UnequipItem(WeaponData.Damage, WeaponData.Type);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemData : EquipmentItemData
{
    public int Damage { get; private set; }             // ���� ������
    public float Rate { get; private set; }             // ���ݼӵ�


    public WeaponItemData(WeaponItemDTO dto)
    {
        this.id = dto.id;
        this.itemName = dto.itemName;
        this.itemToolTip = dto.itemToolTip;
        this.Damage = dto.damage;
        this.Rate = dto.rate;
    }

    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}

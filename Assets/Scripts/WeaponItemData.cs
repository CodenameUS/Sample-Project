using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemData : EquipmentItemData
{
    public int Damage { get; private set; }             // 무기 데미지
    public float Rate { get; private set; }             // 공격속도


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

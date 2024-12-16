using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemData : EquipmentItemData
{
    public int Damage => damage;
    public float Rate => rate;             

    [SerializeField] private int damage;        // 무기 데미지
    [SerializeField] private float rate;        // 공격속도

    public WeaponItemData(WeaponItemDTO dto)
    {
        this.id = dto.id;
        this.itemName = dto.itemName;
        this.itemToolTip = dto.itemToolTip;
        this.itemIcon = dto.itemIcon;
        this.damage = dto.damage;
        this.rate = dto.rate;
    }

    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}

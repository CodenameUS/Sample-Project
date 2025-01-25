using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem, IEquipableItem
{
    public WeaponItem(WeaponItemData data) : base(data) { }

    public bool Equip()
    {
        return true;
    }
}

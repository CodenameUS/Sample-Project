using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                EquipmentItem : 장착 아이템
                
               
 */
public abstract class EquipmentItem : Item
{
    public EquipmentItemData EquipmentData { get; private set; }

    public EquipmentItem(EquipmentItemData data) : base(data)
    {
        EquipmentData = data;
    }
}

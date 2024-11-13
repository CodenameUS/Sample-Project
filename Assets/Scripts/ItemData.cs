using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                ItemData
                    - EquipmentItemData
                        - WeaponItemData
 */

public abstract class ItemData 
{
    [SerializeField] protected int id;                        // 아이템 id
    [SerializeField] protected string itemName;               // 아이템 이름
    [SerializeField] protected string itemToolTip;            // 아이템 툴팁

    public int ID => id;
    public string ItemName => itemName;
    public string ItemToolTip => itemToolTip;


    // 아이템 생성
    public abstract Item CreateItem();  
}

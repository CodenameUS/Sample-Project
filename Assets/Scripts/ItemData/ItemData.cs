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
    [SerializeField] protected int id;                        // ������ id
    [SerializeField] protected string itemName;               // ������ �̸�
    [SerializeField] protected string itemToolTip;            // ������ ����
    [SerializeField] protected string itemIcon;               // ������ ������ �̸�

    public int ID => id;
    public string ItemName => itemName;
    public string ItemToolTip => itemToolTip;
    public string ItemIcon => itemIcon;

    // ������ ����
    public abstract Item CreateItem();  
}

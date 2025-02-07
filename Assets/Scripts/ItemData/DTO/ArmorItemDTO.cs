using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                ArmorItemData 직렬화 전용 클래스
 */
[System.Serializable]
public class ArmorItemDTO
{
    public int id;                      // 아이템 id
    public string itemName;             // 아이템 이름
    public string itemToolTip;          // 아이템 툴팁
    public string itemIcon;             // 아이템 아이콘 이름
    public int defense;                 // 방어력
    public string type;                 // 장비타입
    public string subType;              // 장비세부타입
}

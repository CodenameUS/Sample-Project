using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                WeaponItemData 직렬화 전용 클래스
 */

[System.Serializable]
public class WeaponItemDTO
{
    public int id;                      // 아이템 id
    public string itemName;             // 아이템 이름
    public string itemToolTip;          // 아이템 툴팁
    public int damage;                  // 무기 데미지
    public float rate;                  // 공격속도
}

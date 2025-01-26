using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                PortionItem : 포션 아이템
                
                Use() : IUsableItem 인터페이스 상속
                    - (임시) 갯수 하나 차감, 성공여부 반환
 */

public class PortionItem : CountableItem, IUsableItem
{
    public PortionItemData PortionData { get; private set; }
    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount) 
    {
        PortionData = data;
    }

    // 아이템 사용
    public bool Use()
    {
        Amount--;

        DataManager.Instance.GetPlayerData().UsePortion(PortionData.Value, PortionData.PortionType);

        return true;
    }
}

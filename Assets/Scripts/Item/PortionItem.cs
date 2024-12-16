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
    public PortionItem(PortionItemData data, int amount = 1) : base(data, amount) { }

    public bool Use()
    {
        Amount--;

        return true;
    }
}

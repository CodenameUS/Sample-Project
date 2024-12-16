using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
            CountableItemData : 갯수가 있는 아이템 데이터
    
 */
public abstract class CountableItemData : ItemData
{
    [SerializeField] protected int maxAmount;             // 최대 수량
    public int MaxAmount => maxAmount;
}

using UnityEngine;

/*
                            << CountableItem >>

        - 카테고리 : 셀 수 있는 아이템 클래스

        - SetAmount(int amount)
            - 한 슬롯에 들어갈 수 있는 최대 아이템 갯수 : 99

        - AddAmountAndGetExcess : 아이템 갯수를 합치고, 최대량 초과분을 반환
 */

public abstract class CountableItem : Item
{
    public CountableItemData CountableData { get; private set; }

    // 현재 아이템 수량
    public int Amount { get; protected set; }
    // 한 슬롯의 최대 수량
    public int MaxAmount => CountableData.MaxAmount;
    // 수량이 가득찼는지 여부
    public bool IsMax => Amount >= CountableData.MaxAmount;
    // 개수가 있는지 여부
    public bool IsEmpty => Amount <= 0;


    public CountableItem(CountableItemData data, int amount = 1) : base(data)
    {
        CountableData = data;
        SetAmount(amount);
    }

    // 한 슬롯의 갯수 범위 제한
    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }

    // 갯수 합치기 및 초과량 반환
    public int AddAmountAndGetExcess(int amount)
    {
        int nextAmount = Amount + amount;
        SetAmount(nextAmount);

        // 최대치를 초과시 초과량 반환, 초과하지않으면 0 반환
        return (nextAmount > MaxAmount) ? (nextAmount - MaxAmount) : 0;
    }
}

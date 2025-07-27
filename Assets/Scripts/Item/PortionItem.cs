

/*
                            << PortionItem >>
        - 포션 아이템 클래스 

        - Use() : 포션 사용
            - 아이템 갯수 하나 차감
            - 플레이어 체력 회복
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
        // 갯수 -1
        Amount--;

        // 아이템 사용에 따른 플레이어 능력치 반영
        DataManager.Instance.GetPlayerData().UsePortion(PortionData.Value, PortionData.PortionType);

        return true;
    }
}

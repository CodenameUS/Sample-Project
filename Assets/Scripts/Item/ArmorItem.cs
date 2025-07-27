
/*
                            << ArmorItem >>

        - Equip() : 방어구 장착
            - 방어구 데이터의 수치만큼 플레이어 능력치 상승
        
        
        - Unequip() : 방어구 해제
            - 방어구 데이터의 수치만큼 플레이어 능력치 하락
 */


public class ArmorItem : EquipmentItem
{
    public ArmorItemData ArmorData { get; private set; }
    public ArmorItem(ArmorItemData data) : base(data)
    {
        ArmorData = data;
    }
    
    public override void Equip()
    {
        // 장비착용에 따른 플레이어 능력치 반영
        DataManager.Instance.GetPlayerData().EquipItem(ArmorData.Defense, ArmorData.Type);
    }

    public override void Unequip()
    {
        // 장비해제 따른 플레이어 능력치 반영
        DataManager.Instance.GetPlayerData().UnequipItem(ArmorData.Defense, ArmorData.Type);
    }
}

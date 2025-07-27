
/*
                            << EquipmentItem >>

        - 카테고리 : 장착가능한(장비) 아이템 클래스

        - Equip() : 아이템 장착
            
        - UnEquip() : 아이템 장착 해제
 */

public abstract class EquipmentItem : Item, IEquipableItem
{
    public EquipmentItemData EquipmentData { get; private set; }

    public EquipmentItem(EquipmentItemData data) : base(data)
    {
        EquipmentData = data;
    }

    public virtual void Equip() { }
    public virtual void Unequip() { }
}

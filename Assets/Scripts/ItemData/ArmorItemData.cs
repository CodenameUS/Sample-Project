using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                ArmorItemData : �� ������ ������
                
                ������ : �����͸� �޾� �ʱ�ȭ
                CreateItem() : �ʱ�ȭ�� �����ͷ� ������ ��ü ����
 */

public class ArmorItemData : EquipmentItemData
{
    [SerializeField] private int defense;       // ����
    [SerializeField] private string type;       // ���Ÿ��
    [SerializeField] private string subType;    // ��� ����Ÿ��
    public int Defense => defense;
    public string Type => type;
    public string SubType => subType;

    public ArmorItemData(ArmorItemDTO dto)
    {
        this.id = dto.id;
        this.itemName = dto.itemName;
        this.itemToolTip = dto.itemToolTip;
        this.itemIcon = dto.itemIcon;
        this.defense = dto.defense;
        this.type = dto.type;
        this.subType = dto.subType;
    }

    public override Item CreateItem()
    {
        return new ArmorItem(this);
    }
}

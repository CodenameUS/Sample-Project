using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                WeaponItemData : ���� ������ ������
                
                ������ : �����͸� �޾� �ʱ�ȭ
                CreateItem() : �ʱ�ȭ�� �����ͷ� ������ ��ü ����
 */

public class WeaponItemData : EquipmentItemData
{
    [SerializeField] private int damage;        // ���� ������
    [SerializeField] private float rate;        // ���ݼӵ�
    [SerializeField] private string type;       // ��� Ÿ��

    public int Damage => damage;
    public float Rate => rate;
    public string Type => type;

    public WeaponItemData(WeaponItemDTO dto)
    {
        this.id = dto.id;
        this.itemName = dto.itemName;
        this.itemToolTip = dto.itemToolTip;
        this.itemIcon = dto.itemIcon;
        this.damage = dto.damage;
        this.rate = dto.rate;
        this.type = dto.type;
    }

    public override Item CreateItem()
    {
        return new WeaponItem(this);
    }
}

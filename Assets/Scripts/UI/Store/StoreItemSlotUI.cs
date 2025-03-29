using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemSlotUI : MonoBehaviour
{
    public int itemId;                                          // ������ ID
    [SerializeField] private Image  itemIcon;                   // ������ ������
    [SerializeField] private Text   itemNameText;               // ������ �̸�
    [SerializeField] private Text   itemExplanation;            // ������ ����
    [SerializeField] private Text   itemPrice;                  // ������ ����
    [SerializeField] private Button perchaseBtn;                // ���� ��ư

    private ArmorItemData armorItemData;
    private WeaponItemData weaponItemData;
    private PortionItemData portionItemData;

    private void Start()
    {
        GetItemData();   
    }

    // ������ Ÿ�Ժ� ������ �ҷ�����
    private void GetItemData()
    {
        // 1. ���Ǿ�����
        if(itemId > 10000 && itemId < 20000)
        {
            portionItemData = DataManager.Instance.GetPortionDataById(itemId);
            SetSlotData(portionItemData);
        }
        // 2. ��������
        else if(itemId > 20000 && itemId < 30000)
        {
            armorItemData = DataManager.Instance.GetArmorDataById(itemId);
            SetSlotData(armorItemData);
        }
        // 3. ���������
        else if(itemId > 30000 && itemId < 40000)
        {
            weaponItemData = DataManager.Instance.GetWeaponDataById(itemId);
            SetSlotData(weaponItemData);
        }

    }

    // ������ ���� ���� ä���
    private void SetSlotData(ItemData data)
    {
        // 1. ������
        ResourceManager.Instance.LoadIcon(data.ItemIcon, sprite =>
        {
            // ����
            if (sprite != null)
            {
                itemIcon.sprite = sprite;
            }
            else
            {
                Debug.Log($"Failed to load icon for item : {data.ItemIcon}");
            }
        });

        // 2. ������ �̸�
        itemNameText.text = data.ItemName;

        // 3. ������ ����
        itemExplanation.text = data.ItemExplanation;

        // 4. ������ ����
        itemPrice.text = data.ItemPrice.ToString() + "G";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemSlotUI : MonoBehaviour
{
    public int itemId;                                          // 아이템 ID
    [SerializeField] private Image  itemIcon;                   // 아이템 아이콘
    [SerializeField] private Text   itemNameText;               // 아이템 이름
    [SerializeField] private Text   itemExplanation;            // 아이템 설명
    [SerializeField] private Text   itemPrice;                  // 아이템 가격
    [SerializeField] private Button perchaseBtn;                // 구매 버튼

    private ArmorItemData armorItemData;
    private WeaponItemData weaponItemData;
    private PortionItemData portionItemData;

    private void Start()
    {
        GetItemData();   
    }

    // 아이템 타입별 데이터 불러오기
    private void GetItemData()
    {
        // 1. 포션아이템
        if(itemId > 10000 && itemId < 20000)
        {
            portionItemData = DataManager.Instance.GetPortionDataById(itemId);
            SetSlotData(portionItemData);
        }
        // 2. 방어구아이템
        else if(itemId > 20000 && itemId < 30000)
        {
            armorItemData = DataManager.Instance.GetArmorDataById(itemId);
            SetSlotData(armorItemData);
        }
        // 3. 무기아이템
        else if(itemId > 30000 && itemId < 40000)
        {
            weaponItemData = DataManager.Instance.GetWeaponDataById(itemId);
            SetSlotData(weaponItemData);
        }

    }

    // 아이템 슬롯 정보 채우기
    private void SetSlotData(ItemData data)
    {
        // 1. 아이콘
        ResourceManager.Instance.LoadIcon(data.ItemIcon, sprite =>
        {
            // 성공
            if (sprite != null)
            {
                itemIcon.sprite = sprite;
            }
            else
            {
                Debug.Log($"Failed to load icon for item : {data.ItemIcon}");
            }
        });

        // 2. 아이템 이름
        itemNameText.text = data.ItemName;

        // 3. 아이템 설명
        itemExplanation.text = data.ItemExplanation;

        // 4. 아이템 가격
        itemPrice.text = data.ItemPrice.ToString() + "G";
    }
}

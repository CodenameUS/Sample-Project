using UnityEngine;
using UnityEngine.UI;

/*
                            << PlayerItemSlotUI >>

        - 플레이어 퀵슬롯 관리

        - 퀵슬롯에 아이템 등록
            - 인벤토리 연동 : 아이템 정보 동기화
 */

public class PlayerItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;        // 아이템 아이콘
    [SerializeField] private Text amount;       // 아이템 갯수
    public int index;                           // 퀵슬롯 인덱스

    private CountableItem slotItem;             // 이 슬롯의 아이템


    private void ShowAmount() => amount.gameObject.SetActive(true);
    private void HideAmount() => amount.gameObject.SetActive(false);

    // 슬롯 업데이트
    public void UpdateSlot()
    {
        SetItem(slotItem);
    }

    // 슬롯에 아이템 등록(아이콘이미지, 수량텍스트)
    public void SetItem(CountableItem item)
    {
        if (item == null)
        {
            return;
        }

        slotItem = item;

        ResourceManager.Instance.LoadIcon(item.Data.ItemIcon, sprite =>
        {
            if (sprite != null)
            {
                icon.sprite = sprite;
                icon.color = new Color(1f, 1f, 1f, 1f);

                if(slotItem.Amount > 1)
                {
                    ShowAmount();
                }
                else
                {
                    HideAmount();
                }
                
                amount.text = item.Amount.ToString();
            }
            else
            {
                Debug.Log($"Failed to load icon for item : {item.Data.ItemIcon}");
            }
        });
    }

    // 슬롯의 아이템 제거
    public void RemoveItem()
    {
        icon.sprite = null;
        icon.color = new Color(1f, 1f, 1f, 0f);
        slotItem = null;
        HideAmount();
    }

    // 해당 아이템이 등록되어있는지 여부
    public bool HasItem(CountableItem ci)
    {
        return slotItem == ci;
    }

    // 슬롯의 아이템 사용
    public void UseItem(Inventory iv)
    {
        iv.Use(slotItem);
    }
}

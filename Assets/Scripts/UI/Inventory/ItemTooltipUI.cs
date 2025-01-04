using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  
                        ItemTooltipUI 

           - : 아이템 툴팁 UI 표시
 */
public class ItemTooltipUI : MonoBehaviour
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemTooltipText;

    private RectTransform myRect;


    private void Awake()
    {
        HideTooltipUI();
        Init();
    }

    private void Init()
    {
        TryGetComponent(out myRect);
        myRect.pivot = new Vector2(0f, 1f); ;   
    }

    public void ShowTooltipUI() => gameObject.SetActive(true);
    public void HideTooltipUI() => gameObject.SetActive(false);

    public void SetItemInfo(ItemData data)
    {
        itemNameText.text = data.ItemName;
        itemTooltipText.text = data.ItemToolTip;
    }

    public void SetRectPosition(RectTransform slotRect)
    {
        float slotWidth = slotRect.rect.width;
        float slotHeight = slotRect.rect.height;

        // 툴팁 최초 위치 설정(슬롯 우측 하단)
        myRect.position = slotRect.position + new Vector3(slotWidth, -slotHeight);
        Vector2 pos = myRect.position;

        // 툴팁 크기
        float width = myRect.rect.width;
        float height = myRect.rect.height;

        // 우측 및 하단이 잘렸는 지 여부
        bool rightTruncated = pos.x + width > Screen.width;
        bool bottomTruncated = pos.y - height < 0f;

        ref bool R = ref rightTruncated;
        ref bool B = ref bottomTruncated;

        // 오른쪽 잘림
        if(R && !B)
        {
            myRect.position = new Vector2(pos.x - width - slotWidth, pos.y);
        }
        // 아래쪽 잘림
        else if(!R && B)
        {
            myRect.position = new Vector2(pos.x, pos.y + height + slotHeight);
        }
        // 둘 다 잘림
        else if(R && B)
        {
            myRect.position = new Vector2(pos.x - width - slotWidth, pos.y + height + slotHeight);
        }
    }
}

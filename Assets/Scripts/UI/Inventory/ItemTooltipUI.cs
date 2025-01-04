using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  
                        ItemTooltipUI 

           - : ������ ���� UI ǥ��
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

        // ���� ���� ��ġ ����(���� ���� �ϴ�)
        myRect.position = slotRect.position + new Vector3(slotWidth, -slotHeight);
        Vector2 pos = myRect.position;

        // ���� ũ��
        float width = myRect.rect.width;
        float height = myRect.rect.height;

        // ���� �� �ϴ��� �߷ȴ� �� ����
        bool rightTruncated = pos.x + width > Screen.width;
        bool bottomTruncated = pos.y - height < 0f;

        ref bool R = ref rightTruncated;
        ref bool B = ref bottomTruncated;

        // ������ �߸�
        if(R && !B)
        {
            myRect.position = new Vector2(pos.x - width - slotWidth, pos.y);
        }
        // �Ʒ��� �߸�
        else if(!R && B)
        {
            myRect.position = new Vector2(pos.x, pos.y + height + slotHeight);
        }
        // �� �� �߸�
        else if(R && B)
        {
            myRect.position = new Vector2(pos.x - width - slotWidth, pos.y + height + slotHeight);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform contentAreaRT;   // ������ ����
    [SerializeField] private GameObject itemSlotPrefab;     // ������ ���� ���� ������

    private int horizontalSlotCount = 6;                // ���� ���ΰ���
    private int verticalSlotCount = 6;                  // ���� ���ΰ���
    private float slotMargin = 9f;                      // ���� ����
    private float contentAreaPadding = 8f;             // �κ��丮 ���� ���� ����
    private float slotSize = 81f;                       // ���� ������

    private List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();

    private void Awake()
    {
        InitSlots();
    }

    // ���� ���� ���� ���� ����
    private void InitSlots()
    {
        // ���� ������ ����
        itemSlotPrefab.TryGetComponent(out RectTransform slotRect);

        // ���� ������ ����
        slotRect.sizeDelta = new Vector2(slotSize, slotSize);

        // ���Կ� ItemSlotUI ��ũ��Ʈ ���̱�
        itemSlotPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            itemSlotPrefab.AddComponent<ItemSlotUI>();

        // ���� ���� ��Ȱ��ȭ
        itemSlotPrefab.SetActive(false);

        // ������ ä�� ������ġ, ������ġ
        Vector2 beginPos = new Vector2(contentAreaPadding, -contentAreaPadding);
        Vector2 curPos = beginPos;

        // ������ ���Ե��� ���� ����Ʈ
        slotUIList = new List<ItemSlotUI>(verticalSlotCount * horizontalSlotCount);

        // ���� �����ϱ�
        for(int j = 0; j<verticalSlotCount; j++)
        {
            for(int i = 0;i<horizontalSlotCount;i++)
            {
                int slotIndex = (horizontalSlotCount * j) + i;       // ���� �ε���

                // �����ϳ� ����
                var slotRT = CloneSlot();
                // ������ �������� ����
                slotRT.pivot = new Vector2(0f, 1f);                  // Left Top ����
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]"; // ���̾��Ű�� �����̸�("Item Slot 0~35")

                var slotUI = slotRT.GetComponent<ItemSlotUI>();
                slotUI.SetSlotIndex(slotIndex);                      // ���Կ� �ε������̱�
                slotUIList.Add(slotUI);                              // ����Ʈ�� ������ �������� �߰�

                // ����ĭ(����)
                curPos.x += (slotMargin + slotSize);
            }

            // ������(����)
            curPos.x = beginPos.x;
            curPos.y -= (slotMargin + slotSize);
        }

        // ���� �����ı�
        if (itemSlotPrefab.scene.rootCount != 0)
            Destroy(itemSlotPrefab);

        RectTransform CloneSlot()
        {
            GameObject slotGo = Instantiate(itemSlotPrefab);
            RectTransform rt = slotGo.GetComponent<RectTransform>();
            rt.SetParent(contentAreaRT);

            return rt;
        }
    }
}

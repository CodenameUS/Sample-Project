using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private RectTransform contentAreaRT;   // 아이템 영역
    [SerializeField] private GameObject itemSlotPrefab;     // 복제할 원본 슬롯 프리팹

    private int horizontalSlotCount = 6;                // 슬롯 가로갯수
    private int verticalSlotCount = 6;                  // 슬롯 세로갯수
    private float slotMargin = 9f;                      // 슬롯 여백
    private float contentAreaPadding = 8f;             // 인벤토리 영역 내부 여백
    private float slotSize = 81f;                       // 슬롯 사이즈

    private List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();

    private void Awake()
    {
        InitSlots();
    }

    // 슬롯 영역 내에 슬롯 생성
    private void InitSlots()
    {
        // 슬롯 프리팹 설정
        itemSlotPrefab.TryGetComponent(out RectTransform slotRect);

        // 슬롯 사이즈 설정
        slotRect.sizeDelta = new Vector2(slotSize, slotSize);

        // 슬롯에 ItemSlotUI 스크립트 붙이기
        itemSlotPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            itemSlotPrefab.AddComponent<ItemSlotUI>();

        // 원본 슬롯 비활성화
        itemSlotPrefab.SetActive(false);

        // 슬롯을 채울 시작위치, 현재위치
        Vector2 beginPos = new Vector2(contentAreaPadding, -contentAreaPadding);
        Vector2 curPos = beginPos;

        // 아이템 슬롯들을 담을 리스트
        slotUIList = new List<ItemSlotUI>(verticalSlotCount * horizontalSlotCount);

        // 슬롯 생성하기
        for(int j = 0; j<verticalSlotCount; j++)
        {
            for(int i = 0;i<horizontalSlotCount;i++)
            {
                int slotIndex = (horizontalSlotCount * j) + i;       // 슬롯 인덱스

                // 슬롯하나 복제
                var slotRT = CloneSlot();
                // 복제된 슬롯정보 설정
                slotRT.pivot = new Vector2(0f, 1f);                  // Left Top 기준
                slotRT.anchoredPosition = curPos;
                slotRT.gameObject.SetActive(true);
                slotRT.gameObject.name = $"Item Slot [{slotIndex}]"; // 하이어라키상 슬롯이름("Item Slot 0~35")

                var slotUI = slotRT.GetComponent<ItemSlotUI>();
                slotUI.SetSlotIndex(slotIndex);                      // 슬롯에 인덱스붙이기
                slotUIList.Add(slotUI);                              // 리스트에 생성된 슬롯정보 추가

                // 다음칸(가로)
                curPos.x += (slotMargin + slotSize);
            }

            // 다음줄(세로)
            curPos.x = beginPos.x;
            curPos.y -= (slotMargin + slotSize);
        }

        // 슬롯 원본파괴
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

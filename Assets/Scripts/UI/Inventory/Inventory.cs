using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ItemData[] ItemDataArray;
    public int Capacity { get; private set; }   // 인벤토리 수용한도

    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] GameObject inventoryGo;
    [SerializeField] Item[] items;

    private bool inventoryKeydown;              // 인벤토리 키(I)
    private int initCapacity = 24;              // 초기 인벤토리 수용한도
    private int maxCapacity = 36;               // 최대 인벤토리 수용한도

    Sprite itemSprite;

    #region  ** Unity Events **
    private void Awake()
    {
        // 인벤토리에서 관리할 수 있는 아이템은 최대 36개
        items = new Item[maxCapacity];                  
        ItemDataArray = new ItemData[maxCapacity];

        // 초기 수용량 : 24(임시)
        Capacity = initCapacity;

        inventoryUI.SetInventoryRef(this);
    }

    private void Start()
    {
        UpdateAccessibleSlots();
        InitTest();
    }

    private void Update()
    {
        inventoryKeydown = Input.GetButtonDown("Inventory");
        SetActiveUI();
    }

    #endregion

    #region ** Private Methods **
    // 인벤토리에 아이템 추가해보기(임시)
    private void InitTest()
    {
        int testItem01 = 1001;

        for(int i = 0; i<2;i++)
        {
            int id = testItem01 + i;

            WeaponItemData weaponData = DataManager.Instance.GetDataById(id);

            // 업 캐스팅(WeaponItemData => ItemData)
            ItemDataArray[i] = weaponData;

            // 다운 캐스팅
            if (ItemDataArray[i] is WeaponItemData)
            {
                AddItem(ItemDataArray[i]);
            }
        }

        int testItem02 = 2001;
        for (int i = 0; i < 2; i++)
        {
            int id = testItem02 + i;

            PortionItemData portionData = DataManager.Instance.GetPortionDataById(id);

            // 업 캐스팅(WeaponItemData => ItemData)
            ItemDataArray[i+2] = portionData;

            // 다운 캐스팅
            if (ItemDataArray[i+2] is PortionItemData)
            {
                AddItem(ItemDataArray[i+2]);
            }
        }
    }

    // 인벤토리 UI 활성/비활성화
    private void SetActiveUI()
    {
        if(inventoryKeydown)
        {
            if (inventoryGo.activeSelf)
                inventoryGo.SetActive(false);
            else
                inventoryGo.SetActive(true);
        }

    }

    // 인벤토리 앞쪽부터 비어있는 슬롯 인덱스 탐색(성공시 빈슬롯 인덱스 반환, 실패시 -1 반환)
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        // 첫슬롯부터 전체 슬롯 탐색
        for(int i = startIndex; i < Capacity; i++)
        {
            // 빈 슬롯이 있다면 그 슬롯의 인덱스 반환
            if (items[i] == null)
                return i;
        }

        // 빈 슬롯이 없으면 -1 반환
        return -1;
    }

    // 인덱스 슬롯 갱신
    private void UpdateSlot(int index)
    {
        // 유효한 슬롯만
        if (!IsValidIndex(index)) return;

        Item item = items[index];

        // 슬롯에 아이템 존재
        if(item != null)
        {
            // 1. 아이콘 등록
            inventoryUI.SetItemIcon(index, item.Data.ItemIcon);
        }
    }

    // 인덱스 슬롯 갱신(여러 개의 슬롯) Overload
    private void UpdateSlot(params int[] indices)   // index의 복수형
    {
        foreach(var i in indices)
        {
            UpdateSlot(i);
        }
    }

    #endregion

    #region ** Getter & Check Methods **
    // 해당 인덱스의 슬롯이 아이템을 갖고있는지 확인
    public bool HasItem(int index)
    {
        // 유효하고 슬롯에 아이템이 들어있으면 True
        return IsValidIndex(index) && items[index] != null;
    }

    // 유효한 인덱스 번호인지 확인
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }
    #endregion

    // 해당 인덱스의 슬롯 아이템 정보 가져오기
    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (items[index] == null) return null;

        return items[index].Data;
    }

    // 해당 인덱스의 슬롯 아이템 이름 가져오기
    public string GetItemName(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (items[index] == null) return null;

        return items[index].Data.ItemName;
    }

    #region ** Public Methods **
    // 활성화 시킬 슬롯범위 업데이트
    public void UpdateAccessibleSlots()
    {
        inventoryUI.SetAccessibleSlotRange(Capacity);
    }

    // 인벤토리에 아이템 추가(잉여 아이템 갯수 리턴, 리턴이 0이면 모두 성공)
    public int AddItem(ItemData itemData, int amount = 1)
    {
        int index;

        // 1. ItemData가 CountableItem일 경우 => 갯수가 2개~99개까지 가능(아직 미구현)
        
        // 2. 나머지
        // 수량이 하나일 경우
        if(amount == 1)
        {
            // 빈 슬롯을 찾아서 아이템 생성 후 슬롯에 추가
            index = FindEmptySlotIndex();

            // 빈 슬롯이 있다면
            if(index != -1)
            {
                items[index] = itemData.CreateItem();
                amount = 0;

                // 슬롯 갱신
                UpdateSlot(index);
            }
        }

        return amount;
    }

    // 해당 인덱스 슬롯의 아이템 제거
    public void Remove(int index)
    {
        if (!IsValidIndex(index)) return;

        // 인덱스의 아이템 제거
        items[index] = null;

        // 아이콘 및 텍스트 제거
        inventoryUI.RemoveItem(index);
    }

    // 두 슬롯 아이템 스왑
    public void Swap(int beginIndex, int endIndex)
    {
        // 접근불가 슬롯 처리
        if (!IsValidIndex(beginIndex) || !IsValidIndex(endIndex)) return;

        Item itemA = items[beginIndex];
        Item itemB = items[endIndex];

        // 아이템 위치 변경
        items[beginIndex] = itemB;
        items[endIndex] = itemA;

        // 슬롯 갱신
        UpdateSlot(beginIndex, endIndex);
    }
    #endregion
}

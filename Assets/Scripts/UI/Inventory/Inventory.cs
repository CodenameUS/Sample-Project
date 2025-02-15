using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/*
                        Inventory

            - 인벤토리의 실질적인 내부 로직
                - 아이템 추가, 아이템 사용, 아이템 삭제, 아이템 이동
            - 인벤토리내의 빈슬롯 찾기
                - FindEmptySlotIndex => 빈 슬롯의 Index 반환
                - FindCountableItemSlotIndex => 수량이 있는 아이템을 위한 빈 슬롯의 Index 반환

*/

public class Inventory : MonoBehaviour
{
    #region ** Serialized Fields **
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] GameObject inventoryGo;
    [SerializeField] Item[] items;
    [SerializeField] EquipmentUI equipmentUI;
    #endregion

    #region ** Fields **
    public ItemData[] ItemDataArray;
    public int Capacity { get; private set; }   // 인벤토리 수용한도

    private bool inventoryKeydown;              // 인벤토리 키(I)
    private int initCapacity = 24;              // 초기 인벤토리 수용한도
    private int maxCapacity = 36;               // 최대 인벤토리 수용한도

    #endregion

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
        if (Input.GetButtonDown("Inventory"))
        {
            inventoryGo.SetActive(!inventoryGo.activeSelf);
        }
    }

    #endregion

    #region ** Private Methods **
    // 인벤토리에 아이템 추가해보기(임시)
    private void InitTest()
    {
        int testItem01 = 1001;

        // 장비아이템 임시추가
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
        // 포션아이템 임시추가
        for (int i = 0; i < 2; i++)
        {
            int id = testItem02 + i;

            PortionItemData portionData = DataManager.Instance.GetPortionDataById(id);

            ItemDataArray[i + 2] = portionData;

            if (ItemDataArray[i + 2] is PortionItemData)
            {
                AddItem(ItemDataArray[i + 2], 110);
            }
        }

        int testItem03 = 3001;
        // 방어구 아이템 임시추가
        for (int i = 0; i < 2; i++)
        {
            int id = testItem03 + i;

            ArmorItemData armorData = DataManager.Instance.GetArmorDataById(id);
            ItemDataArray[i + 6] = armorData;

            if (ItemDataArray[i + 6] is ArmorItemData)
            {
                AddItem(ItemDataArray[i + 6]);
            }
        }
    }

    // 인벤토리 앞쪽부터 비어있는 슬롯 인덱스 탐색(성공시 빈슬롯 인덱스 반환, 실패시 -1 반환)
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        // 전체 슬롯 탐색
        for(int i = startIndex; i < Capacity; i++)
        {
            // 빈 슬롯이 있다면 그 슬롯의 인덱스 반환
            if (items[i] == null)
                return i;
        }

        // 빈 슬롯이 없으면 -1 반환
        return -1;
    }

    // 인벤토리 앞쪽부터 갯수 여유가 있는 Countable Item 슬롯 인덱스 탐색
    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for(int i = startIndex; i<Capacity;i++)
        {
            var current = items[i];             // 탐색중인 아이템 기억

            // 빈 슬롯일 때
            if (current == null)
                continue;

            // 아이템이 target과 일치
            if(current.Data == target && current is CountableItem ci)
            {
                // 갯수 여유가 있는지 여부
                if (!ci.IsMax)
                    return i;       // 여유가 있으면 인덱스 반환
            }
        }

        // 없으면 -1 반환
        return -1;
    }

    // 해당 인덱스 슬롯 갱신
    private void UpdateSlot(int index)
    {
        // 유효한 슬롯만
        if (!IsValidIndex(index)) return;

        // 해당 아이템
        Item item = items[index];

        // 슬롯에 아이템 존재
        if(item != null)
        {
            // 1. 수량이 있는 아이템인 경우
            if(item is CountableItem ci)
            {
                // 수량이 없을 때 아이템 제거
                if(ci.IsEmpty)
                {
                    items[index] = null;
                    RemoveIcon();
                    return;
                }
                // 아이콘 및 수량 표시
                else
                {
                    inventoryUI.SetItemIconAndAmountText(index, item.Data.ItemIcon, ci.Amount);
                }
            }
            // 2. 장비 아이템
            else if(item is EquipmentItem ei)
            {
                inventoryUI.SetItemIconAndAmountText(index, item.Data.ItemIcon);
            }
            // 3. 그 외
            else
            {
                // 아이콘 표시
                inventoryUI.SetItemIconAndAmountText(index, item.Data.ItemIcon);
            }
        }
        // 슬롯에 아이템이 없을 때
        else
        {
            RemoveIcon();
        }

        // 아이콘 제거 함수
        void RemoveIcon()
        {
            inventoryUI.RemoveItem(index);
            inventoryUI.HideItemAmountText(index);
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

    // 해당 인덱스의 슬롯이 Countable Item 인지 여부
    private bool IsCountableItem(int index)
    {
        return HasItem(index) && items[index] is CountableItem;
    }

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
    #endregion

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

        // 1. ItemData가 CountableItem일 경우 => 갯수 1개~99개까지 가능
        if(itemData is CountableItemData ciData)
        {
            bool findNextCi = true;
            index = -1;

            // 남은 아이템 수량이 없을때까지 반복
            while(amount > 0)
            {
                // 추가할 아이템이 인벤토리에 존재
                if(findNextCi)
                {
                    // 개수 여유가 있는 슬롯 탐색
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    // 없다면
                    if(index == -1)
                    {
                        findNextCi = false;
                    }
                    // 있다면 합치기
                    else
                    {
                        CountableItem ci = items[index] as CountableItem;
                        // 기존에 있던 아이템 갯수에 추가 및 초과량 반환
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                // 추가할 아이템이 인벤토리에 존재하지 않을 때
                else
                {
                    index = FindEmptySlotIndex(index + 1);  // 빈슬롯 찾기

                    // 빈슬롯이 없을 때
                    if(index == -1)
                    {
                        break;
                    }
                    else
                    {
                        // 새로운 아이템 생성
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        // 슬롯에 아이템 추가
                        items[index] = ci;

                        // 남은 갯수 계산
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }
        }
        // 2. 나머지(수량 없는 아이템)
        else
        {
            if (amount == 1)
            {
                // 빈 슬롯을 찾아서 아이템 생성 후 슬롯에 추가
                index = FindEmptySlotIndex();

                // 빈 슬롯이 있다면
                if (index != -1)
                {
                    items[index] = itemData.CreateItem();
                    amount = 0;

                    // 슬롯 갱신
                    UpdateSlot(index);
                }
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

        // (A -> B 슬롯 스왑)
        // 1. Countable Item 이면서 동일한 아이템일 때
        if(itemA != null && itemB != null && itemA.Data == itemB.Data &&
           itemA is CountableItem ciA && itemB is CountableItem ciB && (ciB.Amount < ciB.MaxAmount))
        {
            int maxAmount = ciB.MaxAmount;          // B 슬롯의 최대량
            int sum = ciA.Amount + ciB.Amount;      // 두 아이템 합한 갯수

            // 두 아이템을 합쳐도 최대치보다 크지않을 때
            if(sum <= maxAmount)
            {
                ciA.SetAmount(0);
                ciB.SetAmount(sum);
            }
            else
            {
                ciA.SetAmount(sum - maxAmount);     
                ciB.SetAmount(maxAmount);           
            }
        }
        // 2. 일반적인 경우
        else
        {
            // 아이템 위치 변경
            items[beginIndex] = itemB;
            items[endIndex] = itemA;
        }
        
        // 슬롯 갱신
        UpdateSlot(beginIndex, endIndex);
    }

    // 해당 슬롯 인덱스의 아이템 사용
    public void Use(int index)
    {
        if (!IsValidIndex(index)) return;
        if (items[index] == null) return;

        // 1. 소모 아이템일 때
        if(items[index] is IUsableItem usable)
        {
            // 사용
            bool success = usable.Use();

            // 성공
            if(success)
            {
                UpdateSlot(index);
            }
        }
        // 2. 장비 아이템일 때
        else if(items[index] is IEquipableItem)
        {
            // 2.1. 무기 아이템일때 
            if(items[index] is WeaponItem curWeapon)
            {
                // 장착중인 아이템이 있으면 해제
                if (equipmentUI.slotUIList[0].HasItem)
                {
                    // 장착중인 아이템
                    WeaponItem prevItem = (WeaponItem)equipmentUI.items[0];
                    // 인벤토리에 아이템 추가
                    AddItem(prevItem.Data);
                    // 캐릭터 정보창 슬롯의 아이콘 제거
                    equipmentUI.slotUIList[0].RemoveItemIcon();
                    // 장착 해제
                    prevItem.Unequip();
                }
                curWeapon.Equip();
                equipmentUI.SetItemIcon(curWeapon, curWeapon.WeaponData.Type, curWeapon.Data.ItemIcon);
            }
            // 2.2 방어구 아이템일때
            else if(items[index] is ArmorItem curArmor)
            {
                // 장착중인 아이템이 있으면 해제
                if(equipmentUI.slotUIList[TypeToIndex(curArmor)].HasItem)
                {
                    // 장착중인 아이템
                    ArmorItem prevItem = (ArmorItem)equipmentUI.items[TypeToIndex(curArmor)];
                    // 인벤토리에 아이템 추가
                    AddItem(prevItem.Data);
                    // 캐릭터 정보창 슬롯의 아이콘 제거
                    equipmentUI.slotUIList[TypeToIndex(curArmor)].RemoveItemIcon();
                    // 장착 해제
                    prevItem.Unequip();
                }
                curArmor.Equip();
                equipmentUI.SetItemIcon(curArmor, curArmor.ArmorData.SubType, curArmor.Data.ItemIcon);
            }
            // 방어구 타입별 인덱스
            int TypeToIndex(ArmorItem curItem)
            {
                int typeIndex;
                switch (curItem.ArmorData.SubType)
                {
                    case "Shoes":
                        typeIndex = 1;
                        return typeIndex;
                    case "Gloves":
                        typeIndex = 2;
                        return typeIndex;
                    case "Top":
                        typeIndex = 3;
                        return typeIndex;
                    default:
                        return 0;
                }
            }
        }
            Remove(index);
            UpdateSlot(index); 
    }
    #endregion
}

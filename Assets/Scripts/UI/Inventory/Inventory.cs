using System.Collections.Generic;
using UnityEngine;
using System.IO;

/*
                            << Inventory >>

        - 인벤토리 데이터 로드 및 세이브

        - 인벤토리 실질적인 내부 로직
            - 아이템 추가(Add), 아이템 사용(Use), 아이템 제거(Remove), 아이템 이동(Swap)

        - 아이템 추가
            - AddItem() : 아이템 데이터를 받아 인벤토리 앞쪽 빈자리에 아이템 추가(일반적 사용)
            - AddItemAt() : 인벤토리의 특정 슬롯에 아이템 추가(데이터 로딩시 사용)
            - AddItemAtPlayerItemSlot() : 퀵슬롯에 아이템 추가(퀵슬롯에 아이템 등록시 사용)
        - 아이템 사용
            - Use() : 마우스포인터 위치 슬롯의 아이템 사용, 퀵슬롯의 아이템 사용
                - 장비아이템 : 장비 장착 처리
                - 포션아이템 : 갯수차감 및 사용 처리
        - 아이템 제거
            - Remove() : 제거하고자하는 슬롯의 아이템 제거
        - 아이템 이동
            - Swap() : 마우스 드래그를통해 아이템 이동 또는 두 아이템의 위치 변경
                - 두 아이템 위치변경시 같은 아이템일경우 갯수가 합쳐짐
                    - 드래그를 시작한쪽의 아이템이 우선 제거
                        - 한 슬롯의 최대갯수를 초과할 경우 드래그 시작한쪽에 남는 아이템
 */

[System.Serializable]
public class ItemSlotData
{
    public int slotIndex;
    public int itemId;
    public int amount;
    public bool isAccessibleSlot;
}

[System.Serializable]
public class InventoryDataList
{
    public List<ItemSlotData> itemList;
}

public class Inventory : Singleton<Inventory>
{
    #region ** Serialized Fields **
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject inventoryGo;
    [SerializeField] private Item[] items;
    [SerializeField] private PlayerItemGroupUI playerItemGruopUI;
    [SerializeField] public EquipmentUI equipmentUI;
    #endregion

    #region ** Fields **
    public ItemData[] itemDataArray;
    public int Capacity { get; private set; }   // 인벤토리 수용한도

    private int initCapacity = 24;              // 초기 인벤토리 수용한도
    private int maxCapacity = 36;               // 최대 인벤토리 수용한도

    #endregion

    #region  ** Unity Events **
    protected override void Awake()
    {
        base.Awake();

        // 인벤토리에서 관리할 수 있는 아이템은 최대 36개
        items = new Item[maxCapacity];                  
        itemDataArray = new ItemData[maxCapacity];

        // 초기 수용량 : 24(임시)
        Capacity = initCapacity;
        inventoryUI.SetInventoryRef(this);
    }

    private void Start()
    {
        LoadInventoryData();
        UpdateAccessibleSlots();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            UIManager.Instance.ToggleUI(inventoryGo);
        }
    }

    #endregion

    #region ** Private Methods **
    
    // 인벤토리 데이터 로드
    private void LoadInventoryData()
    {
        string path = Path.Combine(Application.persistentDataPath, "InventoryData.json");

        if(File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            InventoryDataList dataList = JsonUtility.FromJson<InventoryDataList>(jsonData);

            foreach(var data in dataList.itemList)
            {
                itemDataArray[data.slotIndex] = ItemTypeById(data.itemId);

                // 해당 슬롯인덱스에 저장된 아이템이 없을 때
                if (itemDataArray[data.slotIndex] == null)
                {
                    continue;
                }

                // 아이템 생성 후 해당 슬롯에 직접 배치
                Item item = itemDataArray[data.slotIndex].CreateItem();

                if (item is CountableItem ci)
                    ci.SetAmount(data.amount);

                AddItemAt(data.slotIndex, item);
            }
        }
        else
        {
            string defaultPath = Path.Combine(Application.streamingAssetsPath, "Json/InventoryData.json");

            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.Copy(defaultPath, path);

            string jsonData = File.ReadAllText(path);
            InventoryDataList dataList = JsonUtility.FromJson<InventoryDataList>(jsonData);

            foreach (var data in dataList.itemList)
            {
                itemDataArray[data.slotIndex] = ItemTypeById(data.itemId);

                // 해당 슬롯인덱스에 저장된 아이템이 없을 때
                if (itemDataArray[data.slotIndex] == null)
                {
                    continue;
                }

                // 아이템 생성 후 해당 슬롯에 직접 배치
                Item item = itemDataArray[data.slotIndex].CreateItem();

                if (item is CountableItem ci)
                    ci.SetAmount(data.amount);

                AddItemAt(data.slotIndex, item);
            }
        }

        // 아이템 타입별 반환
        ItemData ItemTypeById(int id)
        {
            if (id > 10000 && id < 20000)
            {
                PortionItemData temp = DataManager.Instance.GetPortionDataById(id);
                return temp;
            }
            else if (id > 20000 && id < 30000)
            {
                ArmorItemData temp = DataManager.Instance.GetArmorDataById(id);
                return temp;
            }
            else if (id > 30000 && id < 40000)
            {
                WeaponItemData temp = DataManager.Instance.GetWeaponDataById(id);
                return temp;
            }
            else
                return null;
        }
    }

    // 인벤토리 데이터 저장
    private void SaveInventoryData()
    {
        InventoryDataList saveData = new InventoryDataList();
        saveData.itemList = new List<ItemSlotData>();

        // 모든 슬롯을 돌며
        for(int i = 0; i<items.Length; i++)
        {
            ItemSlotData slotData = new ItemSlotData();
            slotData.slotIndex = i;

            // 아이템이 있는경우(id, amount 저장)
            if(items[i] != null)
            {
                slotData.itemId = items[i].Data.ID;

                if (items[i] is CountableItem ci)
                    slotData.amount = ci.Amount;
                else
                    slotData.amount = 1;
            }
            // 아이템이 없는경우
            else
            {
                slotData.itemId = 0;
                slotData.amount = 0;
            }

            saveData.itemList.Add(slotData);
        }

        string jsonData = JsonUtility.ToJson(saveData, true);
        string path = Path.Combine(Application.persistentDataPath, "InventoryData.json");
        File.WriteAllText(path, jsonData);

        Debug.Log("인벤토리 저장 완료");
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
                inventoryUI.SetItemIconAndAmountText(index, item.Data.ItemIcon, ci.Amount);
                playerItemGruopUI.UpdateSlots();
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
            playerItemGruopUI.UpdateSlots();
        }

        // 인벤토리 데이터 저장
        SaveInventoryData();
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

    // 특정 인벤토리 슬롯에 특정 아이템 추가
    public void AddItemAt(int index, Item item)
    {
        if(index < 0 || index >= maxCapacity)
        {
            Debug.LogWarning("슬롯 인덱스 범위 초과" + index);
            return;
        }

        items[index] = item;

        UpdateSlot(index);
    }

    // 플레이어 아이템 슬롯에 아이템 추가
    public void AddItemAtPlayerItemSlot(int index, PlayerItemSlotUI slot)
    {
        if (items[index] is CountableItem ci)
            playerItemGruopUI.SetItemIconAndAmountText(slot.index, ci);
    }

    // 해당 슬롯의 아이템 제거
    public void Remove(int index)
    {
        if (!IsValidIndex(index)) return;

        if(items[index] is CountableItem)
            playerItemGruopUI.RemoveItem((CountableItem)items[index]);

        // 인덱스의 아이템 제거
        items[index] = null;

        // 아이콘 및 텍스트 제거
        inventoryUI.RemoveItem(index);

        UpdateSlot(index);
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

            // 합쳤을 때 잉여갯수가 남지않을경우
            if(sum <= maxAmount)
            {
                ciA.SetAmount(0);
                Remove(beginIndex);
                ciB.SetAmount(sum);
            }
            // 남는경우
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
        AudioManager.Instance.PlaySFX("InventoryChange");
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
            // 사용에 성공했을 때
            if(usable.Use())
            {
                // 수량이 있는 아이템의 경우
                if(items[index] is CountableItem ci)
                {
                    if(ci.IsEmpty)
                    {
                        Remove(index);      // 수량이 다 떨어졌을경우 제거
                    }
                }
                // 일반 아이템은 바로 제거
                else
                {
                    Remove(index);
                }
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

            Remove(index);
        }
            UpdateSlot(index); 
    }

    // 퀵슬롯의 아이템 사용
    public void Use(CountableItem ci)
    {
        for(int i = 0;i<=items.Length;i++)
        {
            if (items[i] == ci)
            {
                Use(i);
                return;
            }
        }
    }
    #endregion
}

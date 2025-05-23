using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

[System.Serializable]
public class EquipmentSlotData
{
    public int slotIndex;              
    public int itemId;
}

[System.Serializable]
public class EquipmentSlotDataList
{
    public List<EquipmentSlotData> itemList;
}

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private ItemTooltipUI itemTooltipUI;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject targetUI;
    [SerializeField] private UIRaycaster rc;

    // 장비 타입별 idx(0: Weapon, 1: Shoes, 2: Gloves, 3: Top)
    private enum Type { Weapon, Shoes, Gloves, Top}

    #region ** Fields **
    [Tooltip("캐릭터 장비 슬롯")]
    public List<EquipmentSlotUI> slotUIList = new List<EquipmentSlotUI>();
    public Item[] items;
    public ItemData[] itemDataArray;

    private int leftClick = 0;                              // 좌클릭 = 0
    private int rightClick = 1;                             // 우클릭 = 0;
    private int slotCounts = 4;                             // 장비슬롯 수

    private EquipmentSlotUI pointerOverSlot;                // 현재 마우스 포인터가 위치한 곳의 슬롯
    private EquipmentSlotUI beginDragSlot;                  // 마우스 드래그를 시작한 슬롯
    private Transform beginDragIconTransform;               // 마우스 드래그를 시작한 슬롯의 위치

    private Vector3 beginDragIconPoint;                     // 마우스 드래그를 시작한 아이콘 위치
    private Vector3 beginDragCursorPoint;                   // 마우스 드래그를 시작한 커서 위치
    private int beginDragSlotSiblingIndex;                  // 마우스 드래그를 시작한 슬롯의 SiblingIdx
    #endregion  

    #region ** 유니티 이벤트 함수 **
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        LoadEquipmentSlotData();
        HideUI();
    }
    private void Update()
    {
        OnPointerEnterAndExit();
        ShowOrHideTooltipUI();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }
    #endregion

    #region ** Private Methods **
    // 초기화
    private void Init()
    {
        items = new Item[slotCounts];
        itemDataArray = new ItemData[slotCounts];
    }

    // 플레이어가 장착중인 아이템 데이터 로드
    private void LoadEquipmentSlotData()
    {
        string path = Path.Combine(Application.persistentDataPath, "EquipmentSlotData.json");

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            EquipmentSlotDataList dataList = JsonUtility.FromJson<EquipmentSlotDataList>(jsonData);

            foreach (var data in dataList.itemList)
            {
                itemDataArray[data.slotIndex] = ItemTypeById(data.itemId);

                // 해당 슬롯인덱스에 저장된 아이템이 없을 때
                if (itemDataArray[data.slotIndex] == null)
                {
                    // 기본무기로 세팅
                    if(data.slotIndex == 0)
                    {
                        WeaponManager.Instance.SetWeapon();
                    }
                    continue;
                }

                // 슬롯에 아이템 추가
                Item item = itemDataArray[data.slotIndex].CreateItem();

                if(item is WeaponItem wi)
                {
                    SetItemIcon(wi, wi.WeaponData.Type, wi.Data.ItemIcon);
                    WeaponManager.Instance.SetWeapon(wi.WeaponData.SubType, wi.WeaponData.ItemPrefab);
                }
                else if(item is ArmorItem ai)
                {
                    SetItemIcon(ai, ai.ArmorData.SubType, ai.Data.ItemIcon);
                }
            }
        }
        // 아이템 타입별 반환
        ItemData ItemTypeById(int id)
        { 
            if (id > 20000 && id < 30000)
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

    // 장작중인 아이템 데이터 저장
    private void SaveEquipmentSlotData()
    {
        EquipmentSlotDataList saveData = new EquipmentSlotDataList();
        saveData.itemList = new List<EquipmentSlotData>();

        for(int i = 0;i<items.Length;i++)
        {
            EquipmentSlotData slotData = new EquipmentSlotData();
            slotData.slotIndex = i;

            if(items[i] != null)
            {
                slotData.itemId = items[i].Data.ID;
            }
            else
            {
                slotData.itemId = 0;
            }

            saveData.itemList.Add(slotData);
        }

        string jsonData = JsonUtility.ToJson(saveData, true);
        string path = Path.Combine(Application.persistentDataPath, "EquipmentSlotData.json");
        File.WriteAllText(path, jsonData);

        Debug.Log("장착 장비 저장완료");
    }

    private void HideUI() => targetUI.SetActive(false);

    // 툴팁 UI 갱신
    private void UpdateTooltipUI(EquipmentSlotUI slot)
    {
        if (!slot.HasItem)
            return;

        itemTooltipUI.SetItemInfo(items[slot.index].Data);
        itemTooltipUI.SetUIPosition(slot.SlotRect);
    }
    #endregion

    #region ** 마우스 이벤트 함수들 **
    // 마우스 커서가 UI 위에 있는지 여부
    private bool IsOverUI() => EventSystem.current.IsPointerOverGameObject();

    // 마우스 올라갈때 나갈때 처리
    private void OnPointerEnterAndExit()
    {
        // 이전 프레임 슬롯
        var prevSlot = pointerOverSlot;

        // 현재 프레임 슬롯
        var curSlot = pointerOverSlot = rc.RaycastAndgetFirstComponent<EquipmentSlotUI>();

        // 마우스 올라갈 때
        if(prevSlot == null)
        {
            if(curSlot != null)
            {
                OnCurrentEnter();
            }
        }
        // 마우스 나갈 때
        else
        {
            if (curSlot == null)
            {
                OnPrevExit();
            }
            // 다른 슬롯으로 커서 옮길때
            else if (prevSlot != curSlot)
            {
                OnPrevExit();
                OnCurrentEnter();
            }
        }

        void OnCurrentEnter()
        {
            curSlot.Highlight(true);
        }

        void OnPrevExit()
        {
            prevSlot.Highlight(false);
        }
    }

    // 마우스 눌렀을 때 처리
    private void OnPointerDown()
    {
        // 마우스 좌클릭(Holding)
        if (Input.GetMouseButtonDown(leftClick))
        {
            // 시작 슬롯
            beginDragSlot = rc.RaycastAndgetFirstComponent<EquipmentSlotUI>();

            // 슬롯에 아이템이 있을 때
            if(beginDragSlot != null && beginDragSlot.HasItem)
            {
                // 드래그 위치, 참조
                beginDragIconTransform = beginDragSlot.IconRect.transform;
                beginDragIconPoint = beginDragIconTransform.position;
                beginDragCursorPoint = Input.mousePosition;

                beginDragSlotSiblingIndex = beginDragSlot.transform.GetSiblingIndex();
                beginDragSlot.transform.SetAsLastSibling();     // 가장 위에 표시

                beginDragSlot.SetHighlightOnTop(false);
            }
            else
            {
                beginDragSlot = null;
            }
        }
        // 마우스 우클릭
        else if (Input.GetMouseButtonDown(rightClick))
        {
            // 우클릭 위치의 슬롯
            EquipmentSlotUI slotUI = rc.RaycastAndgetFirstComponent<EquipmentSlotUI>();
            
            // 장비 장착 해제
            if(slotUI != null && slotUI.HasItem)
            {
                EquipmentItem item = (EquipmentItem)items[slotUI.index];
                inventory.AddItem(item.Data);
                item.Unequip();

                // 아이템제거
                items[slotUI.index] = null;
                itemDataArray[slotUI.index] = null;
                slotUIList[slotUI.index].RemoveItemIcon();

                // 저장
                SaveEquipmentSlotData();
            }
        }
    }

    // 마우스 드래그중일 때 처리
    private void OnPointerDrag()
    {
        // 드래그중이 아닐때
        if (beginDragSlot == null) return;

        if(Input.GetMouseButton(leftClick))
        {
            // 슬롯 아이콘 위치 업데이트
            beginDragIconTransform.position = beginDragIconPoint + (Input.mousePosition - beginDragCursorPoint);
        }
    }

    // 마우스 뗐을 때 처리
    private void OnPointerUp()
    {
        if(Input.GetMouseButtonUp(leftClick))
        {
            // 복원
            if(beginDragSlot != null)
            {
                // 위치 복원
                beginDragIconTransform.position = beginDragIconPoint;

                // UI 순서 복원
                beginDragSlot.transform.SetSiblingIndex(beginDragSlotSiblingIndex);

                // 하이라이트 이미지를 아이콘보다 앞에
                beginDragSlot.SetHighlightOnTop(true);

                // 참조제거
                beginDragSlot = null;
                beginDragIconTransform = null;
            }
        }
    }

    // 아이템 툴팁 UI 활성/비활성화
    private void ShowOrHideTooltipUI()
    {
        // 마우스가 아이템 아이콘 위에 올라가있을 때 툴팁표시
        bool isValid = pointerOverSlot != null && pointerOverSlot.HasItem && (pointerOverSlot != beginDragSlot);

        if (isValid)
        {
            UpdateTooltipUI(pointerOverSlot);
            itemTooltipUI.ShowTooltipUI();
        }
        else
            itemTooltipUI.HideTooltipUI();
    }
    #endregion

    // 아이템 아이콘
    public void SetItemIcon(Item item, string type, string icon)
    {
        // 아이템 타입에 따른 index
        if (Enum.TryParse(type, out Type result))
        {
            int index = (int)result;

            // 슬롯 아이템 저장
            items[index] = item;

            // 아이콘 등록
            slotUIList[index].SetItemIcon(icon);
        }
        else
        {
            Debug.LogError($"'{type}'은(는) 유효한 타입이 아닙니다.");
        }

        SaveEquipmentSlotData();
    }


}

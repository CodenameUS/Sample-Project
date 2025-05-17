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

    // ��� Ÿ�Ժ� idx(0: Weapon, 1: Shoes, 2: Gloves, 3: Top)
    private enum Type { Weapon, Shoes, Gloves, Top}

    #region ** Fields **
    [Tooltip("ĳ���� ��� ����")]
    public List<EquipmentSlotUI> slotUIList = new List<EquipmentSlotUI>();
    public Item[] items;
    public ItemData[] itemDataArray;

    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;

    private int leftClick = 0;                              // ��Ŭ�� = 0
    private int rightClick = 1;                             // ��Ŭ�� = 0;
    private int slotCounts = 4;                             // ��񽽷� ��

    private EquipmentSlotUI pointerOverSlot;                // ���� ���콺 �����Ͱ� ��ġ�� ���� ����
    private EquipmentSlotUI beginDragSlot;                  // ���콺 �巡�׸� ������ ����
    private Transform beginDragIconTransform;               // ���콺 �巡�׸� ������ ������ ��ġ

    private Vector3 beginDragIconPoint;                     // ���콺 �巡�׸� ������ ������ ��ġ
    private Vector3 beginDragCursorPoint;                   // ���콺 �巡�׸� ������ Ŀ�� ��ġ
    private int beginDragSlotSiblingIndex;                  // ���콺 �巡�׸� ������ ������ SiblingIdx
    #endregion  

    #region ** ����Ƽ �̺�Ʈ �Լ� **
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
        ped.position = Input.mousePosition;

        OnPointerEnterAndExit();
        ShowOrHideTooltipUI();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }
    #endregion

    #region ** Private Methods **
    // �ʱ�ȭ
    private void Init()
    {
        items = new Item[slotCounts];
        itemDataArray = new ItemData[slotCounts];

        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);
    }

    // �÷��̾ �������� ������ ������ �ε�
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

                // �ش� �����ε����� ����� �������� ���� ��
                if (itemDataArray[data.slotIndex] == null)
                {
                    // �⺻����� ����
                    if(data.slotIndex == 0)
                    {
                        WeaponManager.Instance.SetWeapon();
                    }
                    continue;
                }

                // ���Կ� ������ �߰�
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
        // ������ Ÿ�Ժ� ��ȯ
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

    // �������� ������ ������ ����
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

        Debug.Log("���� ��� ����Ϸ�");
    }

    private void HideUI() => targetUI.SetActive(false);

    // ���� UI ����
    private void UpdateTooltipUI(EquipmentSlotUI slot)
    {
        if (!slot.HasItem)
            return;

        itemTooltipUI.SetItemInfo(items[slot.index].Data);
        itemTooltipUI.SetUIPosition(slot.SlotRect);
    }
    #endregion

    #region ** ���콺 �̺�Ʈ �Լ��� **
    // ���콺 Ŀ���� UI ���� �ִ��� ����
    private bool IsOverUI() => EventSystem.current.IsPointerOverGameObject();

    // ����ĳ������ ù UI����� ������Ʈ�� ��������
    private T RaycastAndgetFirstComponent<T>() where T : Component
    {
        // RaycastResult �ʱ�ȭ
        rrList.Clear();

        // ���� ���콺 ��ġ���� ������ UI��� ����
        gr.Raycast(ped, rrList);

        // ������
        if (rrList.Count == 0)
            return null;

        // ù��° UI�� ������Ʈ ��ȯ
        return rrList[0].gameObject.GetComponent<T>();
    }

    // ���콺 �ö󰥶� ������ ó��
    private void OnPointerEnterAndExit()
    {
        // ���� ������ ����
        var prevSlot = pointerOverSlot;

        // ���� ������ ����
        var curSlot = pointerOverSlot = RaycastAndgetFirstComponent<EquipmentSlotUI>();

        // ���콺 �ö� ��
        if(prevSlot == null)
        {
            if(curSlot != null)
            {
                OnCurrentEnter();
            }
        }
        // ���콺 ���� ��
        else
        {
            if (curSlot == null)
            {
                OnPrevExit();
            }
            // �ٸ� �������� Ŀ�� �ű涧
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

    // ���콺 ������ �� ó��
    private void OnPointerDown()
    {
        // ���콺 ��Ŭ��(Holding)
        if (Input.GetMouseButtonDown(leftClick))
        {
            // ���� ����
            beginDragSlot = RaycastAndgetFirstComponent<EquipmentSlotUI>();

            // ���Կ� �������� ���� ��
            if(beginDragSlot != null && beginDragSlot.HasItem)
            {
                // �巡�� ��ġ, ����
                beginDragIconTransform = beginDragSlot.IconRect.transform;
                beginDragIconPoint = beginDragIconTransform.position;
                beginDragCursorPoint = Input.mousePosition;

                beginDragSlotSiblingIndex = beginDragSlot.transform.GetSiblingIndex();
                beginDragSlot.transform.SetAsLastSibling();     // ���� ���� ǥ��

                beginDragSlot.SetHighlightOnTop(false);
            }
            else
            {
                beginDragSlot = null;
            }
        }
        // ���콺 ��Ŭ��
        else if (Input.GetMouseButtonDown(rightClick))
        {
            // ��Ŭ�� ��ġ�� ����
            EquipmentSlotUI slotUI = RaycastAndgetFirstComponent<EquipmentSlotUI>();
            
            // ��� ���� ����
            if(slotUI != null && slotUI.HasItem)
            {
                EquipmentItem item = (EquipmentItem)items[slotUI.index];
                inventory.AddItem(item.Data);
                item.Unequip();

                // ����������
                items[slotUI.index] = null;
                itemDataArray[slotUI.index] = null;
                slotUIList[slotUI.index].RemoveItemIcon();

                // ����
                SaveEquipmentSlotData();
            }
        }
    }

    // ���콺 �巡������ �� ó��
    private void OnPointerDrag()
    {
        // �巡������ �ƴҶ�
        if (beginDragSlot == null) return;

        if(Input.GetMouseButton(leftClick))
        {
            // ���� ������ ��ġ ������Ʈ
            beginDragIconTransform.position = beginDragIconPoint + (Input.mousePosition - beginDragCursorPoint);
        }
    }

    // ���콺 ���� �� ó��
    private void OnPointerUp()
    {
        if(Input.GetMouseButtonUp(leftClick))
        {
            // ����
            if(beginDragSlot != null)
            {
                // ��ġ ����
                beginDragIconTransform.position = beginDragIconPoint;

                // UI ���� ����
                beginDragSlot.transform.SetSiblingIndex(beginDragSlotSiblingIndex);

                // ���̶���Ʈ �̹����� �����ܺ��� �տ�
                beginDragSlot.SetHighlightOnTop(true);

                // ��������
                beginDragSlot = null;
                beginDragIconTransform = null;
            }
        }
    }

    // ������ ���� UI Ȱ��/��Ȱ��ȭ
    private void ShowOrHideTooltipUI()
    {
        // ���콺�� ������ ������ ���� �ö����� �� ����ǥ��
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

    // ������ ������
    public void SetItemIcon(Item item, string type, string icon)
    {
        // ������ Ÿ�Կ� ���� index
        if (Enum.TryParse(type, out Type result))
        {
            int index = (int)result;

            // ���� ������ ����
            items[index] = item;

            // ������ ���
            slotUIList[index].SetItemIcon(icon);
        }
        else
        {
            Debug.LogError($"'{type}'��(��) ��ȿ�� Ÿ���� �ƴմϴ�.");
        }

        SaveEquipmentSlotData();
    }


}

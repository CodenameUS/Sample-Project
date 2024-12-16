using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ItemData[] ItemDataArray;
    public int Capacity { get; private set; }   // �κ��丮 �����ѵ�

    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] GameObject inventoryGo;
    [SerializeField] Item[] items;

    private bool inventoryKeydown;              // �κ��丮 Ű(I)
    private int initCapacity = 24;              // �ʱ� �κ��丮 �����ѵ�
    private int maxCapacity = 36;               // �ִ� �κ��丮 �����ѵ�

    Sprite itemSprite;

    #region  ** Unity Events **
    private void Awake()
    {
        // �κ��丮���� ������ �� �ִ� �������� �ִ� 36��
        items = new Item[maxCapacity];                  
        ItemDataArray = new ItemData[maxCapacity];

        // �ʱ� ���뷮 : 24(�ӽ�)
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
    // �κ��丮�� ������ �߰��غ���(�ӽ�)
    private void InitTest()
    {
        int testItem01 = 1001;

        for(int i = 0; i<2;i++)
        {
            int id = testItem01 + i;

            WeaponItemData weaponData = DataManager.Instance.GetDataById(id);

            // �� ĳ����(WeaponItemData => ItemData)
            ItemDataArray[i] = weaponData;

            // �ٿ� ĳ����
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

            // �� ĳ����(WeaponItemData => ItemData)
            ItemDataArray[i+2] = portionData;

            // �ٿ� ĳ����
            if (ItemDataArray[i+2] is PortionItemData)
            {
                AddItem(ItemDataArray[i+2]);
            }
        }
    }

    // �κ��丮 UI Ȱ��/��Ȱ��ȭ
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

    // �κ��丮 ���ʺ��� ����ִ� ���� �ε��� Ž��(������ �󽽷� �ε��� ��ȯ, ���н� -1 ��ȯ)
    private int FindEmptySlotIndex(int startIndex = 0)
    {
        // ù���Ժ��� ��ü ���� Ž��
        for(int i = startIndex; i < Capacity; i++)
        {
            // �� ������ �ִٸ� �� ������ �ε��� ��ȯ
            if (items[i] == null)
                return i;
        }

        // �� ������ ������ -1 ��ȯ
        return -1;
    }

    // �ε��� ���� ����
    private void UpdateSlot(int index)
    {
        // ��ȿ�� ���Ը�
        if (!IsValidIndex(index)) return;

        Item item = items[index];

        // ���Կ� ������ ����
        if(item != null)
        {
            // 1. ������ ���
            inventoryUI.SetItemIcon(index, item.Data.ItemIcon);
        }
    }

    // �ε��� ���� ����(���� ���� ����) Overload
    private void UpdateSlot(params int[] indices)   // index�� ������
    {
        foreach(var i in indices)
        {
            UpdateSlot(i);
        }
    }

    #endregion

    #region ** Getter & Check Methods **
    // �ش� �ε����� ������ �������� �����ִ��� Ȯ��
    public bool HasItem(int index)
    {
        // ��ȿ�ϰ� ���Կ� �������� ��������� True
        return IsValidIndex(index) && items[index] != null;
    }

    // ��ȿ�� �ε��� ��ȣ���� Ȯ��
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }
    #endregion

    // �ش� �ε����� ���� ������ ���� ��������
    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (items[index] == null) return null;

        return items[index].Data;
    }

    // �ش� �ε����� ���� ������ �̸� ��������
    public string GetItemName(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (items[index] == null) return null;

        return items[index].Data.ItemName;
    }

    #region ** Public Methods **
    // Ȱ��ȭ ��ų ���Թ��� ������Ʈ
    public void UpdateAccessibleSlots()
    {
        inventoryUI.SetAccessibleSlotRange(Capacity);
    }

    // �κ��丮�� ������ �߰�(�׿� ������ ���� ����, ������ 0�̸� ��� ����)
    public int AddItem(ItemData itemData, int amount = 1)
    {
        int index;

        // 1. ItemData�� CountableItem�� ��� => ������ 2��~99������ ����(���� �̱���)
        
        // 2. ������
        // ������ �ϳ��� ���
        if(amount == 1)
        {
            // �� ������ ã�Ƽ� ������ ���� �� ���Կ� �߰�
            index = FindEmptySlotIndex();

            // �� ������ �ִٸ�
            if(index != -1)
            {
                items[index] = itemData.CreateItem();
                amount = 0;

                // ���� ����
                UpdateSlot(index);
            }
        }

        return amount;
    }

    // �ش� �ε��� ������ ������ ����
    public void Remove(int index)
    {
        if (!IsValidIndex(index)) return;

        // �ε����� ������ ����
        items[index] = null;

        // ������ �� �ؽ�Ʈ ����
        inventoryUI.RemoveItem(index);
    }

    // �� ���� ������ ����
    public void Swap(int beginIndex, int endIndex)
    {
        // ���ٺҰ� ���� ó��
        if (!IsValidIndex(beginIndex) || !IsValidIndex(endIndex)) return;

        Item itemA = items[beginIndex];
        Item itemB = items[endIndex];

        // ������ ��ġ ����
        items[beginIndex] = itemB;
        items[endIndex] = itemA;

        // ���� ����
        UpdateSlot(beginIndex, endIndex);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    // �κ��丮�� ������ �߰��غ���(�ӽ�)
    private void InitTest()
    {
        // �⺻ �� ID
        int testItem01 = 1001;

        // �⺻ �� ������ ��������
        WeaponItemData weaponData = DataManager.Instance.GetDataById(testItem01);

        // �� ĳ����(WeaponItemData => ItemData)
        ItemDataArray[0] = weaponData;

        // �ٿ� ĳ����
        if (ItemDataArray[0] is WeaponItemData)
            AddItem(ItemDataArray[0]);

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

    // ��ȿ�� �ε��� ��ȣ���� Ȯ��
    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
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

    // �ش� �ε����� ������ �������� �����ִ��� Ȯ��
    public bool HasItem(int index)
    {
        // ��ȿ�ϰ� ���Կ� �������� ��������� True
        return IsValidIndex(index) && items[index] != null;
    }

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

                Debug.Log(items[index].Data.ItemIcon);
                // ���� ����
                UpdateSlot(index);
            }
        }

        return amount;
    }
}

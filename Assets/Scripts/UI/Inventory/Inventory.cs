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

        // �������� �ӽ��߰�
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
        // ���Ǿ����� �ӽ��߰�
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
        // ��ü ���� Ž��
        for(int i = startIndex; i < Capacity; i++)
        {
            // �� ������ �ִٸ� �� ������ �ε��� ��ȯ
            if (items[i] == null)
                return i;
        }

        // �� ������ ������ -1 ��ȯ
        return -1;
    }

    // �κ��丮 ���ʺ��� ���� ������ �ִ� Countable Item ���� �ε��� Ž��
    private int FindCountableItemSlotIndex(CountableItemData target, int startIndex = 0)
    {
        for(int i = startIndex; i<Capacity;i++)
        {
            var current = items[i];             // Ž������ ������ ���

            // �� ������ ��
            if (current == null)
                continue;

            // �������� target�� ��ġ
            if(current.Data == target && current is CountableItem ci)
            {
                // ���� ������ �ִ��� ����
                if (!ci.IsMax)
                    return i;       // ������ ������ �ε��� ��ȯ
            }
        }

        // ������ -1 ��ȯ
        return -1;
    }

    // �ش� �ε��� ���� ����
    private void UpdateSlot(int index)
    {
        // ��ȿ�� ���Ը�
        if (!IsValidIndex(index)) return;

        // �ش� ������
        Item item = items[index];

        // ���Կ� ������ ����
        if(item != null)
        {
            // 1. ������ �ִ� �������� ���
            if(item is CountableItem ci)
            {
                // ������ ���� �� ������ ����
                if(ci.IsEmpty)
                {
                    items[index] = null;
                    RemoveIcon();
                    return;
                }
                // ������ �� ���� ǥ��
                else
                {
                    inventoryUI.SetItemIconAndAmountText(index, item.Data.ItemIcon, ci.Amount);
                }
            }
            // 2. ������ ���� �������� ���
            else
            {
                // ������ ǥ��
                inventoryUI.SetItemIconAndAmountText(index, item.Data.ItemIcon);
            }
        }
        // ���Կ� �������� ���� ��
        else
        {
            RemoveIcon();
        }

        // ������ ���� �Լ�
        void RemoveIcon()
        {
            inventoryUI.RemoveItem(index);
            inventoryUI.HideItemAmountText(index);
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

    // �ش� �ε����� ������ Countable Item ���� ����
    private bool IsCountableItem(int index)
    {
        return HasItem(index) && items[index] is CountableItem;
    }

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
    #endregion

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

        // 1. ItemData�� CountableItem�� ��� => ���� 1��~99������ ����
        if(itemData is CountableItemData ciData)
        {
            bool findNextCi = true;
            index = -1;

            // ���� ������ ������ ���������� �ݺ�
            while(amount > 0)
            {
                // �߰��� �������� �κ��丮�� ����
                if(findNextCi)
                {
                    // ���� ������ �ִ� ���� Ž��
                    index = FindCountableItemSlotIndex(ciData, index + 1);

                    // ���ٸ�
                    if(index == -1)
                    {
                        findNextCi = false;
                    }
                    // �ִٸ� ��ġ��
                    else
                    {
                        CountableItem ci = items[index] as CountableItem;
                        // ������ �ִ� ������ ������ �߰� �� �ʰ��� ��ȯ
                        amount = ci.AddAmountAndGetExcess(amount);

                        UpdateSlot(index);
                    }
                }
                // �߰��� �������� �κ��丮�� �������� ���� ��
                else
                {
                    index = FindEmptySlotIndex(index + 1);  // �󽽷� ã��

                    // �󽽷��� ���� ��
                    if(index == -1)
                    {
                        break;
                    }
                    else
                    {
                        // ���ο� ������ ����
                        CountableItem ci = ciData.CreateItem() as CountableItem;
                        ci.SetAmount(amount);

                        // ���Կ� ������ �߰�
                        items[index] = ci;

                        // ���� ���� ���
                        amount = (amount > ciData.MaxAmount) ? (amount - ciData.MaxAmount) : 0;

                        UpdateSlot(index);
                    }
                }
            }
        }
        // 2. ������(���� ���� ������)
        else
        {
            if (amount == 1)
            {
                // �� ������ ã�Ƽ� ������ ���� �� ���Կ� �߰�
                index = FindEmptySlotIndex();

                // �� ������ �ִٸ�
                if (index != -1)
                {
                    items[index] = itemData.CreateItem();
                    amount = 0;

                    // ���� ����
                    UpdateSlot(index);
                }
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

        // (A -> B ���� ����)
        // 1. Countable Item �̸鼭 ������ �������� ��
        if(itemA != null && itemB != null && itemA.Data == itemB.Data &&
           itemA is CountableItem ciA && itemB is CountableItem ciB && (ciB.Amount < ciB.MaxAmount))
        {
            int maxAmount = ciB.MaxAmount;          // B ������ �ִ뷮
            int sum = ciA.Amount + ciB.Amount;      // �� ������ ���� ����

            // �� �������� ���ĵ� �ִ�ġ���� ũ������ ��
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
        // 2. �Ϲ����� ���
        else
        {
            // ������ ��ġ ����
            items[beginIndex] = itemB;
            items[endIndex] = itemA;
        }
        
        // ���� ����
        UpdateSlot(beginIndex, endIndex);
    }

    // �ش� ���� �ε����� ������ ���
    public void Use(int index)
    {
        if (!IsValidIndex(index)) return;
        if (items[index] == null) return;

        // ��� ������ �������� ��
        if(items[index] is IUsableItem usable)
        {
            // ���
            bool success = usable.Use();

            // ����
            if(success)
            {
                UpdateSlot(index);
            }
        }
    }

    // �ش� ���� �ε����� ������ ����
    public void Equip(int index)
    {
        if (!IsValidIndex(index)) return;
        if (items[index] == null) return;

        // ���� ������ �������� ��
        if(items[index] is IEquipableItem equipable)
        {
            // ����
            bool success = equipable.Equip();

            // ����
            if(success)
            {
                UpdateSlot(index);
            }
        }
    }
    #endregion
}

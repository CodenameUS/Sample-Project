using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemData[] ItemDataArraty;           // �κ��丮�� ������ ������ �迭

    public int Capacity { get; private set; }   // �κ��丮 �����ѵ�

    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] GameObject inventoryGo;

    private bool inventoryKeydown;              // �κ��丮 Ű(I)
    private int initCapacity = 24;              // �ʱ� �κ��丮 �����ѵ�
    private int maxCapacity = 36;               // �ִ� �κ��丮 �����ѵ�

    private void Awake()
    {
        Capacity = initCapacity;
        inventoryUI.SetInventoryRef(this);
    }

    private void Start()
    {
        UpdateAccessibleSlots();
    }

    private void Update()
    {
        inventoryKeydown = Input.GetButtonDown("Inventory");
        SetActiveUI();
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

    // Ȱ��ȭ ��ų ���Թ��� ������Ʈ
    public void UpdateAccessibleSlots()
    {
        inventoryUI.SetAccessibleSlotRange(Capacity);
    }
}

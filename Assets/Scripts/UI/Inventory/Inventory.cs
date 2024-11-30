using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemData[] ItemDataArraty;           // 인벤토리의 아이템 데이터 배열

    public int Capacity { get; private set; }   // 인벤토리 수용한도

    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] GameObject inventoryGo;

    private bool inventoryKeydown;              // 인벤토리 키(I)
    private int initCapacity = 24;              // 초기 인벤토리 수용한도
    private int maxCapacity = 36;               // 최대 인벤토리 수용한도

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

    // 활성화 시킬 슬롯범위 업데이트
    public void UpdateAccessibleSlots()
    {
        inventoryUI.SetAccessibleSlotRange(Capacity);
    }
}

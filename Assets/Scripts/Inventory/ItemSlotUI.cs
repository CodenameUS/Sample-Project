using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotUI : MonoBehaviour
{
    public int Index { get; private set; }              // .. ���� �ε���

    // ���� �ε��� ����
    public void SetSlotIndex(int index) => Index = index;


}

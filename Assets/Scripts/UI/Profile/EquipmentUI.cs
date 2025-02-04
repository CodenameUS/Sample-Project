using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentUI : MonoBehaviour
{
    // ��� Ÿ�Ժ� idx(0: Weapon, 1: Shoes, 2: Gloves, 3: Top)
    private enum Type { Weapon, Shoes, Gloves, Top}

    [Tooltip("ĳ���� ��� ����")]
    public List<EquipmentSlotUI> slotUIList = new List<EquipmentSlotUI>();
    public Item[] items;

    #region ** Fields **
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;

    private int leftClick = 0;                              // ��Ŭ�� = 0
    private int rightClick = 1;                             // ��Ŭ�� = 0;
    private int slotCounts = 4;

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

    private void Update()
    {
        ped.position = Input.mousePosition;

        OnPointerEnterAndExit();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
        EndDrag();
    }
    #endregion

    #region ** Private Methods **
    // �ʱ�ȭ
    private void Init()
    {
        items = new Item[slotCounts];

        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);
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
        // ���콺 ��Ŭ��(���� ����)
        else if (Input.GetMouseButtonDown(rightClick))
        {
            // ���Կ��� ������ ����
            beginDragSlot = null;
            
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

                // �巡�� �Ϸ�
                EndDrag();

                // ���̶���Ʈ �̹����� �����ܺ��� �տ�
                beginDragSlot.SetHighlightOnTop(true);

                // ��������
                beginDragSlot = null;
                beginDragIconTransform = null;
            }
        }
    }

    // ���콺 �巡�� ���� ó��
    private void EndDrag()
    {
        
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
    }

}

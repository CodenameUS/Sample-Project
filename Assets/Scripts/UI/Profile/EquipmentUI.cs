using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentUI : MonoBehaviour
{
    [Tooltip("ĳ���� ��� ����")]
    public List<EquipmentSlotUI> slotUIList = new List<EquipmentSlotUI>();

    #region ** Fields **
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;

    private EquipmentSlotUI pointerOverSlot;            // ���� ���콺 �����Ͱ� ��ġ�� ���� ����
    private EquipmentSlotUI beginDragSlot;              // ���콺 �巡�׸� ������ ����
    private Transform beginDragIconTransform;           // ���콺 �巡�׸� ������ ������ ��ġ
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
    }
    #endregion

    #region ** Private Methods **
    // �ʱ�ȭ
    private void Init()
    {
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

    #endregion

}

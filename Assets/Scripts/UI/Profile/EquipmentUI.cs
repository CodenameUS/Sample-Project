using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentUI : MonoBehaviour
{
    [Tooltip("캐릭터 장비 슬롯")]
    public List<EquipmentSlotUI> slotUIList = new List<EquipmentSlotUI>();

    #region ** Fields **
    private GraphicRaycaster gr;
    private PointerEventData ped;
    private List<RaycastResult> rrList;

    private EquipmentSlotUI pointerOverSlot;            // 현재 마우스 포인터가 위치한 곳의 슬롯
    private EquipmentSlotUI beginDragSlot;              // 마우스 드래그를 시작한 슬롯
    private Transform beginDragIconTransform;           // 마우스 드래그를 시작한 슬롯의 위치
    #endregion

    #region ** 유니티 이벤트 함수 **
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
    // 초기화
    private void Init()
    {
        TryGetComponent(out gr);
        if (gr == null)
            gr = gameObject.AddComponent<GraphicRaycaster>();

        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>(10);
    }
    #endregion

    #region ** 마우스 이벤트 함수들 **
    // 마우스 커서가 UI 위에 있는지 여부
    private bool IsOverUI() => EventSystem.current.IsPointerOverGameObject();

    // 레이캐스팅한 첫 UI요소의 컴포넌트를 가져오기
    private T RaycastAndgetFirstComponent<T>() where T : Component
    {
        // RaycastResult 초기화
        rrList.Clear();

        // 현재 마우스 위치에서 감지된 UI요소 저장
        gr.Raycast(ped, rrList);

        // 없으면
        if (rrList.Count == 0)
            return null;

        // 첫번째 UI의 컴포넌트 반환
        return rrList[0].gameObject.GetComponent<T>();
    }

    // 마우스 올라갈때 나갈때 처리
    private void OnPointerEnterAndExit()
    {
        // 이전 프레임 슬롯
        var prevSlot = pointerOverSlot;

        // 현재 프레임 슬롯
        var curSlot = pointerOverSlot = RaycastAndgetFirstComponent<EquipmentSlotUI>();

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

    #endregion

}

using UnityEngine.EventSystems;
using UnityEngine;

/*
                            << MovableHeaderUI >>

        - UI의 헤더부분을 드래그하여 움직일 수 있도록 하는 클래스

        - IDragHandler, IPointerDownHandler 구현
 */

public class MovableHeaderUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private Transform targetUI;    // 이동될 UI

    private Vector2 beginPoint;                     // UI 초기 위치
    private Vector2 moveBegin;                      // 드래그 시작 마우스 위치


    private void Awake()
    {
        if (targetUI == null)
            targetUI = transform.parent;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        // UI 위치 업데이트
        targetUI.position = beginPoint + (eventData.position - moveBegin);
    }


    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        // 마우스 클릭시 위치 저장
        beginPoint = targetUI.position;
        moveBegin = eventData.position;
    }

    // 타겟 UI 비활성화
    public void HideUI()
    {
        UIManager.Instance.CloseUI(targetUI.gameObject);
    }
}
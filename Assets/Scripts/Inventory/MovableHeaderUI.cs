using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 인벤토리 드래그 앤 드롭 UI 이동/// </summary>
public class MovableHeaderUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    // 이동될 UI
    [SerializeField] private Transform targetUI;

    private Vector2 beginPoint;
    private Vector2 moveBegin;

    private void Awake()
    {
        // 지정된 타깃UI가 없을경우 부모로 초기화
        if (targetUI == null)
            targetUI = transform.parent;
    }

    // 드래그하기
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        targetUI.position = beginPoint + (eventData.position - moveBegin);    
    }

    // 드래그 위치
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        beginPoint = targetUI.position;
        moveBegin = eventData.position;
    }

    // 인벤토리 닫기
    public void HideUI()
    {
        targetUI.gameObject.SetActive(false);
    }
}

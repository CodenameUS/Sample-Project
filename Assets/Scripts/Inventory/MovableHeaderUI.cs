using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

/// <summary> �κ��丮 �巡�� �� ��� UI �̵�/// </summary>
public class MovableHeaderUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    // �̵��� UI
    [SerializeField] private Transform targetUI;

    private Vector2 beginPoint;
    private Vector2 moveBegin;

    private void Awake()
    {
        // ������ Ÿ��UI�� ������� �θ�� �ʱ�ȭ
        if (targetUI == null)
            targetUI = transform.parent;
    }

    // �巡���ϱ�
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        targetUI.position = beginPoint + (eventData.position - moveBegin);    
    }

    // �巡�� ��ġ
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        beginPoint = targetUI.position;
        moveBegin = eventData.position;
    }

    // �κ��丮 �ݱ�
    public void HideUI()
    {
        targetUI.gameObject.SetActive(false);
    }
}

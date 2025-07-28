using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

/*
                            << UIRaycaster >>

        - 사용자의 마우스 위치에있는 UI 요소를 Raycast하여 특정 컴포넌트를 가진 UI 오브젝트를 반환
            - 다양한 컴포넌트를 반환할 수 있도록 제너릭으로 구현
 */

public class UIRaycaster : MonoBehaviour
{
    private GraphicRaycaster gr;                
    private PointerEventData ped;               
    private List<RaycastResult> rrList;                 // Raycast 결과 저장 리스트

    private void Awake()
    {
        gr = GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(EventSystem.current);
        rrList = new List<RaycastResult>();
    }

    private void Update()
    {
        ped.position = Input.mousePosition;
    }

    // 마우스위치의 UI중 특정 컴포넌트(T)를 가지고있는 첫번째 오브젝트 컴포넌트(T) 반환
    public T RaycastAndgetFirstComponent<T>() where T : Component
    {
        // 리스트 초기화
        rrList.Clear();

        // 현재 마우스 위치에서 감지된 UI요소 저장
        gr.Raycast(ped, rrList);

        // 없으면 null
        if (rrList.Count == 0)
            return null;

        for(int i = 0; i< rrList.Count; i++)
        {
            T component = rrList[i].gameObject.GetComponent<T>();
            // 찾는 컴포넌트가 있으면 반환
            if(component != null)
            {
                return component;
            }
        }

        return null;
    }
}

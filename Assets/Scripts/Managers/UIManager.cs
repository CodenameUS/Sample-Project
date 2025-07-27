using System.Collections.Generic;
using UnityEngine;


/*
                            << UIManager >>

        - UI의 활성/비활성화 처리(싱글톤)

        - Esc 키입력을통해 가장 최근에 열린 UI을 차례로 비활성화 가능
            - 열려있는 UI가 없을경우 설정 UI가 활성화
 */
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject menuUI;             // 환경설정 UI

    private Stack<GameObject> uiStack = new();
    
    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(IsAnyUIOpen())
            {
                CloseTopUI();
            }
            else
            {
                ToggleMenuUI();
            }
        }
    }

    // UI 열기
    public void OpenUI(GameObject ui)
    {
        if (ui.activeSelf) return;

        ui.SetActive(true);
        uiStack.Push(ui);
    }

    // UI 닫기
    public void CloseUI(GameObject ui)
    {
        if (!ui.activeSelf) return;

        ui.SetActive(false);
        if(uiStack.Contains(ui))
        {
            // 스택에서 해당 UI 제거
            Stack<GameObject> tempStack = new();
            while(uiStack.Count > 0)
            {
                GameObject top = uiStack.Pop();
                if (top == ui) break;
                tempStack.Push(top);
            }
            while(tempStack.Count>0)
            {
                uiStack.Push(tempStack.Pop());
            }
        }
    }

    // UI 활성/비활성화 토글
    public void ToggleUI(GameObject ui)
    {
        if(ui.activeSelf)
        {
            CloseUI(ui);
        }
        else
        {
            OpenUI(ui);
        }
    }

    // 가장 최근에 활성화된 UI 비활성화
    public void CloseTopUI()
    {
        if (uiStack.Count == 0) return;

        GameObject topUI = uiStack.Pop();
        topUI.SetActive(false);
    }

    // UI가 하나라도 열려있는지 여부
    public bool IsAnyUIOpen()
    {
        return uiStack.Count > 0;
    }

    // 열려있는 UI가 없는경우 설정UI 활성화
    private void ToggleMenuUI()
    {
        menuUI.SetActive(!menuUI.activeSelf);
    }
}

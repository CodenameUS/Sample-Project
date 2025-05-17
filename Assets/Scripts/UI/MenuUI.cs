using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuUI : MonoBehaviour
{
    [Header("Connected UI")]
    [SerializeField] private GameObject settingsButtonGo;               
    [SerializeField] private GameObject exitButtonGo;                   
    [SerializeField] private GameObject cancelButtonGo;                
    [SerializeField] private GameObject settingsGo;

  
    // 세팅 버튼 클릭 이벤트
    public void OnSettingsButtonClicked()
    {
        // 볼륨 세팅 UI 활성화
        UIManager.Instance.CloseUI(this.gameObject);
        UIManager.Instance.OpenUI(settingsGo);
    }

    // 게임 종료 버튼 클릭 이벤트
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    // 취소 버튼 클릭 이벤트
    public void OnCancelButtonClicked()
    {
        // 메뉴창 비활성화
        UIManager.Instance.CloseUI(this.gameObject);
    }
}

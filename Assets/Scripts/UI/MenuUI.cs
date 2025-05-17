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

  
    // ���� ��ư Ŭ�� �̺�Ʈ
    public void OnSettingsButtonClicked()
    {
        // ���� ���� UI Ȱ��ȭ
        UIManager.Instance.CloseUI(this.gameObject);
        UIManager.Instance.OpenUI(settingsGo);
    }

    // ���� ���� ��ư Ŭ�� �̺�Ʈ
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    // ��� ��ư Ŭ�� �̺�Ʈ
    public void OnCancelButtonClicked()
    {
        // �޴�â ��Ȱ��ȭ
        UIManager.Instance.CloseUI(this.gameObject);
    }
}

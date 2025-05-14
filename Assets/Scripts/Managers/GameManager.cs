using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
            - PlayerController Ŭ������ ���� ����
            - ĳ��������â Ȱ��/��Ȱ��ȭ(P)
            - ī�޶� ���� ����
 */
public class GameManager : Singleton<GameManager>
{
    [SerializeField] public PlayerController player;
    [SerializeField] public GameObject profileUI;
    [SerializeField] public CinemachineVirtualCamera virtualCamera;

    protected override void Awake()
    {
        base.Awake();
    }

  
    private void Update()
    {
        // ĳ���� ����â Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManager.Instance.ToggleUI(profileUI);
        }
    }
}

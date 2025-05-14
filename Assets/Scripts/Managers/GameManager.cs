using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
            - PlayerController 클래스에 접근 제공
            - 캐릭터정보창 활성/비활성화(P)
            - 카메라 접근 제공
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
        // 캐릭터 정보창 활성화
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManager.Instance.ToggleUI(profileUI);
        }
    }
}

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
    public PlayerController player;
    public CinemachineVirtualCamera virtualCamera;
    public bool isMultiPlaying = false;                     // 멀티플레잉 여부

    [SerializeField] public GameObject profileUI;

    protected override void Awake()
    {
        base.Awake();
        FindPlayerObject();
        FindCameraObject();
    }
    
    private void Update()
    {
        // 캐릭터 정보창 활성화
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManager.Instance.ToggleUI(profileUI);
        }
    }

    public void FindPlayerObject()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void FindCameraObject()
    {
        GameObject cam = GameObject.FindWithTag("PlayerCamera");
        if(cam != null)
        {
            virtualCamera = cam.GetComponent<CinemachineVirtualCamera>();
            virtualCamera.Follow = player.transform;
        }
    }
}

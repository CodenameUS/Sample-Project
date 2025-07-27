using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
                            << GameManager >>

        - 플레이어, 카메라 및 플레이어 상태 참조 제공(싱글톤)
        
        - 'P'키 : 플레이어 프로필 UI 활성/비활성화

        - FindPlayerObject(), FindCameraObject() : 플레이어, 카메라 오브젝트 참조 등록(씬변경 후)
 */

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public CinemachineVirtualCamera virtualCamera;
    
    public bool isMultiPlaying = false;                     // 멀티플레잉 여부
    public bool isChatting = false;                         // 채팅중인지 여부
    public bool isLoading = false;                          // 로딩중인지 여부

    [SerializeField] public GameObject profileUI;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
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

    // 플레이어 오브젝트 탐색(멀티 <-> 싱글 변경시)후 할당
    public void FindPlayerObject()
    {
        player = FindObjectOfType<PlayerController>();
    }
    
    // 카메라 오브젝트 탐색(멀티 <-> 싱글 변경시)후 할당
    public void FindCameraObject()
    {
        GameObject cam = GameObject.FindWithTag("PlayerCamera");
        if(cam != null)
        {
            virtualCamera = cam.GetComponentInChildren<CinemachineVirtualCamera>();
            virtualCamera.Follow = player.transform;
        }
        
    }
}

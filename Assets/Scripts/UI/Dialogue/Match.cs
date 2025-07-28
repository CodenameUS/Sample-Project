using UnityEngine;

/*
                            << Match >>

        - 매칭 NPC 관리 클래스
 */

public class Match : NPC
{
    [SerializeField] private GameObject matchUI;            // 매치 UI 게임오브젝트

    private void Start()
    {
        npcUI = matchUI;
    }

    private void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.G) && !matchUI.activeSelf)
        {
            SetActiveNpcUI();
        }
    }
}

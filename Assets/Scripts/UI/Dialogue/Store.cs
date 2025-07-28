using UnityEngine;

/*
                            << Store >>

        - 상점 NPC 관리 클래스
        
        - DialogueManager에 대화 데이터를 기반으로 상호작용(대화) 요청
 */

public class Store : NPC
{
    [SerializeField] private DialogueDataSO dialogueData;               // 상점 NPC 대화 데이터
    [SerializeField] private GameObject storeUI;                        // 상점 UI

    private void Start()
    {
        npcUI = storeUI;
    }

    private void Update()
    {
        // 대화 시작
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.G) && !dialogueUI.activeSelf && !storeUI.activeSelf)
        {
            DialogueManager.Instance.StartDialogue(dialogueData, this);
        }
    }  
}

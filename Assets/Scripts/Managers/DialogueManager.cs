using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
                            << DialogueManager >>

        - 대화 데이터를 통해 NPC와 대화 기능
        
        - NPC의 대화를 "--" 기준으로 나누어 여러 페이지에 걸쳐 출력
            - Scriptable Object로 대화데이터 작성
        
        - 대화시 타이핑 효과 출력(TypePage)  
 */

public class DialogueManager : Singleton<DialogueManager>
{ 
    [SerializeField] public GameObject dialogueUI;              // 대화창 오브젝트
    [SerializeField] public TMP_Text npcNameText;               // NPC 이름 텍스트
    [SerializeField] public TMP_Text dialogueText;              // 대화 텍스트

    private Queue<string> pages = new Queue<string>();
    private bool isTypipng = false;                             // 대화 타이핑 효과
    private float typingSpeed = 0.05f;                          // 타이핑 속도

    [HideInInspector]
    public NPC npc;                                             // 현재 대상 NPC
    [HideInInspector]                                           
    public bool isReadyToTalk = true;                           // 대화하기 가능여부(중복대화방지)
    [HideInInspector]
    public bool isFirstDialogue = true;                         // 대화의 시작인지여부

    private void Update()
    {
        // 'G'키 입력으로 다음 대화내용 출력
        if(dialogueUI.activeSelf && npc != null && Input.GetKeyDown(KeyCode.G))
        {
            DisplayNextPage(npc);
        }
    }

    // 대화시작
    public void StartDialogue(DialogueDataSO dialogue, NPC npcData)
    {
        if (!isReadyToTalk || !isFirstDialogue)
            return;

        isReadyToTalk = false;

        npc = npcData;

        // 대화창 UI 활성화
        dialogueUI.SetActive(true);                     
        // 대화창 NPC 이름 설정
        npcNameText.text = dialogue.npcName;

        // 이전 대화 내용 초기화 후 새로운 대화 내용으로 채우기
        pages.Clear();                                  

        foreach(string sentence in dialogue.sentences)
        {
            SplitSentenceToPages(sentence);
        }

        DisplayNextPage(npc);
    }

    // 다음 대화내용 출력
    public void DisplayNextPage(NPC npc)
    {
        if (isTypipng) return;

        // 더이상 출력할 내용이 없으면
        if(pages.Count == 0)
        {
            EndDialogue();                      // 대화창 비활성화 
            npc.SetActiveNpcUI();               // 현재 대화중인 NPC의 UI 출력
            return;
        }

        string page = pages.Dequeue();
        StopAllCoroutines();
        
        StartCoroutine(TypePage(page));
    }

    // 기호 "--"를 기준으로 대화 페이지 나누기
    private void SplitSentenceToPages(string sentence)
    {
        string[] pagesArray = sentence.Split(new string[] { "--" }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string page in pagesArray)
        {
            pages.Enqueue(page.Trim());         // 공백 제거 후 큐에 저장
        }
    }
 
    // 타이핑 효과
    private IEnumerator TypePage(string page)
    {
        isTypipng = true;
        dialogueText.text = "";

        AudioManager.Instance.PlaySFX("DialogueEffect");

        foreach (char letter in page.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypipng = false;
        AudioManager.Instance.StopSFX("DialogueEffect");

    }

    // 대화창 비활성화
    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isReadyToTalk = true;
    }
}

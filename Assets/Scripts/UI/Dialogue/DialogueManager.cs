using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueUI;             // 대화창 오브젝트
    [SerializeField] private TMP_Text npcNameText;              // NPC 이름 텍스트
    [SerializeField] private TMP_Text dialogueText;             // 대화 텍스트

    private Queue<string> pages = new Queue<string>();
    private bool isTypipng = false;                             // 대화 타이핑 효과
    private float typingSpeed = 0.05f;                          // 타이핑 속도
        
    private void Update()
    {
        if(dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.G))
        {
            DisplayNextPage();
        }
    }

    // 대화시작
    public void StartDialogue(DialogueDataSO dialogue)
    {
        dialogueUI.SetActive(true);                     // 대화창 UI 활성화

        npcNameText.text = dialogue.npcName;
        pages.Clear();                                  // 이전 대화 내용 초기화

        // 각 문장을 "--" 기준으로 나누어 페이지 저장
        foreach(string sentence in dialogue.sentences)
        {
            SplitSentenceToPages(sentence);
        }

        DisplayNextPage();
    }

    // 다음페이지 대화내용 출력
    public void DisplayNextPage()
    {
        if (isTypipng) return;

        if(pages.Count == 0)
        {
            EndDialogue();
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

        foreach(char letter in page.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypipng = false;
    }

    // 대화창 비활성화
    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
    }
}

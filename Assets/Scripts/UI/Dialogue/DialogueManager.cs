using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueUI;             // 대화창 오브젝트
    [SerializeField] private TMP_Text npcNameText;                  // NPC 이름 텍스트
    [SerializeField] private TMP_Text dialogueText;                 // 대화 텍스트

    private Queue<string> pages = new Queue<string>();
    private bool isTypipng = false;                             // 대화 타이핑 효과
    private float typingSpeed = 0.05f;                          // 타이핑 속도
    private int maxLinePerPage = 2;                             // 대화는 최대 2줄까지 표시
        
    private void Update()
    {
        if(dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextPage();
        }
    }

    // 대화시작
    public void StartDialogue(DialogueDataSO dialogue)
    {
        dialogueUI.SetActive(true);

        npcNameText.text = dialogue.npcName;
        pages.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            SplitSentenceToPages(sentence);
        }

        DisplayNextPage();
    }

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

    // 줄 수를 기준으로 대화 페이지 나누기
    private void SplitSentenceToPages(string sentence)
    {
        TMP_Text tempText = Instantiate(dialogueText, dialogueText.transform.parent);
        tempText.gameObject.SetActive(false);

        List<string> splitPages = new List<string>();

        string[] words = sentence.Split(' ');
        string currentPageText = "";

        for (int i = 0; i < words.Length; i++)
        {
            // 다음 단어 추가
            string testText = currentPageText.Length > 0 ? currentPageText + " " + words[i] : words[i];
            tempText.text = testText;
            tempText.ForceMeshUpdate();

            int lineCount = tempText.textInfo.lineCount;

            if (lineCount > maxLinePerPage)
            {
                // 이전 상태를 페이지로 저장하고 초기화
                splitPages.Add(currentPageText);

                // 현재 단어부터 다시 시작
                currentPageText = words[i];
            }
            else
            {
                currentPageText = testText;
            }
        }

        // 마지막 남은 텍스트 추가
        if (!string.IsNullOrEmpty(currentPageText))
        {
            splitPages.Add(currentPageText);
        }

        Destroy(tempText.gameObject);

        // 페이지 큐에 저장
        foreach (var page in splitPages)
        {
            pages.Enqueue(page);
        }
    }
 

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

    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
    }
}

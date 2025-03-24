using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueUI;             // ��ȭâ ������Ʈ
    [SerializeField] private TMP_Text npcNameText;                  // NPC �̸� �ؽ�Ʈ
    [SerializeField] private TMP_Text dialogueText;                 // ��ȭ �ؽ�Ʈ

    private Queue<string> pages = new Queue<string>();
    private bool isTypipng = false;                             // ��ȭ Ÿ���� ȿ��
    private float typingSpeed = 0.05f;                          // Ÿ���� �ӵ�
    private int maxLinePerPage = 2;                             // ��ȭ�� �ִ� 2�ٱ��� ǥ��
        
    private void Update()
    {
        if(dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextPage();
        }
    }

    // ��ȭ����
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

    // �� ���� �������� ��ȭ ������ ������
    private void SplitSentenceToPages(string sentence)
    {
        TMP_Text tempText = Instantiate(dialogueText, dialogueText.transform.parent);
        tempText.gameObject.SetActive(false);

        List<string> splitPages = new List<string>();

        string[] words = sentence.Split(' ');
        string currentPageText = "";

        for (int i = 0; i < words.Length; i++)
        {
            // ���� �ܾ� �߰�
            string testText = currentPageText.Length > 0 ? currentPageText + " " + words[i] : words[i];
            tempText.text = testText;
            tempText.ForceMeshUpdate();

            int lineCount = tempText.textInfo.lineCount;

            if (lineCount > maxLinePerPage)
            {
                // ���� ���¸� �������� �����ϰ� �ʱ�ȭ
                splitPages.Add(currentPageText);

                // ���� �ܾ���� �ٽ� ����
                currentPageText = words[i];
            }
            else
            {
                currentPageText = testText;
            }
        }

        // ������ ���� �ؽ�Ʈ �߰�
        if (!string.IsNullOrEmpty(currentPageText))
        {
            splitPages.Add(currentPageText);
        }

        Destroy(tempText.gameObject);

        // ������ ť�� ����
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

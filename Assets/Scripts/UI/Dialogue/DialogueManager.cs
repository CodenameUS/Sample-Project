using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueUI;             // ��ȭâ ������Ʈ
    [SerializeField] private TMP_Text npcNameText;              // NPC �̸� �ؽ�Ʈ
    [SerializeField] private TMP_Text dialogueText;             // ��ȭ �ؽ�Ʈ

    private Queue<string> pages = new Queue<string>();
    private bool isTypipng = false;                             // ��ȭ Ÿ���� ȿ��
    private float typingSpeed = 0.05f;                          // Ÿ���� �ӵ�
        
    private void Update()
    {
        if(dialogueUI.activeSelf && Input.GetKeyDown(KeyCode.G))
        {
            DisplayNextPage();
        }
    }

    // ��ȭ����
    public void StartDialogue(DialogueDataSO dialogue)
    {
        dialogueUI.SetActive(true);                     // ��ȭâ UI Ȱ��ȭ

        npcNameText.text = dialogue.npcName;
        pages.Clear();                                  // ���� ��ȭ ���� �ʱ�ȭ

        // �� ������ "--" �������� ������ ������ ����
        foreach(string sentence in dialogue.sentences)
        {
            SplitSentenceToPages(sentence);
        }

        DisplayNextPage();
    }

    // ���������� ��ȭ���� ���
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

    // ��ȣ "--"�� �������� ��ȭ ������ ������
    private void SplitSentenceToPages(string sentence)
    {
        string[] pagesArray = sentence.Split(new string[] { "--" }, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (string page in pagesArray)
        {
            pages.Enqueue(page.Trim());         // ���� ���� �� ť�� ����
        }
    }
 
    // Ÿ���� ȿ��
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

    // ��ȭâ ��Ȱ��ȭ
    private void EndDialogue()
    {
        dialogueUI.SetActive(false);
    }
}

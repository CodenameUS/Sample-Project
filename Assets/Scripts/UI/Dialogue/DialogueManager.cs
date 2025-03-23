using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueUI;             // ��ȭâ ������Ʈ
    [SerializeField] private Text npcNameText;                  // NPC �̸� �ؽ�Ʈ
    [SerializeField] private Text dialogueText;                 // ��ȭ �ؽ�Ʈ

    private Queue<string> sentences = new Queue<string>();
    private bool isTypipng = false;                             // ��ȭ Ÿ���� ȿ��
    private float typingSpeed = 0.05f;                          // Ÿ���� �ӵ�

    // ��ȭ����
    public void StartDialogue(DialogueDataSO dialogue)
    {
        dialogueUI.SetActive(true);

        npcNameText.text = dialogue.npcName;
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTypipng) return;

        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTypipng = true;
        dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray())
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

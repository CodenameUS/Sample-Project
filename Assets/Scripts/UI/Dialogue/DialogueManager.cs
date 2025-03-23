using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueUI;             // 대화창 오브젝트
    [SerializeField] private Text npcNameText;                  // NPC 이름 텍스트
    [SerializeField] private Text dialogueText;                 // 대화 텍스트

    private Queue<string> sentences = new Queue<string>();
    private bool isTypipng = false;                             // 대화 타이핑 효과
    private float typingSpeed = 0.05f;                          // 타이핑 속도

    // 대화시작
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private DialogueDataSO dialogueData;
    [SerializeField] private GameObject dialogueUI;

    private bool isPlayerInRange = false;                   // �÷��̾ �����ȿ� �ִ��� ����
    private BoxCollider rangeCol;                           // ��ȣ�ۿ� ����

    private void Awake()
    {
        rangeCol = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.G) && !dialogueUI.activeSelf)
        {
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}

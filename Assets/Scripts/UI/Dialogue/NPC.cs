using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                    NPC

        - �÷��̾�� ��ȣ�ۿ�(GŰ)
            �� BoxCollider

        - DialogueManager ���� �� NPC�� ��ȭ�� ������ �� �ֵ��� �����͸� �ѱ�

 */
public class NPC : MonoBehaviour
{
    protected GameObject dialogueUI;
    
    [HideInInspector]
    public GameObject npcUI;                                     // ���� NPC�� UI

    protected bool isPlayerInRange = false;                      // �÷��̾ �����ȿ� �ִ��� ����

    private void Awake()
    {
        dialogueUI = DialogueManager.Instance.dialogueUI;
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

    // NPC UI Ȱ��
    public void SetActiveNpcUI()
    {
        npcUI.SetActive(true);
        DialogueManager.Instance.npc = null;
    }

    
}

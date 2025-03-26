using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : NPC
{
    [SerializeField] private DialogueDataSO dialogueData;
    [SerializeField] private GameObject storeUI;

    private void Start()
    {
        npcUI = storeUI;
    }
    private void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.G) && !dialogueUI.activeSelf)
        {
            DialogueManager.Instance.StartDialogue(dialogueData, this);
        }
    }  
}

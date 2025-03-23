using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private DialogueDataSO dialogueData;

    public bool isPlayerInRange = false;
    private BoxCollider rangeCol;

    private void Awake()
    {
        rangeCol = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.G))
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : NPC
{
    [SerializeField] private GameObject matchUI;

    private void Start()
    {
        npcUI = matchUI;
    }

    private void Update()
    {
        if(isPlayerInRange && Input.GetKeyDown(KeyCode.G) && !matchUI.activeSelf)
        {
            SetActiveNpcUI();
        }
    }
}

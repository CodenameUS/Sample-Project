using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public PlayerController player;
    [SerializeField] public GameObject profileUI;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            profileUI.SetActive(!profileUI.activeSelf);
        }
    }
}

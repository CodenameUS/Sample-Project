using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector]
    public float hAxis;
    [HideInInspector]
    public float vAxis;
    public bool attackKeydown;
    public bool skill1Keydown;

    private void Update()
    {
        GetInput();
    }

    // ����� �Է�
    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        attackKeydown = Input.GetButtonDown("Attack");
        skill1Keydown = Input.GetButtonDown("Skill1");
    }

    
}

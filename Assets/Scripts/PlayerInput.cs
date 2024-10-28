using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float hAxis;
    public float vAxis;
    public bool attackKeydown;

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
    }

    
}

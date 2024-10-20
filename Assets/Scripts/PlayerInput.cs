using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float hAxis;
    public float vAxis;

    private void Update()
    {
        GetInput();
    }

    // 사용자 입력
    private void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
    }

    
}

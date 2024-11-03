using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    BoxCollider boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
            Debug.Log("Hit!!");
    }
}

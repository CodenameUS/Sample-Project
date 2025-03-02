using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;          // Ÿ�ٴ��

    private Vector3 offset;                             // Ÿ�ٰ��� Offset

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        offset = new Vector3(0, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        transform.position = target.position + offset;
    }
}

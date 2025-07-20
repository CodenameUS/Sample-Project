using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntRange : MonoBehaviour
{
    [SerializeField] private BoxCollider scanRange;         // 적 탐지 범위
    private Grunt parent;

    private void Awake()
    {
        parent = GetComponentInParent<Grunt>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || other.GetComponent<PlayerController>().isCutscenePlaying)
            return;

        parent.TargetPlayer = GameManager.Instance.player;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private Animator animator;

    int hashAttackCount = Animator.StringToHash("AttackCount");

    private bool hasWeapon = false;                                 // 무기 장착여부(현재는 기본 false)

    // AttackCount 설정
    public int AttackCount
    {
        get => animator.GetInteger(hashAttackCount);
        set => animator.SetInteger(hashAttackCount, value);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        DoAttack();
    }

    private void DoAttack()
    {
        // 무기가 없을 때(주먹공격)
        if(!hasWeapon)
        {
            AttackCount = 0;
            if (playerInput.attackKeydown)
            {
                animator.SetTrigger("Attack");
            }
        }

      
    }
}

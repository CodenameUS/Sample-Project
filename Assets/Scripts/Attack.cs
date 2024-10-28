using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private Animator animator;

    int hashAttackCount = Animator.StringToHash("AttackCount");

    private bool hasWeapon = false;                                 // ���� ��������(����� �⺻ false)

    // AttackCount ����
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
        // ���Ⱑ ���� ��(�ָ԰���)
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

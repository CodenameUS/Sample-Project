using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private BoxCollider leftHandCollider;               
    [SerializeField] private BoxCollider rightHandCollider;

    private Animator anim;

    readonly private int hashAttackType = Animator.StringToHash("AttackType");
    readonly private int hashComboCount = Animator.StringToHash("ComboCount");
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");

    private bool hasEquipedWeapon = false;      // 무기 장착여부(현재는 기본 false)
    private bool isAttack = false;              // 공격중인지 여부
    private bool isComboAllowed = false;        // 콤보공격 가능여부

    private int comboCount = 0;                 // 콤보공격 카운트

    // AttackType 설정
    public int AttackType
    {
        get => anim.GetInteger(hashAttackType);
        set => anim.SetInteger(hashAttackType, value);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        BaseAttack();
    }

    // 기본공격
    private void BaseAttack()
    {
        // 무기가 없을 때(주먹공격)
        if(hasEquipedWeapon == false)
        {
            AttackType = 0;
        }
     
        if(playerInput.attackKeydown)
        {
            if(!isAttack && comboCount == 0)
            {
                comboCount = 1;
                StartCoroutine(BaseAttackPunch(comboCount));
            }
            else if(isComboAllowed && comboCount == 1)
            {
                comboCount = 2;
                StartCoroutine(BaseAttackPunch(comboCount));
            }
        }
    }

    // 기본 주먹공격
    IEnumerator BaseAttackPunch(int combo)
    {
        isAttack = true;
        isComboAllowed = true;

        anim.SetInteger(hashAttackType, AttackType);
        anim.SetInteger(hashComboCount, comboCount);
        anim.SetTrigger(hashAttackTrigger);

        // 애니메이션에 맞게 콜라이더 활성/비활성화
        yield return new WaitForSeconds(0.3f);
        if (combo == 1)
            leftHandCollider.enabled = true;
        else
            rightHandCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);
        if (combo == 1)
            leftHandCollider.enabled = false;
        else
        {
            rightHandCollider.enabled = false;
            isComboAllowed = false;
        }

        comboCount = 0;
        isAttack = false;
    }

}

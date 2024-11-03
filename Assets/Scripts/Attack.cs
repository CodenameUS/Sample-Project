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

    private bool hasEquipedWeapon = false;      // ���� ��������(����� �⺻ false)
    private bool isAttack = false;              // ���������� ����
    private bool isComboAllowed = false;        // �޺����� ���ɿ���

    private int comboCount = 0;                 // �޺����� ī��Ʈ

    // AttackType ����
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

    // �⺻����
    private void BaseAttack()
    {
        // ���Ⱑ ���� ��(�ָ԰���)
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

    // �⺻ �ָ԰���
    IEnumerator BaseAttackPunch(int combo)
    {
        isAttack = true;
        isComboAllowed = true;

        anim.SetInteger(hashAttackType, AttackType);
        anim.SetInteger(hashComboCount, comboCount);
        anim.SetTrigger(hashAttackTrigger);

        // �ִϸ��̼ǿ� �°� �ݶ��̴� Ȱ��/��Ȱ��ȭ
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    
    #region SerializeFields
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private BoxCollider leftHandCollider;               
    [SerializeField] private BoxCollider rightHandCollider;

    #endregion

    #region �������
    private Animator anim;

    readonly private int hashAttackType = Animator.StringToHash("AttackType");
    readonly private int hashComboCount = Animator.StringToHash("ComboCount");
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");
    
    
    private bool hasEquipedWeapon = false;      // ���� ��������(����� �⺻ false)
    private bool isAttack = false;              // ���������� ����
    private bool isComboAllowed = false;        // �޺����� ���ɿ���
    private int comboCount = 0;                 // �޺����� ī��Ʈ

    public int AttackType
    {
        get => anim.GetInteger(hashAttackType);
        set => anim.SetInteger(hashAttackType, value);
    }

    #endregion

    #region Unity Events
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        BaseAttack();
    }
    #endregion

    // �⺻����
    private void BaseAttack()
    {
        // ���Ⱑ ���� ��(�ָ԰���)
        if(hasEquipedWeapon == false)
        {
            AttackType = 0;
        }
     
        if(playerInput.attackKeydown && !isAttack)
        {
            StartCoroutine(BaseAttackPunch());
        }
    }

    // �⺻ �ָ԰���
    private IEnumerator BaseAttackPunch()
    {
        isAttack = true;
        comboCount++;

        anim.SetInteger(hashAttackType, AttackType);
        anim.SetInteger(hashComboCount, comboCount);
        anim.SetTrigger(hashAttackTrigger);

        // ����1Ÿ
        if (comboCount == 1)
        {
            leftHandCollider.enabled = true;
            yield return new WaitForSeconds(0.5f);

            // �߰� �޺��Է°���
            float timer = 0;
            while (timer < 0.5f)
            {
                if (playerInput.attackKeydown)
                {
                    comboCount++;
                    isComboAllowed = true;
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
            leftHandCollider.enabled = false;
        }

        // �߰� �޺��Է��� �߻����� ��
        if(comboCount == 2)
        {
            rightHandCollider.enabled = true;
            anim.SetInteger(hashComboCount, comboCount);
            yield return new WaitForSeconds(1f);
            rightHandCollider.enabled = false;
        }

        // �ĵ����̸����� �ǵ�ġ���� �ڷ�ƾ���ื��
        yield return new WaitForSeconds(0.3f);               

        // �����ʱ�ȭ
        comboCount = 0;
        anim.SetInteger(hashComboCount, comboCount);
        isComboAllowed = false;
        isAttack = false;
    }

   
}

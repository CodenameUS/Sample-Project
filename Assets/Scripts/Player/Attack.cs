using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    
    #region SerializeFields
    [SerializeField] private BoxCollider leftHandCollider;               
    [SerializeField] private BoxCollider rightHandCollider;

    #endregion

    #region 멤버변수
    private Animator anim;

    readonly private int hashAttackType = Animator.StringToHash("AttackType");
    readonly private int hashComboCount = Animator.StringToHash("ComboCount");
    readonly private int hashAttackTrigger = Animator.StringToHash("Attack");

    private bool attackKeydown;
    private bool hasEquipedWeapon = false;      // 무기 장착여부(현재는 기본 false)
    private bool isAttack = false;              // 공격중인지 여부
    private bool isComboAllowed = false;        // 콤보공격 가능여부
    private int comboCount = 0;                 // 콤보공격 카운트

    public int AttackType
    {
        get => anim.GetInteger(hashAttackType);
        set => anim.SetInteger(hashAttackType, value);
    }

    #endregion

    #region Unity Events
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        GetInput();
        BaseAttack();
    }
    #endregion

    private void Init()
    {
        anim = GetComponent<Animator>();
    }

    private void GetInput()
    {
        attackKeydown = Input.GetButtonDown("Attack");
    }

    // 기본공격
    private void BaseAttack()
    {
        // 무기가 없을 때(주먹공격)
        if(hasEquipedWeapon == false)
        {
            AttackType = 0;
        }
     
        if(attackKeydown && !isAttack)
        {
            StartCoroutine(BaseAttackPunch());
        }
    }

    // 기본 주먹공격
    private IEnumerator BaseAttackPunch()
    {
        isAttack = true;
        comboCount++;

        anim.SetInteger(hashAttackType, AttackType);
        anim.SetInteger(hashComboCount, comboCount);
        anim.SetTrigger(hashAttackTrigger);

        // 공격1타
        if (comboCount == 1)
        {
            leftHandCollider.enabled = true;
            yield return new WaitForSeconds(0.5f);

            // 추가 콤보입력감지
            float timer = 0;
            while (timer < 0.5f)
            {
                if (attackKeydown)
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

        // 추가 콤보입력이 발생했을 때
        if(comboCount == 2)
        {
            rightHandCollider.enabled = true;
            anim.SetInteger(hashComboCount, comboCount);
            yield return new WaitForSeconds(1f);
            rightHandCollider.enabled = false;
        }

        // 후딜레이를통해 의도치않은 코루틴실행막기
        yield return new WaitForSeconds(0.3f);               

        // 공격초기화
        comboCount = 0;
        anim.SetInteger(hashComboCount, comboCount);
        isComboAllowed = false;
        isAttack = false;
    }

   
}

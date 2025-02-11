using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
                    WeaponController : 무기 및 전략 관리

            - SetWeapon() : 현재 무기 설정
            - ChangeAttackStrategy() : 공격 전략 변경
            - Attack() : 현재 무기의 공격 실행
 */

public class WeaponController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject myWeapon;

    private Weapon currentWeapon;
    private WeaponType weaponType;

    readonly private int hashWeaponType = Animator.StringToHash("WeaponType");

    // 무기 타입별 AttackType 파라미터값 
    public int AttackType
    {
        get => player.Anim.GetInteger(hashWeaponType);
        set => player.Anim.SetInteger(hashWeaponType, value);
    }

    private void Start()
    {
        SetWeapon();
    }

    // 현재 무기 세팅
    public void SetWeapon()
    {
        currentWeapon = myWeapon.GetComponentInChildren<Weapon>();          // 현재 장착중인 무기 가져오기
        
        // 무기가 없을 때(임시)
        if(currentWeapon == null)
        {
            weaponType = WeaponType.None;
        }
        else
        {
            weaponType = currentWeapon.type;
        }
        Debug.Log("현재 무기 타입 : " + weaponType);
        // 무기별 애니메이션
        AttackType = (int)weaponType;
    }

    // 공격 전략 변경
    public void ChangeAttackStrategy(IAttackStrategy newStrategy)
    {
        if(currentWeapon != null)
        {
            currentWeapon.SetAttackStrategy(newStrategy);
        }
        // 무기가 없을 때(임시)
        else
        {
            Debug.Log("무기가 장착되지 않았음");
        }
    }

    // 현재 무기 공격 실행
    public void Attack()
    {
        if(currentWeapon != null)
        {
            currentWeapon.PerformAttack();
        }
        // 무기가 없을 때(임시)
        else
        {
            Debug.Log("무기가 장착되지 않았음");
        }
    }
}

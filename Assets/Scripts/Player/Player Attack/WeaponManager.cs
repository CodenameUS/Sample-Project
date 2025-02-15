using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
                    WeaponManager : 무기 및 전략 관리

            - SetWeapon() : 현재 무기 설정
            - ChangeAttackStrategy() : 공격 전략 변경
            - Attack() : 현재 무기의 공격 실행
 */

public class WeaponManager : Singleton<WeaponManager>
{
    [SerializeField] PlayerController player;
    [SerializeField] Transform weaponTransform;

    public GameObject myWeapon;
    private WeaponType myWeaponType;
    public Weapon currentWeapon;

    readonly private int hashWeaponType = Animator.StringToHash("WeaponType");

    // 무기 타입별 WeaponType 파라미터값 
    public int CurWeaponType
    {
        get => player.Anim.GetInteger(hashWeaponType);
        set => player.Anim.SetInteger(hashWeaponType, value);
    }

    private void Start()
    {
        InitWeapon();
    }

    // 시작 무기 세팅
    public void InitWeapon()
    {
        currentWeapon = weaponTransform.GetComponentInChildren<Weapon>();          // 현재 장착중인 무기 가져오기
        
        // 무기가 없을 때
        if(currentWeapon == null)
        {
            // 맨손 무기 생성
            ResourceManager.Instance.LoadWeaponPrefab("Punch.prefab", prefab =>
            {
                if (prefab != null)
                {
                    // 프리팹 생성
                    GameObject newWeapon = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, weaponTransform);
                    newWeapon.transform.localPosition = prefab.transform.localPosition;
                    newWeapon.transform.localRotation = prefab.transform.localRotation;

                    // 무기 설정
                    myWeapon = newWeapon;
                    currentWeapon = myWeapon.GetComponent<Weapon>();
                    myWeaponType = WeaponType.None;
                }
                else
                {
                    Debug.Log($"Failed to load prefab for item : {prefab}");
                }
            });
        }
        else
        {
            myWeaponType = currentWeapon.type;
        }

        // 현재 무기 애니메이션설정
        CurWeaponType = (int)myWeaponType;
    }


    // 현재 무기 세팅(기본값 Punch)
    public void SetWeapon(string type = "None", string weapon = "Punch")
    {
        if(Enum.TryParse(type, out WeaponType result))
        {
            ResourceManager.Instance.LoadWeaponPrefab(weapon + ".prefab", prefab =>
            {
                if (prefab != null)
                {
                    currentWeapon = null;
                    // 기존 무기 삭제
                    Destroy(myWeapon);
                    // 프리팹 생성
                    GameObject newWeapon = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, weaponTransform);
                    newWeapon.transform.localPosition = prefab.transform.localPosition;
                    newWeapon.transform.localRotation = prefab.transform.localRotation;

                    myWeapon = newWeapon;
                    currentWeapon = myWeapon.GetComponent<Weapon>();

                    // 새로운 무기로 설정
                    myWeapon = newWeapon;
                    myWeaponType = result;
                    CurWeaponType = (int)myWeaponType;
                    
                }
                else
                {
                    Debug.Log($"Fail to load prefab for item : {prefab}");
                }
            });
        }
        else
        {
            Debug.Log($"{type} 은(는) 유효한 타입이 아닙니다.");
        }

       
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

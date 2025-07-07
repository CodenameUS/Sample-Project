using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
                    WeaponManager : 플레이어 무기에 따른 무기 프리팹생성 및 무기애니메이션 설정

            - SetWeapon() : 현재 무기 설정
 */

public class WeaponManager : Singleton<WeaponManager>
{
    [SerializeField] Transform weaponTransform;             // 무기가 생성될 위치

    #region ** Fields **
    private GameObject myWeaponGo;                      // 장착중인 무기 오브젝트
    private WeaponType myWeaponType;                    // 장착중인 무기의 타입
    public Weapon currentWeapon;                        // 현재 무기

    readonly private int hashWeaponType = Animator.StringToHash("WeaponType");
    #endregion 


    // 무기 타입별 WeaponType 파라미터값 
    public int CurWeaponType
    {
        get => GameManager.Instance.player.Anim.GetInteger(hashWeaponType);
        set => GameManager.Instance.player.Anim.SetInteger(hashWeaponType, value);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    // 현재 무기 세팅(기본값 Punch)
    public void SetWeapon(string type = "None", string weapon = "Punch")
    {
        if(Enum.TryParse(type, out WeaponType result))
        {
            if(!GameManager.Instance.isMultiPlaying)
            {
                // 싱글플레이 무기프리팹 생성
                ResourceManager.Instance.LoadWeaponPrefab(weapon + ".prefab", prefab =>
                {
                    if (prefab != null)
                    {
                        currentWeapon = null;
                        // 기존 무기 오브젝트 제거
                        Destroy(myWeaponGo);

                        // 프리팹 생성
                        GameObject newWeapon = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, weaponTransform);
                        newWeapon.transform.localPosition = prefab.transform.localPosition;
                        newWeapon.transform.localRotation = prefab.transform.localRotation;

                        // 현재 무기 설정
                        myWeaponGo = newWeapon;
                        currentWeapon = myWeaponGo.GetComponent<Weapon>();
                        myWeaponType = result;
                        CurWeaponType = (int)myWeaponType;
                        Debug.Log("현재 내 무기 : " + CurWeaponType);
                    }
                    else
                    {
                        Debug.Log($"다음 프리팹을 불러오는데 실패하였습니다. : {prefab}");
                    }
                });
            }
            // 멀티플레이 무기프리팹 생성
            else
            {

            }
        }
        else
        {
            Debug.Log($"{type} 은(는) 유효한 타입이 아닙니다.");
        }
    }
}

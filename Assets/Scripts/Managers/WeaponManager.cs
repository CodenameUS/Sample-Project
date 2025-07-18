using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

/*
                    WeaponManager : 플레이어 무기에 따른 무기 프리팹생성 및 무기애니메이션 설정

            - SetWeapon() : 현재 무기 설정
 */

public class WeaponManager : SingletonPun<WeaponManager>
{

    #region ** Fields **
    private Transform weaponPoint;                      // 무기가 생성될 위치
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
    public void SetWeapon(string type, string weapon)
    {
        if(Enum.TryParse(type, out WeaponType result))
        {
            // 싱글플레이
            if(!GameManager.Instance.isMultiPlaying)
            {
                // 무기프리팹 생성
                ResourceManager.Instance.LoadWeaponPrefab(weapon + ".prefab", prefab =>
                {
                    if (prefab != null)
                    {
                        currentWeapon = null;
                        // 기존 무기 오브젝트 제거
                        Destroy(myWeaponGo);
                        weaponPoint = GameManager.Instance.player.WeaponPoint;

                        // 프리팹 생성
                        GameObject newWeapon = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, weaponPoint);
                        newWeapon.transform.localPosition = prefab.transform.localPosition;
                        newWeapon.transform.localRotation = prefab.transform.localRotation;

                        // 현재 무기 설정
                        myWeaponGo = newWeapon;
                        currentWeapon = myWeaponGo.GetComponent<Weapon>();
                        myWeaponType = result;
                        CurWeaponType = (int)myWeaponType;
                    }
                    else
                    {
                        Debug.Log($"다음 프리팹을 불러오는데 실패하였습니다. : {prefab}");
                    }
                });
            }
            // 멀티플레이 
            else if(GameManager.Instance.isMultiPlaying)
            {
                currentWeapon = null;
                Destroy(myWeaponGo);

                weaponPoint = GameManager.Instance.player.WeaponPoint;
                GameObject weaponRef = Resources.Load<GameObject>("Weapons/" + weapon);
                Vector3 refLocalPos = weaponRef.transform.localPosition;
                Quaternion refLocalRot = weaponRef.transform.localRotation;

                // 무기 생성
                GameObject newWeapon = PhotonNetwork.Instantiate("Weapons/" + weapon, weaponPoint.position, weaponPoint.rotation);
                newWeapon.transform.SetParent(weaponPoint);
                newWeapon.transform.localPosition = refLocalPos;
                newWeapon.transform.localRotation = refLocalRot;

                myWeaponGo = newWeapon;
                currentWeapon = myWeaponGo.GetComponent<Weapon>();
                myWeaponType = result;
                CurWeaponType = (int)myWeaponType;
            }
        }
        else
        {
            Debug.Log($"{type} 은(는) 유효한 타입이 아닙니다.");
        }
    }

    [PunRPC]
    public void RPC_SetWeapon(string type, string weapon)
    {
        SetWeapon(type, weapon);
    }

    public void RequestSetWeapon(string type = "None", string weapon = "Punch")
    {
        if(GameManager.Instance.isMultiPlaying)
        {
            photonView.RPC("RPC_SetWeapon", RpcTarget.AllBuffered, type, weapon);
        }
        else
        {
            SetWeapon(type, weapon);
        }
    }
}

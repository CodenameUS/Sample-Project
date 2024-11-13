using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WeaponController : MonoBehaviour
{
    int testId = 1001;

    void Start()
    {
        WeaponItemData weaponData = DataManager.Instance.GetDataById(testId);

        if(weaponData != null)
        {
            Debug.Log("무기 ID : " + weaponData.ID);
            Debug.Log("무기 이름 : " + weaponData.ItemName);
            Debug.Log("무기 툴팁 : " + weaponData.ItemToolTip);
            Debug.Log("무기 데미지 : " + weaponData.Damage);
            Debug.Log("무기 공격속도 : " + weaponData.Rate);
        }
        else
        {
            Debug.Log("데이터가 없습니다!");
        }
    }
}

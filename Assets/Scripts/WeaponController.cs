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
            Debug.Log("���� ID : " + weaponData.ID);
            Debug.Log("���� �̸� : " + weaponData.ItemName);
            Debug.Log("���� ���� : " + weaponData.ItemToolTip);
            Debug.Log("���� ������ : " + weaponData.Damage);
            Debug.Log("���� ���ݼӵ� : " + weaponData.Rate);
        }
        else
        {
            Debug.Log("�����Ͱ� �����ϴ�!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
                    WeaponManager : ���� �� ���� ����

            - SetWeapon() : ���� ���� ����
            - ChangeAttackStrategy() : ���� ���� ����
            - Attack() : ���� ������ ���� ����
 */

public class WeaponManager : Singleton<WeaponManager>
{
    [SerializeField] PlayerController player;
    [SerializeField] Transform weaponTransform;

    public GameObject myWeapon;
    private WeaponType myWeaponType;
    public Weapon currentWeapon;

    readonly private int hashWeaponType = Animator.StringToHash("WeaponType");

    // ���� Ÿ�Ժ� WeaponType �Ķ���Ͱ� 
    public int CurWeaponType
    {
        get => player.Anim.GetInteger(hashWeaponType);
        set => player.Anim.SetInteger(hashWeaponType, value);
    }

    private void Start()
    {
        InitWeapon();
    }

    // ���� ���� ����
    public void InitWeapon()
    {
        currentWeapon = weaponTransform.GetComponentInChildren<Weapon>();          // ���� �������� ���� ��������
        
        // ���Ⱑ ���� ��
        if(currentWeapon == null)
        {
            // �Ǽ� ���� ����
            ResourceManager.Instance.LoadWeaponPrefab("Punch.prefab", prefab =>
            {
                if (prefab != null)
                {
                    // ������ ����
                    GameObject newWeapon = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, weaponTransform);
                    newWeapon.transform.localPosition = prefab.transform.localPosition;
                    newWeapon.transform.localRotation = prefab.transform.localRotation;

                    // ���� ����
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

        // ���� ���� �ִϸ��̼Ǽ���
        CurWeaponType = (int)myWeaponType;
    }


    // ���� ���� ����(�⺻�� Punch)
    public void SetWeapon(string type = "None", string weapon = "Punch")
    {
        if(Enum.TryParse(type, out WeaponType result))
        {
            ResourceManager.Instance.LoadWeaponPrefab(weapon + ".prefab", prefab =>
            {
                if (prefab != null)
                {
                    currentWeapon = null;
                    // ���� ���� ����
                    Destroy(myWeapon);
                    // ������ ����
                    GameObject newWeapon = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, weaponTransform);
                    newWeapon.transform.localPosition = prefab.transform.localPosition;
                    newWeapon.transform.localRotation = prefab.transform.localRotation;

                    myWeapon = newWeapon;
                    currentWeapon = myWeapon.GetComponent<Weapon>();

                    // ���ο� ����� ����
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
            Debug.Log($"{type} ��(��) ��ȿ�� Ÿ���� �ƴմϴ�.");
        }

       
    }

    // ���� ���� ����
    public void ChangeAttackStrategy(IAttackStrategy newStrategy)
    {
        if(currentWeapon != null)
        {
            currentWeapon.SetAttackStrategy(newStrategy);
        }
        // ���Ⱑ ���� ��(�ӽ�)
        else
        {
            Debug.Log("���Ⱑ �������� �ʾ���");
        }
    }

    // ���� ���� ���� ����
    public void Attack()
    {
        if(currentWeapon != null)
        {
            currentWeapon.PerformAttack();
        }
        // ���Ⱑ ���� ��(�ӽ�)
        else
        {
            Debug.Log("���Ⱑ �������� �ʾ���");
        }
    }
}

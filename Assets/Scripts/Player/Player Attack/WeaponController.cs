using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
                    WeaponController : ���� �� ���� ����

            - SetWeapon() : ���� ���� ����
            - ChangeAttackStrategy() : ���� ���� ����
            - Attack() : ���� ������ ���� ����
 */

public class WeaponController : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject myWeapon;

    private Weapon currentWeapon;
    private WeaponType weaponType;

    readonly private int hashWeaponType = Animator.StringToHash("WeaponType");

    // ���� Ÿ�Ժ� AttackType �Ķ���Ͱ� 
    public int AttackType
    {
        get => player.Anim.GetInteger(hashWeaponType);
        set => player.Anim.SetInteger(hashWeaponType, value);
    }

    private void Start()
    {
        SetWeapon();
    }

    // ���� ���� ����
    public void SetWeapon()
    {
        currentWeapon = myWeapon.GetComponentInChildren<Weapon>();          // ���� �������� ���� ��������
        
        // ���Ⱑ ���� ��(�ӽ�)
        if(currentWeapon == null)
        {
            weaponType = WeaponType.None;
        }
        else
        {
            weaponType = currentWeapon.type;
        }
        Debug.Log("���� ���� Ÿ�� : " + weaponType);
        // ���⺰ �ִϸ��̼�
        AttackType = (int)weaponType;
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

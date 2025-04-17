using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        IceShot
          
            - ������ų
             
*/

public class IceShot : Skill
{
    public IceShot(SkillData data) : base(data) { }

    public override void Activate(GameObject user)
    {
        // �ùٸ� ���⸦ �����ߴ��� ����
        bool hasWeapon = WeaponManager.Instance.currentWeapon.type == WeaponType.Staff;

        if (anim == null)
        {
            Debug.Log($"{user} �� Animator�� �������� ����.");
            return;
        }
        else if (!hasWeapon)
        {
            Debug.Log($"������ ����δ� ��ų�� ����� �� �����ϴ�.");
        }
        else
        {
            anim.SetTrigger("Skill");
            anim.SetInteger("SkillId", data.AnimId);
            Debug.Log($"{data.Name} : ���!");
        }
    }
}

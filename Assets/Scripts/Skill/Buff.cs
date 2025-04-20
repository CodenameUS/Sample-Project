using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        Buff
          
            - ������ų
             
*/

public class Buff : Skill
{
    public Buff(SkillData data) : base(data) { }

    public override bool Activate(GameObject user)
    {
        // ���⸦ �����ߴ��� ����
        bool hasWeapon = WeaponManager.Instance.currentWeapon != null;

        if (anim == null)
        {
            Debug.Log($"{user} �� Animator�� �������� ����.");
            return false;
        }
        else if (!hasWeapon)
        {
            Debug.Log($"������ ���Ⱑ �����ϴ�.");
            return false;
        }
        else
        {
            anim.SetTrigger("Skill");
            anim.SetInteger("SkillId", data.AnimId);
            Debug.Log($"{data.Name} : ���!");
            return true;
        }
    }
}

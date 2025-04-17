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

    public override void Activate(GameObject user)
    {
        // ���⸦ �����ߴ��� ����
        bool hasWeapon = WeaponManager.Instance.currentWeapon != null;

        if (anim == null)
        {
            Debug.Log($"{user} �� Animator�� �������� ����.");
            return;
        }
        else if (!hasWeapon)
        {
            Debug.Log($"������ ���Ⱑ �����ϴ�.");
        }
        else
        {
            anim.SetTrigger("Skill");
            anim.SetInteger("SkillId", data.AnimId);
            Debug.Log($"{data.Name} : ���!");
        }
    }
}

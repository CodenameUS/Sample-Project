using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        Slash
          
            - �˽�ų    
*/

public class Slash : Skill
{
    public Slash(SkillData data) : base(data) { }

    // ��ų ���
    public override bool Activate(GameObject user)
    {
        // �ùٸ� ���⸦ �����ߴ��� ����
        bool hasWeapon = WeaponManager.Instance.currentWeapon.type == WeaponType.Sword;

        if (anim == null)
        {
            Debug.Log($"{user} �� Animator�� �������� ����.");
            return false;
        }
        else if(!hasWeapon)
        {
            Debug.Log($"������ ����δ� ��ų�� ����� �� �����ϴ�.");
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

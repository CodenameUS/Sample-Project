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
    public override void Activate(GameObject user)
    {
        // �ùٸ� ���⸦ �����ߴ��� ����
        bool hasWeapon = WeaponManager.Instance.currentWeapon.type == WeaponType.Sword;

        if (anim == null)
        {
            Debug.Log($"{user} �� Animator�� �������� ����.");
            return;
        }
        else if(!hasWeapon)
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

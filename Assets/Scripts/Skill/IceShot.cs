using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        IceShot
          
            - 마법스킬
             
*/

public class IceShot : Skill
{
    public IceShot(SkillData data) : base(data) { }

    public override bool Activate(GameObject user)
    {
        // 올바른 무기를 장착했는지 여부
        bool hasWeapon = WeaponManager.Instance.currentWeapon.type == WeaponType.Staff;

        if (anim == null)
        {
            Debug.Log($"{user} 의 Animator가 존재하지 않음.");
            return false;
        }
        else if (!hasWeapon)
        {
            Debug.Log($"장착한 무기로는 스킬을 사용할 수 없습니다.");
            return false;
        }
        else
        {
            anim.SetTrigger("Skill");
            anim.SetInteger("SkillId", data.AnimId);
            Debug.Log($"{data.Name} : 사용!");
            return true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        Slash
          
            - 검스킬    
*/

public class Slash : Skill
{
    public Slash(SkillData data) : base(data) { }

    
    // 스킬 사용
    public override bool Activate(GameObject user)
    {
        // 올바른 무기를 장착했는지 여부
        bool hasWeapon = WeaponManager.Instance.currentWeapon.type == WeaponType.Sword;

        if (anim == null)
        {
            Debug.Log($"{user} 의 Animator가 존재하지 않음.");
            return false;
        }
        else if(!hasWeapon)
        {
            Debug.Log($"장착한 무기로는 스킬을 사용할 수 없습니다.");
            return false;
        }
        else
        { 
            anim.SetTrigger("Skill");
            anim.SetInteger("SkillId", data.AnimId);

            if(cachedEffect == null)
            {
                // 생성된 이펙트가 없으면 생성
                cachedEffect = UnityEngine.Object.Instantiate(effectPrefab,
                user.transform.position + new Vector3(0,1,0),
                user.transform.rotation, SkillManager.Instance.gameObject.transform);
            }
            else
            {
                // 생성된 이펙트가 있으면 새로운 위치 지정
                cachedEffect.transform.position = user.transform.position + new Vector3(0, 1, 0);
                cachedEffect.transform.rotation = user.transform.rotation;
            }

            cachedEffect.SetActive(true);

            return true;
        }
    }
}

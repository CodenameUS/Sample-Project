using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        Buff
          
            - 버프스킬
             
*/

public class Buff : Skill, IBuffSkill
{
    public Buff(SkillData data) : base(data) { }

    // 스킬 사용
    public override bool Activate(GameObject user)
    {
        // 무기를 장착했는지 여부
        bool hasWeapon = WeaponManager.Instance.currentWeapon != null;

        if (anim == null)
        {
            Debug.Log($"{user} 의 Animator가 존재하지 않음.");
            return false;
        }
        else if (!hasWeapon)
        {
            Debug.Log($"장착한 무기가 없습니다.");
            return false;
        }
        else
        {
            // 애니메이션 설정
            anim.SetTrigger("Skill");
            anim.SetInteger("SkillId", data.AnimId);

            if(cachedEffect == null)
            {
                cachedEffect = UnityEngine.Object.Instantiate(effectPrefab);

                FollowingEffect(user);
            }
            else
            {
                // 생성된 이펙트가 있으면 새로운 위치 지정
                cachedEffect.transform.position = user.transform.position;
                cachedEffect.transform.rotation = user.transform.rotation;
            }

            cachedEffect.SetActive(true);

            return true;
        }
    }

    public void FollowingEffect(GameObject user)
    {
        FollowTarget follow = cachedEffect.AddComponent<FollowTarget>();
        follow.target = user.transform;
        follow.duration = data.Cooldown / 2;
    }

  
}

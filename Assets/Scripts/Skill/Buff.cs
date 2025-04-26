using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        Buff
          
            - ������ų
             
*/

public class Buff : Skill, IBuffSkill
{
    public Buff(SkillData data) : base(data) { }

    // ��ų ���
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
            // �ִϸ��̼� ����
            anim.SetTrigger("Skill");
            anim.SetInteger("SkillId", data.AnimId);

            if(cachedEffect == null)
            {
                cachedEffect = UnityEngine.Object.Instantiate(effectPrefab);

                FollowingEffect(user);
            }
            else
            {
                // ������ ����Ʈ�� ������ ���ο� ��ġ ����
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

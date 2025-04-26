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

    // ��ų ���
    public override bool Activate(GameObject user)
    {
        // �ùٸ� ���⸦ �����ߴ��� ����
        bool hasWeapon = WeaponManager.Instance.currentWeapon.type == WeaponType.Staff;

        if (anim == null)
        {
            Debug.Log($"{user} �� Animator�� �������� ����.");
            return false;
        }
        else if (!hasWeapon)
        {
            Debug.Log($"������ ����δ� ��ų�� ����� �� �����ϴ�.");
            return false;
        }
        else
        {
            // �ִϸ��̼� ����
            anim.SetTrigger("Skill");
            anim.SetInteger("SkillId", data.AnimId);

            if (cachedEffect == null)
            {
                // ������ ����Ʈ�� ������ ����
                cachedEffect = UnityEngine.Object.Instantiate(effectPrefab,
                user.transform.position + user.transform.forward * 3f,
                user.transform.rotation, SkillManager.Instance.gameObject.transform);
            }
            else
            {
                // ������ ����Ʈ�� ������ ���ο� ��ġ ����
                cachedEffect.transform.position = user.transform.position + user.transform.forward * 2f;
                cachedEffect.transform.rotation = user.transform.rotation;
            }

            cachedEffect.SetActive(true);

            return true;
        }
    }
}

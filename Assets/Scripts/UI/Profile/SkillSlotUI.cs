using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        SkillSlotUI
          
            - �÷��̾� ��ų������ ��ų���� �� ��ų���
             
*/

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] string skillId;            // ������ ��ų ID
    private Skill skill;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        SkillData skillData = SkillManager.Instance.GetSkillDataById(skillId);
        skill = SkillManager.Instance.CreateSkillInstance(skillData);
        skill.InitAnimator(GameManager.Instance.player.gameObject);
    }

    public void UseSkill()
    {
        if (skill != null)
            skill.Activate(GameManager.Instance.player.gameObject);
    }
}

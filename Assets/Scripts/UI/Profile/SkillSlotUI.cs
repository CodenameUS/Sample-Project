using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        SkillSlotUI
          
            - 플레이어 스킬슬롯의 스킬설정 및 스킬사용
             
*/

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] string skillId;            // 슬롯의 스킬 ID
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        Skill
          
            - ��ų�� ���������� ������ ������ ����
            
            - InitAnimator() : ��ų�����ü�� �ִϸ����� ĳ��
            
            - Activate() : ��ų ���
                - ���� ��ųŬ�������� ����
*/
public abstract class Skill
{
    protected SkillData data;
    protected Animator anim;

    public Skill(SkillData data)
    {
        this.data = data;
    }

    // �ִϸ��̼� ĳ��
    public void InitAnimator(GameObject user)
    {
        anim = user.GetComponent<Animator>();
    }

    public abstract bool Activate(GameObject user);
}
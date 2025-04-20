using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                        Skill
          
            - 스킬이 공통적으로 가지는 데이터 관리
            
            - InitAnimator() : 스킬사용주체의 애니메이터 캐싱
            
            - Activate() : 스킬 사용
                - 개별 스킬클래스에서 구현
*/
public abstract class Skill
{
    protected SkillData data;
    protected Animator anim;

    public Skill(SkillData data)
    {
        this.data = data;
    }

    // 애니메이션 캐싱
    public void InitAnimator(GameObject user)
    {
        anim = user.GetComponent<Animator>();
    }

    public abstract bool Activate(GameObject user);
}
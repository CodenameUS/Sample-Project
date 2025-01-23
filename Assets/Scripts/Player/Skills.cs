using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    private bool skill1Keydown;
    readonly private int hashSkillType = Animator.StringToHash("SkillType");
    readonly private int hashSkillTrigger = Animator.StringToHash("Skill");

    private Animator anim;

    public int AttackType
    {
        get => anim.GetInteger(hashSkillType);
        set => anim.SetInteger(hashSkillType, value);
    }

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        GetInput();
    }

    private void Init()
    {
        anim = GetComponent<Animator>();
    }
    private void GetInput()
    {
        skill1Keydown = Input.GetButtonDown("Skill1");
    }
}

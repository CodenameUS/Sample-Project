using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

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
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    private void Buff()
    {
       
    }
}

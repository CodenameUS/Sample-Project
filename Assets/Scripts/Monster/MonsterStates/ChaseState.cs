using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                Monster State - Chase (�߰ݻ���)

        - �߰� �ִϸ��̼� ����
 */
public class ChaseState<T> : BaseState<T> where T : Monster
{
    public ChaseState(T monster) : base(monster) { }

    public override void OnStateEnter()
    {
        if (monster.Anim != null) monster.Anim.SetBool("Walk", true);

    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}

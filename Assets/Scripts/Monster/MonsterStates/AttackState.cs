using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                Monster State - Attack (���ݻ���)

        - ���� �ִϸ��̼� ����
 */

public class AttackState<T> : BaseState<T> where T : Monster
{
    public AttackState(T monster) : base(monster) { }

    public override void OnStateEnter()
    {

    }

    public override void OnStateUpdate()
    {

    }

    public override void OnStateExit()
    {

    }
}

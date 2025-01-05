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
        monster.Anim.SetBool("Walk", true);

    }

    public override void OnStateUpdate()
    {
        // Target �߰�
        monster.Nav.SetDestination(monster.Target.transform.position);

        float distanceToPlayer = Vector3.Distance(monster.transform.position, monster.Target.transform.position);

        // �����Ÿ��̻� �������� �����ڸ��� ����
        if (distanceToPlayer > monster.maxDistance)
        {
            monster.Nav.SetDestination(monster.startPosition);

            if(Vector3.Distance(monster.transform.position, monster.startPosition) <= monster.idleThreshold)
            {
                monster.isReset = true;
            }
        }

        

    }

    public override void OnStateExit()
    {
        monster.Anim.SetBool("Walk", false);
    }
}

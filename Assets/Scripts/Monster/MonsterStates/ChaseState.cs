using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                Monster State - Chase (추격상태)

        - 추격 애니메이션 설정
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
        // Target 추격
        monster.Nav.SetDestination(monster.Target.transform.position);

        float distanceToPlayer = Vector3.Distance(monster.transform.position, monster.Target.transform.position);

        // 일정거리이상 벌어지면 원래자리로 복귀
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

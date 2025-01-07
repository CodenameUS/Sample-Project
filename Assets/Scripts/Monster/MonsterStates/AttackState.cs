using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                Monster State - Attack (공격상태)

        - 
 */

public class AttackState<T> : BaseState<T> where T : Monster
{
    public AttackState(T monster) : base(monster) { }

    public override void OnStateEnter()
    {
        monster.Anim.SetBool("Walk", false);
    }

    public override void OnStateUpdate()
    {
        // 위치 고정
        monster.Nav.SetDestination(monster.transform.position);

        // 플레이어 바라보기
        monster.transform.LookAt(monster.Target.transform);

        // 공격준비가 되었을 때
        if(monster.isAttackReady)
        {
            monster.Anim.SetTrigger("Attack");

            // 공격성공시 처리


            // 공격속도에 따른 공격가능여부 설정
            monster.isAttackReady = false;
            monster.Invoke(nameof(monster.ReadyToAttack), monster.attackDelay);
        }
    }

    public override void OnStateExit()
    {

    }


}

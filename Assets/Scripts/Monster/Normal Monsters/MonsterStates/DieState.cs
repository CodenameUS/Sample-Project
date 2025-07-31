using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
                            << DieState >>

        - Die 상태 : 죽음 애니메이션 실행

        - 히트박스 제거, 일정시간 후 몬스터 오브젝트 파괴
 */

public class DieState<T> : BaseState<T> where T : Monster
{
    public DieState(T monster) : base(monster) { }

    public override void OnStateEnter()
    {
        // 죽음 애니메이션 실행
        if(GameManager.Instance.isMultiPlaying)
        {
            monster.photonView.RPC(nameof(monster.RPC_TriggerDieAnim), Photon.Pun.RpcTarget.All);
        }
        else
        {
            monster.TriggerDieAnim();
        }

        monster.Anim.SetBool("Walk", false);

        // 몬스터 히트박스 제거
        monster.HitBox.enabled = false;

        // 몬스터 오브젝트 제거
        if (GameManager.Instance.isMultiPlaying)
        {
            monster.photonView.RPC(nameof(monster.RPC_DeactiveGameObject), Photon.Pun.RpcTarget.All);
        }
        else
        {
            monster.Invoke(nameof(monster.DeactiveGameObject), 3);
        }
    }

    public override void OnStateUpdate()
    {
        monster.Nav.SetDestination(monster.transform.position);
    }

    public override void OnStateExit()
    {

    }
}

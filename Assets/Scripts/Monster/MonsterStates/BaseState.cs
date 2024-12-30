using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
            여러가지 상태를 구현하기위한 추상클래스
        - OnStateEnter() : 상태에 처음 진입했을 때 한 번만 호출(초기설정)
        - OnStateUpdate() : 매 프레임마다 호출
        - OnStateExit() : 상태 변경시 호출(마무리작업)
 */

public abstract class BaseState 
{
    protected Monster monster;

    protected BaseState(Monster enemy)
    {
        monster = enemy;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}

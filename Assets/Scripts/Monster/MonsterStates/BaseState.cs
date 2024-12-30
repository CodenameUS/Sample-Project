using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
            �������� ���¸� �����ϱ����� �߻�Ŭ����
        - OnStateEnter() : ���¿� ó�� �������� �� �� ���� ȣ��(�ʱ⼳��)
        - OnStateUpdate() : �� �����Ӹ��� ȣ��
        - OnStateExit() : ���� ����� ȣ��(�������۾�)
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

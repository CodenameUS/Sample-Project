using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
                NormalAttack : Concrete Attack Strategy - �⺻ ����
            
            - ���� ������ ����
 */
public class NormalAttack : IAttackStrategy
{
    // ����
    public void Attack()
    {
        Debug.Log("�⺻ ���� ����!");
    }
}

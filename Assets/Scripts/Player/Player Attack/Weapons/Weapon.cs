using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*                  
                    Weapon : ���� ���̽� Ŭ����
            - SetAttackStrategy() : ���� ������ �������� ����
            - PerformAttack() : ���� ������ �������� ����
 */
public abstract class Weapon : MonoBehaviour
{
    protected IAttackStrategy attackStrategy;
    public WeaponType type;

    // ���� ����
    public void SetAttackStrategy(IAttackStrategy nextStrategy)
    {
        attackStrategy = nextStrategy;
    }

    // ���� ����
    public void PerformAttack()
    {
        if(attackStrategy != null)
        {
            attackStrategy.Attack();
        }
        else
        {
            Debug.Log("���� ������ �������� �ʾ���.");
        }
    }
}

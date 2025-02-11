using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*                  
                    Weapon : 무기 베이스 클래스
            - SetAttackStrategy() : 공격 전략을 동적으로 변경
            - PerformAttack() : 현재 설정된 전략으로 공격
 */
public abstract class Weapon : MonoBehaviour
{
    protected IAttackStrategy attackStrategy;
    public WeaponType type;

    // 전략 설정
    public void SetAttackStrategy(IAttackStrategy nextStrategy)
    {
        attackStrategy = nextStrategy;
    }

    // 공격 수행
    public void PerformAttack()
    {
        if(attackStrategy != null)
        {
            attackStrategy.Attack();
        }
        else
        {
            Debug.Log("공격 전략이 설정되지 않았음.");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*                  
                    Weapon : 무기 베이스 클래스
            - Attack() : 현재 무기로 공격
 */
public abstract class Weapon : MonoBehaviour
{
    public WeaponType type;

    // 데미지 등
    public abstract void Attack();

    protected abstract void OnTriggerEnter(Collider other);
  
    // 공격판정
    public abstract void SetHitBox(bool isEnabled);
}

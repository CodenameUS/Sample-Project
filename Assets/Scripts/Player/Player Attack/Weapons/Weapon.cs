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
  
    // 공격 판정 On/Off
    public abstract void SetHitBox(bool isEnabled);

    // 이펙트 On/Off
    public abstract void SetEffect(bool isEnabled);
}

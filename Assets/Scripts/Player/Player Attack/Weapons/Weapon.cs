using UnityEngine;

/*
                    Weapon : 무기 공통 부모클래스
                        - Punch
                        - Sword
                        - Staff
                        - Bow
 */
public abstract class Weapon : MonoBehaviour
{
    public WeaponType type;             // 무기타입
    protected string soundId;           // 공격사운드ID

    // 공격 판정 (레이캐스트 방식)
    public abstract void Attack();
  
    // 공격 판정 ON/OFF (Collider 방식)
    public abstract void Attack(bool isEnabled);

    // 이펙트 On/Off
    public abstract void SetEffect(bool isEnabled);

    // 공격 효과음 실행
    public abstract void PlayerSfx();
}

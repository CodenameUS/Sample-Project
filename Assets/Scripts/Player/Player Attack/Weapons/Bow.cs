using UnityEngine;

/*
                            << Punch >>
        - 미구현
        - 공격판정 : Projectile 방식
        - 싱글/멀티 데미지 처리 분리
 */

public class Bow : Weapon
{
    private void Awake()
    {
        type = WeaponType.Bow;
    }

    public override void Attack()
    {
        // 화살 생성
    }

    public override void Attack(bool isEnabled)
    {
        
    }

    public override void SetEffect(bool isEnabled)
    {
        
    }

    public override void PlayerSfx()
    {
        
    }
}

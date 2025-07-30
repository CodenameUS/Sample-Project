using UnityEngine;

/*
                            << Staff >>

        - 공격판정 : Raycast 방식
        - 싱글/멀티 데미지 처리 분리
 */

public class Staff : Weapon
{
    private float attackRange = 3f;             // 공격 범위
    private float attackRadius = 3f;            // 공격 반경 

    private Vector3 attackOrigin;               // 공격 시작위치
    private Vector3 attackDir;                  // 공격 방향

   

    private void Awake()
    {
        type = WeaponType.Staff;

        soundId = "Staff";
    }

    // 공격판정(Raycast 방식)
    public override void Attack()
    {
        // 공격시작 위치 : 플레이어약간앞, 공격방향 : 플레이어 정면
        attackOrigin = GameManager.Instance.player.transform.position + GameManager.Instance.player.transform.forward * 2f;
        attackDir = GameManager.Instance.player.transform.forward;
        
        // 공격
        RaycastHit[] hits = Physics.SphereCastAll(
            attackOrigin,
            attackRadius,
            attackDir,
            attackRange,
            LayerMask.GetMask("Monster", "BossMonster")
        );


        // 1. 멀티모드일때
        if (GameManager.Instance.isMultiPlaying)
        {
            foreach (RaycastHit hit in hits)
            {
                float randomDamage = DataManager.Instance.GetPlayerData().Damage * Random.Range(0.8f, 1f);

                // 일반몬스터 처리
                if (hit.collider.TryGetComponent<Monster>(out var monster))
                {
                    monster.photonView.RPC(nameof(monster.GetDamaged), Photon.Pun.RpcTarget.All, randomDamage);
                }

                // 보스몬스터 처리
                if (hit.collider.TryGetComponent<BossMonster>(out var boss))
                {
                    boss.photonView.RPC(nameof(boss.GetDamaged), Photon.Pun.RpcTarget.All, randomDamage);
                }
            }
        }
        // 2. 싱글모드일때
        else
        {
            foreach (RaycastHit hit in hits)
            {
                float randomDamage = DataManager.Instance.GetPlayerData().Damage * Random.Range(0.8f, 1f);

                // 일반몬스터 처리
                if (hit.collider.TryGetComponent<Monster>(out var monster))
                {
                    monster.GetDamaged(randomDamage);
                }

                // 보스몬스터 처리
                if (hit.collider.TryGetComponent<BossMonster>(out var boss))
                {
                    boss.GetDamaged(randomDamage);
                }
            }
        }
    }

    public override void Attack(bool isEnabled)
    {
        
    }

    public override void SetEffect(bool isEnabled)
    {
        
    }

    public override void PlayerSfx()
    {
        AudioManager.Instance.PlaySFX(soundId);
    }

    // 공격범위 시각화
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackOrigin, attackOrigin + attackDir * attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackOrigin + attackDir * attackRange, attackRadius);
    }

    
}

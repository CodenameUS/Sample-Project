using UnityEngine;

/*
                            << Punch >>

        - 공격판정 : Raycast 방식
        - 싱글/멀티 데미지 처리 분리
 */

public class Punch : Weapon
{
    private float attackRange = 2f;                         // 공격 사거리
    private int maxComboCount = 1;

    private Vector3 boxSize = new Vector3(0.8f, 2f, 0.8f);
    private Vector3 attackOrigin;                             
    private Vector3 attackDir;


    private void Awake()
    {
        // 무기 타입 설정
        type = WeaponType.None;
        soundId = "Punch";
    }

    private  void OnTriggerEnter(Collider other)
    {
        
    }

    // 공격판정(Raycast 방식)
    public override void Attack()
    {
        SetComboCount();

        attackOrigin = GameManager.Instance.player.transform.position + GameManager.Instance.player.transform.up;
        attackDir = GameManager.Instance.player.transform.forward;

        RaycastHit[] hits = Physics.BoxCastAll(
            attackOrigin,                       // 중심위치 : 플레이어
            boxSize,                            // 박스크기
            attackDir,                          // 공격방향     
            Quaternion.identity,                // 회전X
            attackRange,                        // 공격최대거리
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

    // ComboCount 설정
    private void SetComboCount()
    {
        if (GameManager.Instance.player.CurComboCount < maxComboCount)
            GameManager.Instance.player.CurComboCount++;
        else
            GameManager.Instance.player.CurComboCount = 0;
    }

    // 공격범위 시각화
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackOrigin, boxSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(attackOrigin, attackOrigin + attackDir * attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attackOrigin + attackDir * attackRange, boxSize);
    }
}

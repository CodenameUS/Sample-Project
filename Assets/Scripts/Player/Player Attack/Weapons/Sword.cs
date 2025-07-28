using UnityEngine;

/*
                            << Sword >>

        - 공격판정 : Collider 트리거 방식
        - 싱글/멀티 데미지 처리 분리
 */

public class Sword : Weapon
{
    private BoxCollider hitBox;                 // 공격 판정
    private TrailRenderer effect;               // 공격 이펙트

    private void Awake()
    {
        // 무기 타입 설정
        type = WeaponType.Sword;

        hitBox = GetComponent<BoxCollider>();
        effect = GetComponentInChildren<TrailRenderer>();
        soundId = "Sword";
    }
    
    // 공격판정(Collider 트리거)
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monster") && !other.CompareTag("BossMonster"))
            return;

        float randomDamage = randomDamage = DataManager.Instance.GetPlayerData().Damage * Random.Range(0.8f, 1f);

        // 1. 멀티모드일때
        if (GameManager.Instance.isMultiPlaying)
        {
            // 일반몬스터 처리
            if (other.TryGetComponent<Monster>(out var monster))
            {
                monster.photonView.RPC(nameof(monster.GetDamaged), Photon.Pun.RpcTarget.All, randomDamage);
            }
            // 보스몬스터 처리
            else if (other.TryGetComponent<BossMonster>(out var boss))
            {
                boss.photonView.RPC(nameof(boss.GetDamaged), Photon.Pun.RpcTarget.All, randomDamage);
            }
        }
        // 2. 싱글모드일때
        else if (!GameManager.Instance.isMultiPlaying)
        {
            if (other.TryGetComponent<Monster>(out var monster))
            {
                monster.GetDamaged(randomDamage);
            }
            else if (other.TryGetComponent<BossMonster>(out var boss))
            {
                boss.GetDamaged(randomDamage);
            }
        }
    }

    public override void Attack()
    {

    }

    public override void Attack(bool isEnabled)
    {
        hitBox.enabled = isEnabled;
    }

    public override void SetEffect(bool isEnabled)
    {
        effect.enabled = isEnabled;
    }

    public override void PlayerSfx()
    {
        AudioManager.Instance.PlaySFX(soundId);
    }
}

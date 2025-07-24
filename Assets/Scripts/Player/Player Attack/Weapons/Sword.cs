using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/*
                    Sword : 무기(검) 클래스

            - Collider를 사용해서 공격판정 구현
            - SetHitBox() : Collider On/Off - 애니메이션 이벤트에 사용
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

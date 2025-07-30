using UnityEngine;

/*
                            << StayHitbox >>

        - 지속공격형 스킬의 히트 구현
            - 공격 간격(attackInterval)마다 데미지 발생
 */

public class StayHitbox : MonoBehaviour
{
    public float damage;
    public float attackInterval;             // 공격 간격(0.5초)
    public float duration;

    private float lifeTimer = 0f;
    private float attackTimer = 0f;

    private void OnEnable()
    {
        lifeTimer = 0f;
        attackTimer = 0f;
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if(lifeTimer >= duration)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Monster") && !other.CompareTag("BossMonster"))
            return;

        attackTimer += Time.deltaTime;

        // 1. 멀티모드일때
        if(GameManager.Instance.isMultiPlaying)
        {
            if (attackTimer >= attackInterval)
            {
                if(other.TryGetComponent<Monster>(out var monster))
                {
                    monster.photonView.RPC(nameof(monster.GetDamaged), Photon.Pun.RpcTarget.All, damage);
                }
                else if(other.TryGetComponent<BossMonster>(out var boss))
                {
                    boss.photonView.RPC(nameof(boss.GetDamaged), Photon.Pun.RpcTarget.All,
                        damage);
                }

                attackTimer = 0;
            }
        }
        else if(!GameManager.Instance.isMultiPlaying)
        {
            if (attackTimer >= attackInterval)
            {
                if (other.TryGetComponent<Monster>(out var monster))
                {
                    monster.GetDamaged(damage);
                }
                else if (other.TryGetComponent<BossMonster>(out var boss))
                {
                    boss.GetDamaged(damage);
                }

                attackTimer = 0;
            }
        }
        /*
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();

            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                if (GameManager.Instance.isMultiPlaying)
                {
                    monster.photonView.RPC(nameof(monster.GetDamaged), Photon.Pun.RpcTarget.All,
                        damage);
                }
                else
                {
                    monster.GetDamaged(damage);

                }
                timer = 0;
            }
        }
        else if(other.CompareTag("BossMonster"))
        {
            BossMonster boss = other.GetComponent<BossMonster>();
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                if (GameManager.Instance.isMultiPlaying)
                {
                    boss.photonView.RPC(nameof(boss.GetDamaged), Photon.Pun.RpcTarget.All,
                        damage);
                }
                else
                {
                    boss.GetDamaged(damage);

                }
                timer = 0;
            }
        }
        */
    }
}

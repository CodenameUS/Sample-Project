using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
                        IceShot
          
            - 마법스킬
             
*/

public class IceShot : Skill
{
    public IceShot(SkillData data) : base(data) { }

    // 스킬 사용
    public override bool Activate(GameObject user)
    {
        // 올바른 무기를 장착했는지 여부
        bool hasWeapon = WeaponManager.Instance.currentWeapon.type == WeaponType.Staff;

        if (anim == null)
        {
            Debug.Log($"{user} 의 Animator가 존재하지 않음.");
            return false;
        }
        else if (!hasWeapon)
        {
            Debug.Log($"장착한 무기로는 스킬을 사용할 수 없습니다.");
            return false;
        }
        else
        {
            // 애니메이션 실행
            if (GameManager.Instance.isMultiPlaying)
                player.GetComponent<PhotonView>()?.RPC(nameof(player.RPC_TriggerSkillAnim), RpcTarget.All, data.AnimId);
            else
                player.TriggerSkillAnim(data.AnimId);

            // 이펙트 실행
            if (cachedEffect == null)
            {
                // 생성된 이펙트가 없으면 생성
                cachedEffect = UnityEngine.Object.Instantiate(effectPrefab,
                user.transform.position + user.transform.forward * 3f,
                user.transform.rotation, SkillManager.Instance.gameObject.transform);
            }
            else
            {
                // 생성된 이펙트가 있으면 새로운 위치 지정
                cachedEffect.transform.position = user.transform.position + user.transform.forward * 2f;
                cachedEffect.transform.rotation = user.transform.rotation;
            }

            cachedEffect.SetActive(true);
            SkillManager.Instance.StartCoroutine(EnableHitbox(cachedEffect));
            AudioManager.Instance.PlaySFX(data.Name);
            return true;
        }
    }

    // 공격판정
    private IEnumerator EnableHitbox(GameObject effect)
    {
        effect.TryGetComponent<StayHitbox>(out StayHitbox hitbox);
        if (hitbox == null)
        {
            float randomDamage = data.Damage + Random.Range(DataManager.Instance.GetPlayerData().Damage * 0.1f, DataManager.Instance.GetPlayerData().Damage * 0.2f);
            hitbox = effect.AddComponent<StayHitbox>();
            hitbox.damage = randomDamage;

            // 지속시간 3초
            yield return new WaitForSeconds(3f);

            effect.SetActive(false);
        }
    }
}

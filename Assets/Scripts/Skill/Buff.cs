using System.Collections;
using UnityEngine;
using Photon.Pun;

/*
                            << Slash >>

        - Buff 스킬데이터 기반으로 스킬 실행
            - 애니메이션, 스킬이펙트, 공격사운드 플레이
            - FollowTarget 클래스로 스킬 이펙트가 사용자를 따라다니게

        - 지속시간동안 공격력 및 방어력 증가
 */


public class Buff : Skill, IBuffSkill
{
    public Buff(SkillData data) : base(data) { }

    // 스킬 사용
    public override bool Activate(GameObject user)
    {
        // 무기를 장착했는지 여부
        bool hasWeapon = WeaponManager.Instance.currentWeapon != null;

        if (anim == null)
        {
            Debug.Log($"{user} 의 Animator가 존재하지 않음.");
            return false;
        }
        else if (!hasWeapon)
        {
            Debug.Log($"장착한 무기가 없습니다.");
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
                cachedEffect = UnityEngine.Object.Instantiate(effectPrefab, SkillManager.Instance.gameObject.transform);

                FollowingEffect(user);
            }
            else
            {
                // 생성된 이펙트가 있으면 새로운 위치 지정
                cachedEffect.transform.position = user.transform.position;
                cachedEffect.transform.rotation = user.transform.rotation;
            }

            cachedEffect.SetActive(true);

            // 버프 효과 적용
            SkillManager.Instance.StartCoroutine(EnhanceStatus());
            // 사운드 출력
            AudioManager.Instance.PlaySFX(data.Name, 0.3f);             

            return true;
        }
    }

    // 버프 이펙트가 플레이어를 따라다니도록
    public void FollowingEffect(GameObject user)
    {
        FollowTarget follow = cachedEffect.AddComponent<FollowTarget>();
        follow.target = user.transform;
        follow.duration = data.Cooldown / 2;
    }

    // 버프 : 능력치 상승
    private IEnumerator EnhanceStatus()
    {
        // 공격력, 방어력 증가
        DataManager.Instance.GetPlayerData().Defense += data.Damage;
        DataManager.Instance.GetPlayerData().Damage += data.Damage;

        // 지속시간
        yield return new WaitForSeconds(data.Cooldown / 2);

        // 공격력, 방어력 복구
        DataManager.Instance.GetPlayerData().Defense -= data.Damage;
        DataManager.Instance.GetPlayerData().Damage -= data.Damage;
    }
}

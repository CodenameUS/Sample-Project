using UnityEngine;

/*
                            << IBuffSkill >>

        - 버프 스킬용 인터페이스

        - FollowingEffect() : 스킬 이펙트가 사용주체를 따라다니도록
 */

public interface IBuffSkill
{
    void FollowingEffect(GameObject user);
}

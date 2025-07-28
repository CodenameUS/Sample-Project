using UnityEngine;

/*
                            << Skill >>

        - 스킬 공통 데이터 관리
        
        - InitAnimator() : 스킬사용주체의 애니메이터 캐싱
        
        - SetEffect : 이펙트 프리팹 캐싱

        - Activate() : 스킬 사용
            - 스킬 사용 성공 여부 반환
            - 개별 스킬클래스에서 세부 구현
 */

public abstract class Skill
{
    protected SkillData data;                       // 스킬데이터
    protected Animator anim;                        // 스킬사용 주체의 애니메이터
    protected GameObject effectPrefab;              // 스킬이펙트 프리팹
    protected PlayerController player;              // 플레이어

    public GameObject cachedEffect;                 // 캐싱된 이펙트 오브젝트

    public Skill(SkillData data)
    {
        this.data = data;
    }

    // 애니메이션 캐싱
    public void CachingData(GameObject user)
    {
        anim = user.GetComponent<Animator>();
        player = user.GetComponent<PlayerController>();
    }

    // 이펙트 프리팹 저장
    public void SetEffect(GameObject effect)
    {
        effectPrefab = effect;
    }

    // 스킬 사용
    public abstract bool Activate(GameObject user);
}
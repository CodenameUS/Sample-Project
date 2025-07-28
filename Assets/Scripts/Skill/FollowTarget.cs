using System.Collections;
using UnityEngine;

/*
                            << FollowTarget >>

        - 스킬 이펙트가 타깃을 따라다니게하는 클래스
 */

public class FollowTarget : MonoBehaviour
{
    public Transform target;                // 타깃
    public float duration;                  // 지속시간

    // 지속시간이 지나면 파괴
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (target != null)
            transform.position = target.position;
    }
}

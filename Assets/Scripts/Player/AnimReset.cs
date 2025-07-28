using UnityEngine;

/*
                            << AnimReset >>

        - 의도치않은 애니메이션 중복 방지
        - Animator의 특정 상태에 부착하여 사용
 */

public class AnimReset : StateMachineBehaviour
{
    [SerializeField] private string triggerName;            // reset할 트리거

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerName);
    }
}

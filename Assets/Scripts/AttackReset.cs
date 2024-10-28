using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReset : StateMachineBehaviour
{
    [SerializeField] private string triggerName;            // reset할 트리거

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerName);
    }
}

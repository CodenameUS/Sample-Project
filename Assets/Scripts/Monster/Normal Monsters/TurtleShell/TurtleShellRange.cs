using UnityEngine;

/*
                            << TurtleShellRange >>

        - Idle 상태에서 플레이어가 Range에 접근시 Chase상태로 돌입하기위한 Collider 이벤트
 */

public class TurtleShellRange : MonoBehaviour
{
    [SerializeField] private SphereCollider scanRange;              // 적 탐지 범위
    private  TurtleShell parent;

    private void Awake()
    {
        parent = GetComponentInParent<TurtleShell>();
    }

    // Range에 플레이어가 들어왔을경우 Chase 상태 돌입
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if(parent.curState == TurtleShell.States.Idle)
            parent.ChangeState(TurtleShell.States.Chase);
    }
}

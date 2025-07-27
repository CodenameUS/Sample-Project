/*
                            << StateMachine >>

        - 몬스터 현재 상태 설정 및 다음 상태로의 전환 수행

        - 상태에서의 동작(OnStateEnter, OnStateUpdate, OnStateExit) 수행
*/

public class StateMachine<T> where T : Monster
{
    public BaseState<T> curState { get; set; }

    public StateMachine(BaseState<T> init)
    {
        // 현재상태 초기화
        curState = init;
        curState.OnStateEnter();
    }

    // 상태 변경
    public void ChangeState(BaseState<T> nextState)
    {
        // 다른상태로만 변경
        if (nextState == curState)
            return;

        // 현재상태 탈출
        if (curState != null)
            curState.OnStateExit();

        // 현재상태를 nextState로 설정
        curState = nextState;
        curState.OnStateEnter();
    }

    public void UpdateState()
    {
        if (curState != null)
            curState.OnStateUpdate();
    }
}

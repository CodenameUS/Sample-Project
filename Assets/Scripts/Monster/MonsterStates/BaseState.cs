/*
                            << BaseState >>

        - 몬스터의 상태 구현을 위한 추상클래스
        
        - OnStateEnter() : 상태에 처음 진입했을 때 한 번만 호출(초기설정)
        - OnStateUpdate() : Update 함수에서 매 프레임마다 호출(상태동안 진행할 동작)
        - OnStateExit() : 상태 변경시 호출(마무리, 복원 처리)
 */

public abstract class BaseState<T> where T : Monster
{
    protected T monster;
   
    protected BaseState(T monster)
    {
        this.monster = monster;
    }
    
    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}

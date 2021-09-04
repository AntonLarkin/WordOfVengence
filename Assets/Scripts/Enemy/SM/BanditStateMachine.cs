public class BanditStateMachine 
{
    private BanditState currentState;

    public void UpdateState()
    {
        currentState?.OnUpdate();
    }

    public void SetState(BanditState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}

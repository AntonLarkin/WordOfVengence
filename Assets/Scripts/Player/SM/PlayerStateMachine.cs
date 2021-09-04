public class PlayerStateMachine 
{
    private PlayerState currentState;

    public void UpdateState()
    {
        currentState?.OnUpdate();
    }

    public void SetState(PlayerState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public PlayerState ShowCurrentsState()
    {
        return currentState;
    }
}

public abstract class PlayerState 
{
    protected readonly Player player;
    protected readonly PlayerStateMachine stateMachine;

    protected PlayerState(Player player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public abstract void OnUpdate();
    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }
}

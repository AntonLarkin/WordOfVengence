public abstract class BanditState 
{
    protected readonly BaseBandit bandit;
    protected readonly BanditStateMachine stateMachine;

    protected BanditState(BaseBandit bandit,BanditStateMachine stateMachine)
    {
        this.bandit = bandit;
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

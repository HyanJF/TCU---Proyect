public class BotStateMachine
{
    private IBotState currentState;
    private BotController bot;

    public BotStateMachine(BotController bot)
    {
        this.bot = bot;
    }

    public void ChangeState(IBotState newState)
    {
        currentState?.Exit();

        currentState = newState;

        currentState.Enter(bot);
    }

    public void Update()
    {
        currentState?.Update();
    }

    public bool IsInState<T>() where T : IBotState
    {
        return currentState is T;
    }
}
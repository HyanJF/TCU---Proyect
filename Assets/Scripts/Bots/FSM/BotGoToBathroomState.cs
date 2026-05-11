using UnityEngine;

public class BotGoToBathroomState : IBotState
{
    private BotController bot;

    public void Enter(BotController bot)
    {
        this.bot = bot;
    }

    public void Update()
    {
        if (BotBlackboard.Instance.bathroom == null)
        {
            bot.stateMachine.ChangeState(new BotThinkState());
            return;
        }

        Vector2 target = BotBlackboard.Instance.bathroom.position;

        bot.movement.MoveTo(target);

        if (bot.movement.Reached(target))
        {
            bot.actions.HideBot();

            bot.stateMachine.ChangeState(new BotUsingWCState());
        }
    }

    public void Exit()
    {
        bot.movement.Stop();
    }
}
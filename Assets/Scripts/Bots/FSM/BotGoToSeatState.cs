using UnityEngine;

public class BotGoToSeatState : IBotState
{
    private BotController bot;

    public void Enter(BotController bot)
    {
        this.bot = bot;
    }

    public void Update()
    {
        if (bot.targetSeat == null)
        {
            bot.stateMachine.ChangeState(new BotThinkState());
            return;
        }

        bot.movement.MoveTo(bot.targetSeat.transform.position);

        if (bot.movement.Reached(bot.targetSeat.transform.position))
        {
            SeatManager.Instance.OccupySeat(bot.targetSeat, bot.gameObject);

            bot.actions.Drink();

            bot.movement.Stop();
        }
    }

    public void Exit()
    {
        bot.movement.Stop();
    }
}
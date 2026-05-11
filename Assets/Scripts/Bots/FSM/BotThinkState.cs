using UnityEngine;

public class BotThinkState : IBotState
{
    private BotController bot;

    private float timer = 1f;

    public void Enter(BotController bot)
    {
        this.bot = bot;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Decide();
        }
    }

    void Decide()
    {
        BotNeed need = bot.brain.DecideNeed();

        switch (need)
        {
            case BotNeed.Thirst:
                HandleThirst();
                return;

            case BotNeed.Bladder:
                bot.stateMachine.ChangeState(new BotGoToBathroomState());
                return;

            case BotNeed.Comfort:
                bot.stateMachine.ChangeState(new BotWanderState());
                return;

            case BotNeed.Social:
                bot.stateMachine.ChangeState(new BotWanderState());
                return;

            default:
                bot.stateMachine.ChangeState(new BotWanderState());
                return;
        }
    }

    void HandleThirst()
    {
        if (!SeatManager.Instance.HasFreeSeat(BotBlackboard.Instance.seats))
        {
            bot.stateMachine.ChangeState(new BotWanderState());
            return;
        }

        bot.targetSeat =
            SeatManager.Instance.GetFreeSeat(BotBlackboard.Instance.seats);

        if (bot.targetSeat != null)
        {
            bot.stateMachine.ChangeState(new BotGoToSeatState());
        }
    }

    public void Exit()
    {

    }
}
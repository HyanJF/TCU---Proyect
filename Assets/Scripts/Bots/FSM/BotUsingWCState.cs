using UnityEngine;

public class BotUsingWCState : IBotState
{
    private BotController bot;

    private float timer;

    public void Enter(BotController bot)
    {
        this.bot = bot;

        BotNeeds value = bot.needs;

        float bladderValue = value.bladder;

        timer = Mathf.Clamp(bladderValue * 0.1f, 1f, 10f);

        bot.actions.UseBathroom();
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            ExitBathroom();
        }
    }

    void ExitBathroom()
    {
        bot.actions.ShowBot();

        if (BotBlackboard.Instance.exitBathroom != null)
        {
            bot.transform.position =
                BotBlackboard.Instance.exitBathroom.position;
        }

        bot.stateMachine.ChangeState(new BotThinkState());
    }

    public void Exit()
    {

    }
}
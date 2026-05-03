using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BotUsingWCState : IBotState
{
    private BotController bot;
    private float timer;

    public void Enter(BotController bot)
    {
        Debug.Log("Entrando a UsingWC");
        this.bot = bot;

        bot.movement.SetMovement(Vector2.zero);

        BotStats stats = bot.GetComponent<BotStats>();

        if (stats != null)
        {
            float bladderValue = stats.bladder;

            timer = Mathf.Clamp(bladderValue * 0.1f, 1f, 10f);

            stats.ReduceBladder(bladderValue);

            Debug.Log($"[WC] Tiempo en baño: {timer}");
        }
        else
        {
            timer = 2f;
        }
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
        BotStats stats = bot.GetComponent<BotStats>();

        if (stats != null)
        {
            stats.bathroomVisits++;
        }

        bot.SetBotActiveVisual(true);

        if (BotBlackboard.Instance != null && BotBlackboard.Instance.exitBathroom != null)
        {
            bot.transform.position = BotBlackboard.Instance.exitBathroom.position;
        }

        bot.ChangeState(new BotThinkState());
    }

    public void Exit()
    {
        bot.movement.SetMovement(Vector2.zero);
    }
}
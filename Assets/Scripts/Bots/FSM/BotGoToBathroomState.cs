using UnityEngine;

public class BotGoToBathroomState : IBotState
{
    private BotController bot;

    public void Enter(BotController bot)
    {
        Debug.Log("Entrando a GoToBathroom");
        this.bot = bot;
        bot.ClearPath();
    }

    public void Update()
    {
        if (BotBlackboard.Instance == null || BotBlackboard.Instance.bathroom == null)
        {
            Debug.LogWarning("[Bathroom] No existe bathroom → Wander");
            bot.ChangeState(new BotWanderState());
            return;
        }

        Vector2 target = BotBlackboard.Instance.bathroom.position;

        bot.MoveTo(target);

        if (Vector2.Distance(bot.transform.position, target) < bot.ReachDistance)
        {
            Debug.Log("[Bathroom] Llegó al baño → Desactivar bot");

            BotStats stats = bot.GetComponent<BotStats>();

            bot.SetBotActiveVisual(false);
            bot.ChangeState(new BotUsingWCState());
        }
    }

    public void Exit()
    {
        bot.movement.SetMovement(Vector2.zero);
    }
}
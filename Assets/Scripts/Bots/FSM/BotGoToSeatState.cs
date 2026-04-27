using UnityEngine;

public class BotGoToSeatState : IBotState
{
    private BotController bot;

    public void Enter(BotController bot)
    {
        Debug.Log("Entrando a GoToSeat");
        this.bot = bot;
        bot.ClearPath();
    }

    public void Update()
    {
        if (bot.targetSeat == null)
        {
            Debug.Log("[GoToSeat] No hay asiento → Wander");
            bot.ChangeState(new BotWanderState());
            return;
        }

        bot.MoveTo(bot.targetSeat.transform.position);

        if (Vector2.Distance(bot.transform.position, bot.targetSeat.transform.position) < 1f)
        {
            Debug.Log("[GoToSeat] Llegó al asiento");

            bot.OnReachedSeat();

            bot.ClearPath();
        }
    }

    public void Exit()
    {
        bot.movement.SetMovement(Vector2.zero);
    }
}
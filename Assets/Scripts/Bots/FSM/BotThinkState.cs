using UnityEngine;

public class BotThinkState : IBotState
{
    private BotController bot;
    private float thinkTime = 2f;
    private float timer;

    public void Enter(BotController bot)
    {
        Debug.Log("Entrando a Think");
        this.bot = bot;
        timer = thinkTime;

        bot.movement.SetMovement(Vector2.zero);
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            DecideNextState();
        }
    }

    void DecideNextState()
    {
        bot.ClearPath();

        BotStats stats = bot.GetComponent<BotStats>();

        if (stats == null)
        {
            Debug.LogWarning("No hay BotStats → Wander");
            bot.ChangeState(new BotWanderState());
            return;
        }

        if (BotBlackboard.Instance == null || BotBlackboard.Instance.seats == null)
        {
            Debug.LogWarning("Blackboard o seats NULL → Wander");
            bot.ChangeState(new BotWanderState());
            return;
        }

        // INTERACT
        if (stats.wantsInteract)
        {
            Debug.Log("[THINK] INTERACT → Desactivar bot");
            bot.gameObject.SetActive(false);
            return;
        }

        // THIRST
        if (stats.thirst > 70f)
        {
            bool hasSeats = SeatManager.Instance != null &&
                            SeatManager.Instance.HasFreeSeat(BotBlackboard.Instance.seats);

            if (!hasSeats)
            {
                Debug.Log("[THINK] THIRST pero NO hay asientos → fallback");
            }
            else
            {
                Debug.Log("[THINK] THIRST → GO TO SEAT");

                bot.targetSeat = SeatManager.Instance.GetFreeSeat(BotBlackboard.Instance.seats);

                if (bot.targetSeat != null)
                {
                    bot.ChangeState(new BotGoToSeatState());
                    return;
                }
            }
        }

        // COMFORT
        if (stats.comfort < 30f)
        {
            Debug.Log("[THINK] LOW COMFORT → WANDER");
            bot.ChangeState(new BotWanderState());
            return;
        }

        // BLADDER
        if (stats.bladder > 70f)
        {
            Debug.Log("[THINK] BLADDER → (temporal) WANDER");
            bot.ChangeState(new BotWanderState());
            return;
        }

        // DEFAULT
        Debug.Log("[THINK] DEFAULT → WANDER");
        bot.ChangeState(new BotWanderState());
    }

    public void Exit() { }
}
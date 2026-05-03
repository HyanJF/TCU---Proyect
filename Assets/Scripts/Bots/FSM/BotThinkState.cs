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

        BotPriority priority = bot.GetComponent<BotPriority>();

        if (priority == null)
        {
            Debug.LogWarning("No hay BotPriority → Wander");
            bot.ChangeState(new BotWanderState());
            return;
        }

        if (BotBlackboard.Instance == null)
        {
            Debug.LogWarning("No hay Blackboard → Wander");
            bot.ChangeState(new BotWanderState());
            return;
        }

        BotStats stats = bot.GetComponent<BotStats>();

        if (stats != null)
        {
            if (stats.drinksDone >= 1 && stats.bathroomVisits >= 1)
            { 
                if (BotBlackboard.Instance.seatsTable != null &&
                    SeatManager.Instance != null &&
                    SeatManager.Instance.HasFreeSeat(BotBlackboard.Instance.seatsTable))
                {
                    Debug.Log("[THINK] PROGRESS → GO TO TABLE");

                    bot.targetSeat = SeatManager.Instance.GetFreeSeat(BotBlackboard.Instance.seatsTable);

                    if (bot.targetSeat != null)
                    {
                        bot.ChangeState(new BotGoToSeatState());
                        return;
                    }
                }
            }
        }

        BotNeed need = priority.EvaluateNeeds();

        switch (need)
        {
            case BotNeed.Interact:
                Debug.Log("[THINK] INTERACT → Desactivar bot");
                bot.gameObject.SetActive(false);
                return;

            case BotNeed.Thirst:
                HandleThirst();
                return;

            case BotNeed.Bladder:
                Debug.Log("[THINK] BLADDER → GO TO BATHROOM");
                bot.ChangeState(new BotGoToBathroomState());
                return;

            case BotNeed.Comfort:
                Debug.Log("[THINK] COMFORT → WANDER");
                bot.ChangeState(new BotWanderState());
                return;

            default:
                Debug.Log("[THINK] DEFAULT → WANDER");
                bot.ChangeState(new BotWanderState());
                return;
        }
    }

    void HandleThirst()
    {
        if (BotBlackboard.Instance.seats == null)
        {
            bot.ChangeState(new BotWanderState());
            return;
        }

        bool hasSeats = SeatManager.Instance != null &&
                        SeatManager.Instance.HasFreeSeat(BotBlackboard.Instance.seats);

        if (!hasSeats)
        {
            Debug.Log("[THINK] THIRST pero NO hay asientos → Wander");
            bot.ChangeState(new BotWanderState());
            return;
        }

        Debug.Log("[THINK] THIRST → GO TO SEAT");

        bot.targetSeat = SeatManager.Instance.GetFreeSeat(BotBlackboard.Instance.seats);

        if (bot.targetSeat != null)
        {
            bot.ChangeState(new BotGoToSeatState());
        }
        else
        {
            bot.ChangeState(new BotWanderState());
        }
    }

    public void Exit() { }
}
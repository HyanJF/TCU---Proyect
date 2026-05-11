using UnityEngine;

public class BotWanderState : IBotState
{
    private BotController bot;

    private Vector2 targetPoint;

    public void Enter(BotController bot)
    {
        this.bot = bot;

        PickNewPoint();
    }

    public void Update()
    {
        bot.movement.MoveTo(targetPoint);

        if (bot.movement.Reached(targetPoint))
        {
            bot.needs.ApplyComfort(5f);

            bot.stateMachine.ChangeState(new BotThinkState());
        }
    }

    void PickNewPoint()
    {
        var waypoints = BotBlackboard.Instance.waypoints;

        if (waypoints == null || waypoints.Count == 0)
            return;

        int index = Random.Range(0, waypoints.Count);

        targetPoint = waypoints[index].position;
    }

    public void Exit()
    {
        bot.movement.Stop();
    }
}
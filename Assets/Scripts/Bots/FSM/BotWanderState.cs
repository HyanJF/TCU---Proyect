using UnityEngine;

public class BotWanderState : IBotState
{
    private BotController bot;

    private Vector2 targetPoint;

    public void Enter(BotController bot)
    {
        Debug.Log("Entrando a Wander");
        this.bot = bot;

        PickNewPoint();
        bot.ClearPath();

        Debug.Log($"[WANDER] Nuevo target: {targetPoint}");
        
    }

    public void Update()
    {
        bot.MoveTo(targetPoint);

        if (Vector2.Distance(bot.transform.position, targetPoint) < 1f)
        {
            bot.OnReachedWaypoint();
            bot.ChangeState(new BotThinkState());
        }
    }

    void PickNewPoint()
    {
        var waypoints = BotBlackboard.Instance.waypoints;

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogWarning("No hay waypoints en Blackboard");
            return;
        }

        Transform selected = null;
        int attempts = 10;

        while (attempts > 0)
        {
            int index = Random.Range(0, waypoints.Count);

            if (waypoints[index] != null)
            {
                selected = waypoints[index];
                break;
            }

            attempts--;
        }

        if (selected == null)
        {
            Debug.LogError("Todos los waypoints son NULL");
            return;
        }

        targetPoint = selected.position;
    }

    public void Exit()
    {
        Debug.Log("Saliendo de Wander");
        bot.movement.SetMovement(Vector2.zero);
    }

    public void DebugDraw()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPoint, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(bot.transform.position, targetPoint);
    }
}
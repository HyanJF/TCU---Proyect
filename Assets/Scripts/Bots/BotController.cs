using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    private MovementController movement;

    public GridManager gridManager;
    public Seat targetSeat;

    private List<Node> path;
    private int pathIndex;

    [SerializeField] private float reachDistance = 0.25f;
    [SerializeField] private float repathTime = 0.5f;

    private float repathTimer;

    private void Awake()
    {
        movement = GetComponent<MovementController>();
    }

    private void Start()
    {
        AcquireNewSeat();
    }

    private void Update()
    {
        if (gridManager == null)
            return;

        if (targetSeat == null)
        {
            AcquireNewSeat();
            movement.SetMovement(Vector2.zero);
            return;
        }

        repathTimer -= Time.deltaTime;

        if (repathTimer <= 0f)
        {
            UpdatePath();
            repathTimer = repathTime;
        }

        MoveAlongPath();
    }

    void AcquireNewSeat()
    {
        targetSeat = SeatManager.Instance.GetFreeSeat(BotBlackboard.Instance.seats);
    }

    void UpdatePath()
    {
        if (targetSeat == null) return;

        path = gridManager.FindPath(transform.position, targetSeat.transform.position);
        pathIndex = 0;
    }

    void MoveAlongPath()
    {
        if (path == null || path.Count == 0)
        {
            movement.SetMovement(Vector2.zero);
            return;
        }

        if (pathIndex >= path.Count)
        {
            movement.SetMovement(Vector2.zero);

            // cuando llega al asiento
            SeatManager.Instance.OccupySeat(targetSeat, gameObject);
            return;
        }

        Vector2 target = path[pathIndex].worldPosition;

        if (Vector2.Distance(transform.position, target) < reachDistance)
        {
            pathIndex++;
            return;
        }

        Vector2 dir = (target - (Vector2)transform.position).normalized;
        movement.SetMovement(dir);
    }
}
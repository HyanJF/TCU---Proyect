using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    private IBotState currentState;

    public MovementController movement;
    public GridManager gridManager;
    public float ReachDistance => reachDistance;

    public Seat targetSeat;
    public List<Seat> currentSeatList;

    private List<Node> currentPath;
    private int pathIndex;

    private Vector2 lastTarget;
    private Collider2D[] colliders;
    private Renderer[] renderers;

    [SerializeField] private float reachDistance = 0.25f;

    private void Awake()
    {
        movement = GetComponent<MovementController>();
        colliders = GetComponentsInChildren<Collider2D>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return null;

        ChangeState(new BotWanderState());
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IBotState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(this);
    }

    // MOVER A UN PUNTO
    public void MoveTo(Vector2 target)
    {
        if (gridManager == null)
            return;

        // recalcula si:

        // - no hay path
        // - o cambió el destino
        if (currentPath == null || Vector2.Distance(target, lastTarget) > 0.5f)
        {
            Debug.Log($"[PATH] Nuevo destino: {target}");

            currentPath = gridManager.FindPath(transform.position, target);
            pathIndex = 0;
            lastTarget = target;

            if (currentPath == null)
            {
                Debug.LogWarning("[PATH] No se pudo generar path, moviendo directo");

                Vector2 dir = (target - (Vector2)transform.position).normalized;
                movement.SetMovement(dir);
                return;
            }
            else
            {
                Debug.Log($"[PATH] Path generado con {currentPath.Count} nodos");
            }
        }

        FollowPath();
    }

    void FollowPath()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            movement.SetMovement(Vector2.zero);
            return;
        }

        if (pathIndex >= currentPath.Count)
        {
            movement.SetMovement(Vector2.zero);
            return;
        }

        Vector2 nodeTarget = currentPath[pathIndex].worldPosition;

        // DEBUG
        Debug.DrawLine(transform.position, nodeTarget, Color.green);

        if (Vector2.Distance(transform.position, nodeTarget) < reachDistance)
        {
            pathIndex++;
            return;
        }

        Vector2 dir = (nodeTarget - (Vector2)transform.position).normalized;
        movement.SetMovement(dir);
    }

    public void ClearPath()
    {
        currentPath = null;
        pathIndex = 0;
    }

    private void OnDisable()
    {
        currentState = null;
    }

    // EVENTOS DE ACCIÓN
    public void OnReachedSeat()
    {
        Debug.Log("[EVENT] Reached Seat");

        if (targetSeat == null)
        {
            Debug.LogWarning("[Seat] targetSeat NULL");
            return;
        }

        // VALIDACIÓN EXTRA
        if (targetSeat.state == Seat.SeatState.Occupied)
        {
            Debug.LogWarning("[Seat] Ya estaba ocupado → cancelar");
            targetSeat = null;
            return;
        }

        BotStats stats = GetComponent<BotStats>();

        if (stats != null)
        {
            stats.ReduceThirst(100f);
            stats.drinksDone++;
        }

        SeatManager.Instance.OccupySeat(targetSeat, gameObject);

        ClearPath();
    }

    public void OnReachedWaypoint()
    {
        Debug.Log("[EVENT] Reached Waypoint");

        BotStats stats = GetComponent<BotStats>();

        if (stats != null)
        {
            stats.AddComfort(5f);
           
        }

        ClearPath();
    }

    public bool IsMoving()
    {
        if (movement == null)
            return false;

        return movement.HasMovement();
    }

    public void SetBotActiveVisual(bool active)
    {
        foreach (var col in colliders)
            col.enabled = active;

        foreach (var rend in renderers)
            rend.enabled = active;
    }

    public bool IsInState<T>() where T : IBotState
    {
        return currentState is T;
    }

    // DEBUG VISUAL
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);

        // destino actual del path
        Gizmos.color = Color.purple;
        Gizmos.DrawSphere(lastTarget, 0.5f);

        if (currentState is BotWanderState wander)
        {
            wander.DebugDraw();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    public GridManager gridManager;
    public PathfindingSystem pathfinding;
    public MovementController movementController;

    [SerializeField] private float reachDistance = 0.25f;

    private List<Node> currentPath;
    private int pathIndex;

    private Vector2 lastTarget;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    public void MoveTo(Vector2 target)
    {
        if (gridManager == null)
            return;

        if (currentPath == null ||
            Vector2.Distance(target, lastTarget) > 0.5f)
        {
            GeneratePath(target);
        }

        FollowPath();
    }

    void GeneratePath(Vector2 target)
    {
        currentPath = pathfinding.FindPath(transform.position, target);

        pathIndex = 0;
        lastTarget = target;

        if (currentPath == null)
        {
            Vector2 dir = (target - (Vector2)transform.position).normalized;

            movementController.SetMovement(dir);
        }
    }

    void FollowPath()
    {
        if (currentPath == null || currentPath.Count == 0)
        {
            movementController.SetMovement(Vector2.zero);
            return;
        }

        if (pathIndex >= currentPath.Count)
        {
            movementController.SetMovement(Vector2.zero);
            return;
        }

        Vector2 target = currentPath[pathIndex].worldPosition;

        Debug.DrawLine(transform.position, target, Color.green);

        if (Vector2.Distance(transform.position, target) < reachDistance)
        {
            pathIndex++;
            return;
        }

        Vector2 dir = (target - (Vector2)transform.position).normalized;

        movementController.SetMovement(dir);
    }

    public void Stop()
    {
        currentPath = null;
        pathIndex = 0;

        movementController.SetMovement(Vector2.zero);
    }

    public bool Reached(Vector2 target)
    {
        return Vector2.Distance(transform.position, target) < reachDistance;
    }

    public bool IsMoving()
    {
        return movementController.HasMovement();
    }
}
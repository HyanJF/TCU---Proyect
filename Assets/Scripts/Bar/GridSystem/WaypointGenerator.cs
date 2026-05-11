using System.Collections.Generic;
using UnityEngine;

public class WaypointGenerator : MonoBehaviour
{
    [Header("Waypoint Settings")]
    [Range(0f, 1f)]
    public float waypointChance = 0.35f;

    [Range(1, 5)]
    public int minWaypointDistance = 1;

    public List<Transform> generatedWaypoints =
        new List<Transform>();

    public void GenerateWaypoints(GridManager gridManager)
    {
        generatedWaypoints.Clear();

        foreach (Transform child in transform)
        {
            if (child.name == "Waypoint")
            {
                Destroy(child.gameObject);
            }
        }

        List<Node> waypointNodes = new List<Node>();

        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                Node node = gridManager.grid[x, y];

                if (!node.walkable)
                    continue;

                if (node.isTableZone)
                    continue;

                if (node.movementPenalty > 10)
                    continue;

                if (!IsAreaWalkable(gridManager, x, y, 1))
                    continue;

                if (Random.value > waypointChance)
                    continue;

                if (HasNearbyWaypoint(x, y, waypointNodes))
                    continue;

                waypointNodes.Add(node);

                GameObject wp = new GameObject("Waypoint");

                wp.transform.position = node.worldPosition;
                wp.transform.parent = transform;

                generatedWaypoints.Add(wp.transform);
            }
        }

        Debug.Log($"Waypoints generados: {generatedWaypoints.Count}");

        if (BotBlackboard.Instance != null)
        {
            BotBlackboard.Instance.waypoints = generatedWaypoints;
        }
    }

    bool HasNearbyWaypoint(
        int x,
        int y,
        List<Node> existingWaypoints)
    {
        foreach (Node node in existingWaypoints)
        {
            int dstX = Mathf.Abs(node.gridX - x);
            int dstY = Mathf.Abs(node.gridY - y);

            if (dstX <= minWaypointDistance &&
                dstY <= minWaypointDistance)
            {
                return true;
            }
        }

        return false;
    }

    bool IsAreaWalkable(
        GridManager gridManager,
        int centerX,
        int centerY,
        int radius)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Node node =
                    gridManager.GetNode(centerX + x, centerY + y);

                if (node == null)
                    return false;

                if (!node.walkable)
                    return false;
            }
        }

        return true;
    }
}
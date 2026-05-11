using UnityEngine;

public class GridDebugRenderer : MonoBehaviour
{
    private GridManager gridManager;
    private WaypointGenerator waypointGenerator;
    private SocialZoneGenerator socialZoneGenerator;

    [Header("Debug")]
    public bool drawGrid = true;
    public bool drawWaypoints = true;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
        waypointGenerator = GetComponent<WaypointGenerator>();
        socialZoneGenerator = GetComponent<SocialZoneGenerator>();
    }

    int GetZoneSize(SocialZoneSize type)
{
    switch (type)
    {
        case SocialZoneSize.Small:
            return 3;

        case SocialZoneSize.Medium:
            return 5;

        case SocialZoneSize.Large:
            return 9;
    }

    return 3;
}

    private void OnDrawGizmos()
    {
        if (gridManager == null)
        {
            gridManager = GetComponent<GridManager>();
        }

        if (waypointGenerator == null)
        {
            waypointGenerator = GetComponent<WaypointGenerator>();
        }

        if (gridManager == null)
            return;

        if (drawGrid)
        {
            DrawGrid();
        }

        if (drawWaypoints)
        {
            DrawWaypoints();
        }

        DrawSocialZones();
    }

    void DrawGrid()
    {
        if (gridManager.grid == null)
            return;

        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                Node node = gridManager.grid[x, y];

                if (node == null)
                    continue;

                DrawNode(node);

                Gizmos.DrawWireCube(
                    node.worldPosition,
                    Vector3.one * (gridManager.cellSize * 0.9f)
                );
            }
        }
    }

    void DrawNode(Node node)
    {
        // MESA
        if (node.isTableZone)
        {
            Gizmos.color = Color.blue;
            return;
        }

        // OBSTÁCULO
        if (!node.walkable)
        {
            Gizmos.color = Color.red;
            return;
        }

        // PENALTIES
        float maxPenalty = 50f;

        float t = Mathf.Clamp01(
            node.movementPenalty / maxPenalty
        );

        Gizmos.color = Color.Lerp(
            Color.white,
            Color.yellow,
            t
        );

        if (t > 0.5f)
        {
            Gizmos.color = Color.Lerp(
                Color.yellow,
                new Color(1f, 0.3f, 0f),
                (t - 0.5f) * 2f
            );
        }
    }

    void DrawWaypoints()
    {
        if (waypointGenerator == null)
            return;

        Gizmos.color = Color.yellow;

        foreach (var wp in waypointGenerator.generatedWaypoints)
        {
            if (wp == null)
                continue;

            Gizmos.DrawSphere(
                wp.position,
                0.2f
            );
        }
    }

    void DrawSocialZones()
    {
        if (socialZoneGenerator == null)
            return;

        foreach (var zone in socialZoneGenerator.generatedZones)
        {
            int size =
                GetZoneSize(zone.sizeType);

            Vector3 zoneSize =
                Vector3.one *
                size *
                gridManager.cellSize;

            // COLOR POR TIPO
            switch (zone.sizeType)
            {
                case SocialZoneSize.Small:
                    Gizmos.color = Color.green;
                    break;

                case SocialZoneSize.Medium:
                    Gizmos.color = new Color(0f, 0.7f, 0f);
                    break;

                case SocialZoneSize.Large:
                    Gizmos.color = new Color(0f, 0.4f, 0f);
                    break;
            }

            Gizmos.DrawWireCube(
                zone.center,
                zoneSize
            );
        }
    }
}
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
                socialZoneGenerator.GetZoneSize(
                    zone.sizeType
                );

            float visualSize =
                gridManager.cellSize * 0.9f;

            Vector3 zoneSize =
                Vector3.one *
                size *
                visualSize;

            Color zoneColor = Color.green;

            // COLOR POR TIPO

            switch (zone.sizeType)
            {
                case SocialZoneSize.Small:
                    zoneColor =
                        new Color(0f, 1f, 0f, 0.20f);
                    break;

                case SocialZoneSize.Medium:
                    zoneColor =
                        new Color(0f, 0.8f, 0f, 0.25f);
                    break;

                case SocialZoneSize.Large:
                    zoneColor =
                        new Color(0f, 0.55f, 0f, 0.30f);
                    break;
            }
            // CUBO SÓLIDO

            Gizmos.color = zoneColor;

            Gizmos.DrawCube(
                zone.center,
                zoneSize
            );

            // OUTLINE

            Gizmos.color =
                new Color(
                    zoneColor.r,
                    zoneColor.g,
                    zoneColor.b,
                    1f
                );

            Gizmos.DrawWireCube(
                zone.center,
                zoneSize
            );
        }
    }
}
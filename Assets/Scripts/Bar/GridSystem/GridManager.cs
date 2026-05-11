using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid")]
    public int width = 20;
    public int height = 30;
    public float cellSize = 1f;

    [Header("Layers")]
    public LayerMask obstacleLayer;
    public LayerMask tableLayer;

    [HideInInspector]
    public Node[,] grid;

    private GridDangerSystem dangerSystem;
    private WaypointGenerator waypointGenerator;

    private void Awake()
    {
        CreateGrid();

        dangerSystem = GetComponent<GridDangerSystem>();
        waypointGenerator = GetComponent<WaypointGenerator>();

        if (dangerSystem != null)
        {
            dangerSystem.CalculateDangerZones(this);
        }

        if (waypointGenerator != null)
        {
            waypointGenerator.GenerateWaypoints(this);
        }
    }

    void CreateGrid()
    {
        grid = new Node[width, height];

        Vector2 origin = (Vector2)transform.position
            - Vector2.right * width / 2f * cellSize
            - Vector2.up * height / 2f * cellSize;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 worldPos = origin +
                    Vector2.right * (x * cellSize + cellSize / 2f) +
                    Vector2.up * (y * cellSize + cellSize / 2f);

                bool walkable =
                    !Physics2D.OverlapCircle(worldPos, cellSize / 2f, obstacleLayer);

                bool nearTable =
                    Physics2D.OverlapCircle(worldPos, cellSize / 2f, tableLayer);

                grid[x, y] = new Node(
                    walkable,
                    nearTable,
                    worldPos,
                    x,
                    y
                );
            }
        }
    }

    public Node NodeFromWorld(Vector2 worldPos)
    {
        float percentX = (worldPos.x + width / 2f) / width;
        float percentY = (worldPos.y + height / 2f) / height;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((width - 1) * percentX);
        int y = Mathf.RoundToInt((height - 1) * percentY);

        return grid[x, y];
    }

    public Node GetNode(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;

        return grid[x, y];
    }
}
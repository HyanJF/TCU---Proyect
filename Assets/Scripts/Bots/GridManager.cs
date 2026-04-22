using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 20;
    public int height = 30;
    public float cellSize = 1f;

    public LayerMask obstacleLayer;

    private Node[,] grid;

    void Start()
    {
        CreateGrid();
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

                bool walkable = !Physics2D.OverlapCircle(worldPos, cellSize / 2f, obstacleLayer);

                grid[x, y] = new Node(walkable, worldPos, x, y);
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

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    int GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(a.gridX - b.gridX);
        int dstY = Mathf.Abs(a.gridY - b.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = NodeFromWorld(startPos);
        Node targetNode = NodeFromWorld(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        foreach (Node n in grid)
        {
            n.gCost = int.MaxValue;
            n.hCost = 0;
            n.parent = null;
        }

        while (openSet.Count > 0)
        {
            Node current = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < current.fCost ||
                   (openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost))
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in GetNeighbours(current))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int newCost = current.gCost + GetDistance(current, neighbour);

                if (newCost < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return null;
    }

    List<Node> RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
            return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = grid[x, y];

                if (node == null) continue;

                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawWireCube(node.worldPosition, Vector3.one * (cellSize * 0.9f));
            }
        }
    }
}
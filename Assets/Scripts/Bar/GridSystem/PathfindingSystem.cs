using System.Collections.Generic;
using UnityEngine;

public class PathfindingSystem : MonoBehaviour
{
    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = gridManager.NodeFromWorld(startPos);
        Node targetNode = gridManager.NodeFromWorld(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        foreach (Node n in gridManager.grid)
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
                   (openSet[i].fCost == current.fCost &&
                    openSet[i].hCost < current.hCost))
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

                int newCost =
                    current.gCost +
                    GetDistance(current, neighbour) +
                    neighbour.movementPenalty;

                if (newCost < neighbour.gCost ||
                    !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCost;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
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

    List<Node> GetNeighbours(Node node)
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

                Node neighbour =
                    gridManager.GetNode(checkX, checkY);

                if (neighbour == null)
                    continue;

                bool isDiagonal = x != 0 && y != 0;

                if (isDiagonal)
                {
                    Node horizontal =
                        gridManager.GetNode(node.gridX + x, node.gridY);

                    Node vertical =
                        gridManager.GetNode(node.gridX, node.gridY + y);

                    if (horizontal == null || vertical == null)
                        continue;

                    if (!horizontal.walkable || !vertical.walkable)
                        continue;
                }

                neighbours.Add(neighbour);
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
}
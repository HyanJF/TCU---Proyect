using UnityEngine;

public class GridDangerSystem : MonoBehaviour
{
    public void CalculateDangerZones(GridManager gridManager)
    {
        int dangerRadius = 1;

        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                Node node = gridManager.grid[x, y];

                if (!node.walkable)
                    continue;

                int penalty = 0;

                if (node.isTableZone)
                {
                    penalty += 35;
                }

                for (int dx = -dangerRadius; dx <= dangerRadius; dx++)
                {
                    for (int dy = -dangerRadius; dy <= dangerRadius; dy++)
                    {
                        int checkX = x + dx;
                        int checkY = y + dy;

                        Node neighbour =
                            gridManager.GetNode(checkX, checkY);

                        if (neighbour == null)
                            continue;

                        if (!neighbour.walkable)
                        {
                            float distance =
                                Mathf.Sqrt(dx * dx + dy * dy);

                            int dangerValue =
                                Mathf.RoundToInt(20 / (distance + 1));

                            penalty += dangerValue;
                        }
                    }
                }

                node.movementPenalty = penalty;
            }
        }
    }
}
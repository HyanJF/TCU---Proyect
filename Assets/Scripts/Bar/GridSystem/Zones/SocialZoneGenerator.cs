using System.Collections.Generic;
using UnityEngine;

public class SocialZoneGenerator : MonoBehaviour
{
    [Header("Social Zones")]
    public int maxZones = 3;

    [Header("Generation")]
    [Range(0f, 1f)]
    public float zoneChance = 0.25f;

    public List<SocialZone> generatedZones =
        new List<SocialZone>();

    private GridManager gridManager;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        GenerateZones();
    }

    public void GenerateZones()
    {
        generatedZones.Clear();

        int createdZones = 0;

        // LARGE

        if (TryGenerateZone(SocialZoneSize.Large))
        {
            createdZones++;
        }

        // MEDIUM

        if (createdZones < maxZones)
        {
            if (TryGenerateZone(SocialZoneSize.Medium))
            {
                createdZones++;
            }
        }

        // SMALL

        while (createdZones < maxZones)
        {
            bool generated =
                TryGenerateZone(SocialZoneSize.Small);

            if (!generated)
                break;

            createdZones++;
        }

        Debug.Log(
            $"[SOCIAL] Zonas generadas: {generatedZones.Count}"
        );
    }

    bool TryGenerateZone(SocialZoneSize sizeType)
    {
        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                // RANDOM CHANCE
                if (Random.value > zoneChance)
                    continue;

                // VALIDACIÓN
                if (!CanPlaceZone(x, y, sizeType))
                    continue;

                Node centerNode =
                    gridManager.GetNode(x, y);

                if (centerNode == null)
                    continue;

                // DISTANCIA ENTRE ZONAS
                if (HasNearbyZone(
                    centerNode.worldPosition,
                    sizeType))
                {
                    continue;
                }

                // CREAR ZONA
                SocialZone zone =
                    new SocialZone();

                zone.center =
                    centerNode.worldPosition;

                zone.sizeType =
                    sizeType;

                generatedZones.Add(zone);

                return true;
            }
        }

        return false;
    }

    bool CanPlaceZone(
        int centerX,
        int centerY,
        SocialZoneSize sizeType)
    {
        int zoneSize =
            GetZoneSize(sizeType);

        int spacing =
            GetZoneSpacing(sizeType);

        // Tamaño total requerido
        int radius =
            (zoneSize / 2) + spacing;

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Node node =
                    gridManager.GetNode(
                        centerX + x,
                        centerY + y
                    );

                if (node == null)
                    return false;

                if (!node.walkable)
                    return false;

                if (node.isTableZone)
                    return false;
            }
        }

        return true;
    }

    bool HasNearbyZone(
        Vector2 position,
        SocialZoneSize sizeType)
    {
        foreach (var zone in generatedZones)
        {
            int currentSize =
                GetZoneSize(sizeType);

            int otherSize =
                GetZoneSize(zone.sizeType);

            int spacing =
                GetZoneSpacing(sizeType);

            float requiredDistance =
                ((currentSize + otherSize)
                * 0.5f
                * gridManager.cellSize)
                + spacing;

            float distance =
                Vector2.Distance(
                    position,
                    zone.center
                );

            if (distance < requiredDistance)
            {
                return true;
            }
        }

        return false;
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
                return 7;
        }

        return 3;
    }

    int GetZoneSpacing(SocialZoneSize type)
    {
        switch (type)
        {
            case SocialZoneSize.Small:
                return 1;

            case SocialZoneSize.Medium:
                return 1;

            case SocialZoneSize.Large:
                return 2;
        }

        return 1;
    }
}
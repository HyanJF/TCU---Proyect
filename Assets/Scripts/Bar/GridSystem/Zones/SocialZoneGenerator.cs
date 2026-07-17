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

        // LOOP PRINCIPAL
        while (createdZones < maxZones)
        {
            bool generatedSomething = false;

            //ZONAS GRANDES

            if (createdZones < maxZones)
            {
                bool generated =
                    TryGenerateZone(SocialZoneSize.Large);

                if (generated)
                {
                    createdZones++;
                    generatedSomething = true;
                }
            }

            //ZONAS MEDIANAS

            if (createdZones < maxZones)
            {
                bool generated =
                    TryGenerateZone(SocialZoneSize.Medium);

                if (generated)
                {
                    createdZones++;
                    generatedSomething = true;
                }
            }

            //ZONAS PEQUEÑAS

            if (createdZones < maxZones)
            {
                bool generated =
                    TryGenerateZone(SocialZoneSize.Small);

                if (generated)
                {
                    createdZones++;
                    generatedSomething = true;
                }
            }

            if (!generatedSomething)
            {
                Debug.Log(
                    "[SOCIAL] No hay más espacio disponible"
                );

                break;
            }
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

        // Tamaño total pa que jale
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

            int currentSpacing =
                GetZoneSpacing(sizeType);

            int otherSize =
                GetZoneSize(zone.sizeType);

            int otherSpacing =
                GetZoneSpacing(zone.sizeType);

            // RADIO TOTAL REAL
            float currentRadius =
                (currentSize * 0.5f)
                + currentSpacing;

            float otherRadius =
                (otherSize * 0.5f)
                + otherSpacing;

            float requiredDistance =
                (currentRadius + otherRadius)
                * gridManager.cellSize;

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

    public int GetZoneSize(SocialZoneSize type)
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
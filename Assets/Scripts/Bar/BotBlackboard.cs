using System.Collections.Generic;
using UnityEngine;

public class BotBlackboard : MonoBehaviour
{
    public static BotBlackboard Instance;

    [Header("Baño")]
    public Transform bathroom;
    public Transform exitBathroom;

    [Header("Asientos Barra")]
    public List<Seat> seats = new List<Seat>();

    [Header("Asientos Mesas")]
    public List<Seat> seatsTable = new List<Seat>();

    [Header("Waypoints")]
    public List<Transform> waypoints = new List<Transform>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
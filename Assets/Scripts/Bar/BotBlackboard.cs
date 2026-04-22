using System.Collections.Generic;
using UnityEngine;

public class BotBlackboard : MonoBehaviour
{
    public static BotBlackboard Instance;

    [Header("Barra")]
    public Transform barra;

    [Header("Re-spawn")]
    public Transform reactivatePoint;

    [Header("Asientos Barra")]
    public List<Seat> seats = new List<Seat>();

    [Header("Asientos Mesas")]
    public List<Seat> seatsTable = new List<Seat>();

    private void Awake()
    {
        Instance = this;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class BotBlackboard : MonoBehaviour
{
    public static BotBlackboard Instance;

    [Header("Barra")]
    public Transform barra;

    [Header("Re-spawn")]
    public Transform reactivatePoint;

    [Header("Asientos")]
    public List<Seat> seats = new List<Seat>();

    [Header("Bots en espera")]
    private List<GameObject> waitingBots = new List<GameObject>();

    [Header("Cooldown")]
    public float releaseCooldown = 2f;
    private float lastReleaseTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // BOT entra a la barra
    public void RegisterBot(GameObject bot)
    {
        waitingBots.Add(bot);
    }

    // Obtener asiento random
    public Seat GetRandomFreeSeat()
    {
        List<Seat> freeSeats = new List<Seat>();

        foreach (var seat in seats)
        {
            if (!seat.isOccupied)
                freeSeats.Add(seat);
        }

        if (freeSeats.Count == 0) return null;

        int index = Random.Range(0, freeSeats.Count);
        freeSeats[index].isOccupied = true;
        OnSeatsChanged?.Invoke();

        return freeSeats[index];
    }

    // Liberar uno con cooldown
    public void ReleaseOneSeat()
    {
        if (Time.time < lastReleaseTime + releaseCooldown)
            return;

        foreach (var seat in seats)
        {
            if (seat.isOccupied)
            {
                seat.isOccupied = false;
                lastReleaseTime = Time.time;

                OnSeatsChanged?.Invoke();

                ReactivateBot();
                return;
            }
        }
    }

    // Reactivar bot
    void ReactivateBot()
    {
        if (waitingBots.Count == 0) return;

        GameObject bot = waitingBots[0];
        waitingBots.RemoveAt(0);

        bot.transform.position = reactivatePoint.position;
        bot.SetActive(true);

        // Avisarle que cambió de estado
        bot.GetComponent<BotController>().OnReactivated();
    }

    public bool HasOccupiedSeats()
    {
        foreach (var seat in seats)
        {
            if (seat.isOccupied)
                return true;
        }

        return false;
    }

    public System.Action OnSeatsChanged;
}
using System.Collections.Generic;
using UnityEngine;

public class SeatManager : MonoBehaviour
{
    public static SeatManager Instance;

    public System.Action OnSeatsChanged;

    private void Awake()
    {
        Instance = this;
    }

    public Seat GetFreeSeat(List<Seat> seats)
    {
        List<Seat> freeSeats = new List<Seat>();

        foreach (var seat in seats)
        {
            if (seat.IsFree())
                freeSeats.Add(seat);
        }

        if (freeSeats.Count == 0)
            return null;

        Seat selected = freeSeats[Random.Range(0, freeSeats.Count)];
        selected.Reserve();

        OnSeatsChanged?.Invoke();

        return selected;
    }

    public void OccupySeat(Seat seat, GameObject bot)
    {
        seat.Occupy(bot);
        OnSeatsChanged?.Invoke();
    }

    public void ReleaseSeat(Seat seat)
    {
        GameObject bot = seat.Release();

        if (bot != null)
        {
            bot.SetActive(true);
            bot.transform.position = seat.transform.position;

            BotController controller = bot.GetComponent<BotController>();

            controller.targetSeat = null;
        }

        OnSeatsChanged?.Invoke();
    }

    public bool HasOccupiedSeats(List<Seat> seats)
    {
        foreach (var seat in seats)
        {
            if (seat.state == Seat.SeatState.Occupied)
                return true;
        }

        return false;
    }

    public void ReleaseFirstOccupiedSeat(List<Seat> seats)
    {
        foreach (var seat in seats)
        {
            if (seat.state == Seat.SeatState.Occupied)
            {
                ReleaseSeat(seat);
                return;
            }
        }
    }
}
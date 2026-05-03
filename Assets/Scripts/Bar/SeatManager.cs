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

    // SOLO CHECAR
    public bool HasFreeSeat(List<Seat> seats, Seat.SeatType? type = null)
    {
        if (seats == null) return false;

        foreach (var seat in seats)
        {
            if (seat == null) continue;

            if (!seat.IsFree()) continue;

            // FILTRO POR TIPO
            if (type.HasValue && seat.seatType != type.Value)
                continue;

            return true;
        }

        return false;
    }

    // OBTENER Y RESERVAR
    public Seat GetFreeSeat(List<Seat> seats, Seat.SeatType? type = null)
    {
        if (seats == null || seats.Count == 0)
            return null;

        List<Seat> freeSeats = new List<Seat>();

        foreach (var seat in seats)
        {
            if (seat == null) continue;

            if (!seat.IsFree()) continue;

            // FILTRO POR TIPO
            if (type.HasValue && seat.seatType != type.Value)
                continue;

            freeSeats.Add(seat);
        }

        if (freeSeats.Count == 0)
            return null;

        Seat selected = freeSeats[Random.Range(0, freeSeats.Count)];
        selected.Reserve();

        Debug.Log($"[SeatManager] Asiento reservado: {selected.name}");

        OnSeatsChanged?.Invoke();

        return selected;
    }

    public void OccupySeat(Seat seat, GameObject bot)
    {
        if (seat == null || bot == null)
            return;

        // VALIDACIÓN FINAL
        if (seat.state == Seat.SeatState.Occupied)
        {
            Debug.LogWarning("[SeatManager] Intento de ocupar asiento ya ocupado");
            return;
        }

        seat.Occupy(bot);
        bot.SetActive(false);

        Debug.Log($"[SeatManager] Bot sentado en: {seat.name}");

        OnSeatsChanged?.Invoke();
    }

    public void ReleaseSeat(Seat seat)
    {
        if (seat == null)
            return;

        GameObject bot = seat.Release();

        if (bot != null)
        {
            bot.SetActive(true);

            Vector3 pos = seat.transform.position;
            pos.y -= 1f;

            bot.transform.position = pos;

            BotController controller = bot.GetComponent<BotController>();

            if (controller != null)
            {
                controller.targetSeat = null;
                controller.ClearPath();
                controller.ChangeState(new BotThinkState());
            }

            Debug.Log($"[SeatManager] Bot liberado de: {seat.name}");
        }

        OnSeatsChanged?.Invoke();
    }

    public bool HasOccupiedSeats(List<Seat> seats)
    {
        if (seats == null) return false;

        foreach (var seat in seats)
        {
            if (seat != null && seat.state == Seat.SeatState.Occupied)
                return true;
        }

        return false;
    }

    public void ReleaseFirstOccupiedSeat(List<Seat> seats)
    {
        if (seats == null) return;

        foreach (var seat in seats)
        {
            if (seat != null && seat.state == Seat.SeatState.Occupied)
            {
                ReleaseSeat(seat);
                return;
            }
        }
    }
}
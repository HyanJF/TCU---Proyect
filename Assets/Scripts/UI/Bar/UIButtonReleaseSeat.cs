using UnityEngine;

public class UIButtonReleaseSeat : MonoBehaviour
{
    public void ReleaseSeat()
    {
        if (!SeatManager.Instance.HasOccupiedSeats(BotBlackboard.Instance.seats))
        {
            Debug.Log("No hay asientos ocupados");
            return;
        }

        SeatManager.Instance.ReleaseFirstOccupiedSeat(
            BotBlackboard.Instance.seats
        );
    }
}
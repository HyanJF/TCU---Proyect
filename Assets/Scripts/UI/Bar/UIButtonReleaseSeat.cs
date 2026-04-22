using UnityEngine;

public class UIButtonReleaseSeat : MonoBehaviour
{
    public void ReleaseSeat()
    {
        SeatManager.Instance.ReleaseFirstOccupiedSeat(
            BotBlackboard.Instance.seats
        );
    }
}
using UnityEngine;

public class Seat : MonoBehaviour
{
    public enum SeatState
    {
        Free,
        Reserved,
        Occupied
    }

    public enum SeatType
    {
        Bar,
        Table
    }

    public SeatState state = SeatState.Free;

    [Header("Visual")]
    public GameObject visualBot;

    public GameObject currentBot;

    [Header("Type")]
    public SeatType seatType;

    private void Start()
    {
        if (visualBot != null)
            visualBot.SetActive(false);
    }

    public void Reserve()
    {
        state = SeatState.Reserved;
    }

    public void Occupy(GameObject bot)
    {
        state = SeatState.Occupied;
        currentBot = bot;

        if (visualBot != null)
            visualBot.SetActive(true);
    }

    public GameObject Release()
    {
        state = SeatState.Free;

        if (visualBot != null)
            visualBot.SetActive(false);

        GameObject bot = currentBot;
        currentBot = null;

        return bot;
    }

    public bool IsFree()
    {
        return state == SeatState.Free;
    }
}
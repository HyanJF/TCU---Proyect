using UnityEngine;

public class UISeatIndicator : MonoBehaviour
{
    public GameObject imageObject;

    private void Start()
    {
        if (SeatManager.Instance != null)
        {
            SeatManager.Instance.OnSeatsChanged += UpdateUI;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (SeatManager.Instance == null || BotBlackboard.Instance == null)
            return;

        bool state = SeatManager.Instance.HasOccupiedSeats(
            BotBlackboard.Instance.seats // barra
        );

        imageObject.SetActive(state);
    }

    private void OnDestroy()
    {
        if (SeatManager.Instance != null)
        {
            SeatManager.Instance.OnSeatsChanged -= UpdateUI;
        }
    }
}
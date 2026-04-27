using UnityEngine;

public class UISeatIndicator : MonoBehaviour
{
    public GameObject imageObject;

    private void OnEnable()
    {
        if (SeatManager.Instance != null)
        {
            SeatManager.Instance.OnSeatsChanged += UpdateUI;
        }

        UpdateUI(); // siempre actualizar al activarse
    }

    void UpdateUI()
    {
        if (SeatManager.Instance == null || BotBlackboard.Instance == null)
            return;

        if (imageObject == null)
        {
            Debug.LogWarning("[UISeatIndicator] imageObject no asignado");
            return;
        }

        // Revisar TODOS los asientos
        bool barOccupied = SeatManager.Instance.HasOccupiedSeats(
            BotBlackboard.Instance.seats
        );

        bool finalState = barOccupied;

        imageObject.SetActive(finalState);

        Debug.Log($"[UI] Asientos ocupados → {finalState}");
    }

    private void OnDisable()
    {
        if (SeatManager.Instance != null)
        {
            SeatManager.Instance.OnSeatsChanged -= UpdateUI;
        }
    }
}
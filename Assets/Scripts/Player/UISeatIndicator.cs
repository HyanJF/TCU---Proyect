using UnityEngine;

public class UISeatIndicator : MonoBehaviour
{
    public GameObject imageObject;

    private void Start()
    {
        if (BotBlackboard.Instance != null)
        {
            BotBlackboard.Instance.OnSeatsChanged += UpdateUI;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        bool state = BotBlackboard.Instance.HasOccupiedSeats();
        imageObject.SetActive(state);
    }
}
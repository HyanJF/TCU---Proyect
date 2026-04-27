using UnityEngine;

public class BotStats : MonoBehaviour
{
    [Header("Needs")]
    [Range(0f, 100f)] public float thirst;
    [Range(0f, 100f)] public float bladder;
    [Range(0f, 100f)] public float comfort;

    [Header("Flags")]
    public bool wantsInteract;

    [Header("Rates")]
    public float thirstRate = 3f;
    public float bladderRate = 2f;

    private BotController controller;

    private void Awake()
    {
        controller = GetComponent<BotController>();
    }

    private void Update()
    {
        // NECESIDADES QUE SUBEN
        thirst += thirstRate * Time.deltaTime;
        bladder += bladderRate * Time.deltaTime;

        // CONFORT CONTINUO (si se está moviendo)
        if (controller != null && controller.IsMoving())
        {
            AddComfort(Time.deltaTime * 2f);
        }

        ClampStats();
    }

    // REDUCIR THIRST
    public void ReduceThirst(float amount)
    {
        thirst = Mathf.Clamp(thirst - amount, 0f, 100f);
        Debug.Log($"[STATS] Thirst: {thirst}");
    }

    // AUMENTAR COMFORT
    public void AddComfort(float amount)
    {
        comfort = Mathf.Clamp(comfort + amount, 0f, 100f);
    }

    // REDUCIR BLADDER
    public void ReduceBladder(float amount)
    {
        bladder = Mathf.Clamp(bladder - amount, 0f, 100f);
        Debug.Log($"[STATS] Bladder: {bladder}");
    }

    void ClampStats()
    {
        thirst = Mathf.Clamp(thirst, 0f, 100f);
        bladder = Mathf.Clamp(bladder, 0f, 100f);
        comfort = Mathf.Clamp(comfort, 0f, 100f);
    }
}
using Unity.VisualScripting;
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

    [Header("Multiplier")]
    public float thirstMultiplier = 1f;
    public float bladderMultiplier = 1f;

    private float defaultMultiplier = 1f;

    [Header("Progress")]
    public int drinksDone = 0;
    public int bathroomVisits = 0;

    private BotController controller;

    private void Awake()
    {
        controller = GetComponent<BotController>();
    }

    private void Update()
    {

        // NECESIDADES QUE SUBEN
        thirst += thirstRate * thirstMultiplier * Time.deltaTime;

        // BLADDER SOLO SUBE SI NO está en baño
        if (!controller.IsInState<BotUsingWCState>())
        {
            bladder += bladderRate * bladderMultiplier * Time.deltaTime;
        }

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

    // MULTIPLICADORES
    public void SetThirstMultiplier(float value)
    {
        thirstMultiplier = value;
    }

    public void ResetThirstMultiplier()
    {
        thirstMultiplier = defaultMultiplier;
    }

    public void SetBladderMultiplier(float value)
    {
        bladderMultiplier = value;
    }

    public void ResetBladderMultiplier()
    {
        bladderMultiplier = defaultMultiplier;
    }
}
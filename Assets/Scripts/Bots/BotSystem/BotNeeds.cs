using UnityEngine;

public class BotNeeds : MonoBehaviour
{
    [Header("Needs")]
    [Range(0f, 100f)] public float thirst;
    [Range(0f, 100f)] public float bladder;
    [Range(0f, 100f)] public float comfort;
    [Range(0f, 100f)] public float social;

    [Header("Flags")]
    public bool wantsInteract;

    [Header("Rates")]
    public float thirstRate = 3f;
    public float bladderRate = 2f;
    public float socialRate = 1.5f;

    [Header("Progress")]
    public int drinksDone;
    public int bathroomVisits;

    private BotController bot;

    private void Awake()
    {
        bot = GetComponent<BotController>();
    }

    private void Update()
    {
        IncreaseNeeds();

        if (bot.movement.IsMoving())
        {
            ApplyComfort(Time.deltaTime * 2f);
        }

        ClampNeeds();
    }

    void IncreaseNeeds()
    {
        thirst += thirstRate * Time.deltaTime;

        social += socialRate * Time.deltaTime;

        if (!bot.stateMachine.IsInState<BotUsingWCState>())
        {
            bladder += bladderRate * Time.deltaTime;
        }
    }

    void ClampNeeds()
    {
        thirst = Mathf.Clamp(thirst, 0f, 100f);
        bladder = Mathf.Clamp(bladder, 0f, 100f);
        comfort = Mathf.Clamp(comfort, 0f, 100f);
        social = Mathf.Clamp(social, 0f, 100f);
    }

    // DRINK EFFECTS

    public void ApplyDrink(float amount)
    {
        // Reduce sed
        thirst -= amount;

        // Aumenta ganas de baño
        bladder += amount * 0.2f;

        // Mejora comfort ligeramente
        comfort += 10f;

        drinksDone++;

        ClampNeeds();
    }

    // BATHROOM EFFECTS

    public void ApplyBathroom(float amount)
    {
        bladder -= amount;

        // Ir al baño reduce comfort un poco
        comfort -= 5f;

        bathroomVisits++;

        ClampNeeds();
    }

    // SOCIAL EFFECTS

    public void ApplySocial(float amount)
    {
        social -= amount;

        // Socializar mejora comfort
        comfort += 15f;

        ClampNeeds();
    }

    // COMFORT

    public void ApplyComfort(float amount)
    {
        comfort += amount;

        ClampNeeds();
    }
}
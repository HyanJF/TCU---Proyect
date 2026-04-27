using UnityEngine;

public class BotPriority : MonoBehaviour
{
    private BotStats stats;

    public BotNeed currentNeed = BotNeed.None;

    private void Awake()
    {
        stats = GetComponent<BotStats>();
    }

    public BotNeed EvaluateNeeds()
    {
        float highestValue = 0f;
        BotNeed selectedNeed = BotNeed.None;

        if (stats.thirst > highestValue)
        {
            highestValue = stats.thirst;
            selectedNeed = BotNeed.Thirst;
        }

        if (stats.bladder > highestValue)
        {
            highestValue = stats.bladder;
            selectedNeed = BotNeed.Bladder;
        }

        if (stats.comfort > highestValue)
        {
            highestValue = stats.comfort;
            selectedNeed = BotNeed.Comfort;
        }

        if (stats.wantsInteract)
        {
            selectedNeed = BotNeed.Interact;
        }

        currentNeed = selectedNeed;
        return selectedNeed;
    }
}
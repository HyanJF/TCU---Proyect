using UnityEngine;

public class BotBrain : MonoBehaviour
{
    private BotNeeds needs;

    public BotNeed currentNeed = BotNeed.None;

    private void Awake()
    {
        needs = GetComponent<BotNeeds>();
    }

    public BotNeed DecideNeed()
    {
        float highestValue = 0f;

        BotNeed selected = BotNeed.None;

        if (needs.thirst > highestValue)
        {
            highestValue = needs.thirst;
            selected = BotNeed.Thirst;
        }

        if (needs.bladder > highestValue)
        {
            highestValue = needs.bladder;
            selected = BotNeed.Bladder;
        }

        float comfortNeed = 100f - needs.comfort;

        if (comfortNeed > highestValue)
        {
            highestValue = comfortNeed;
            selected = BotNeed.Comfort;
        }

        if (needs.social > highestValue)
        {
            highestValue = needs.social;
            selected = BotNeed.Social;
        }

        if (needs.wantsInteract)
        {
            selected = BotNeed.Interact;
        }

        currentNeed = selected;

        return selected;
    }
}
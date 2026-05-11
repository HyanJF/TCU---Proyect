using UnityEngine;

public class BotActions : MonoBehaviour
{
    private BotNeeds needs;
    private BotVisual visual;

    private void Awake()
    {
        needs = GetComponent<BotNeeds>();
        visual = GetComponent<BotVisual>();
    }

    public void Drink()
    {
        needs.ApplyDrink(100f);

        needs.drinksDone++;

        Debug.Log("[ACTION] Drink");
    }

    public void UseBathroom()
    {
        needs.ApplyBathroom(100f);

        needs.bathroomVisits++;

        Debug.Log("[ACTION] Bathroom");
    }

    public void Socialize()
    {
        needs.ApplySocial(100f);

        Debug.Log("[ACTION] Socialize");
    }

    public void HideBot()
    {
        visual.SetVisible(false);
    }

    public void ShowBot()
    {
        visual.SetVisible(true);
    }
}
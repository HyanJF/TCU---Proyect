using UnityEngine;

public class BotVisual : MonoBehaviour
{
    private Renderer[] renderers;
    private Collider2D[] colliders;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider2D>();
    }

    public void SetVisible(bool active)
    {
        foreach (var r in renderers)
            r.enabled = active;

        foreach (var c in colliders)
            c.enabled = active;
    }
}
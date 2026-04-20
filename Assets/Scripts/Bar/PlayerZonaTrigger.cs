using UnityEngine;

public class PlayerZoneTrigger : MonoBehaviour
{
    public GameObject menuUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            menuUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            menuUI.SetActive(false);
        }
    }
}
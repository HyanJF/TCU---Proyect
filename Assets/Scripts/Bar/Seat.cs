using UnityEngine;

public class Seat : MonoBehaviour
{
    public bool isOccupied = false;
    public GameObject bot;

    private void Update()
    {
        if (bot != null)
        {
            bot.SetActive(isOccupied);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isOccupied ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
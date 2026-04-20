using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    public GameObject botPrefab;
    public Transform[] spawnPoints;

    public void SpawnBot()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Instantiate(botPrefab, spawnPoints[index].position, Quaternion.identity);
    }
}
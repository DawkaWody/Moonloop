using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] starPrefabs;
    [SerializeField] private int numberOfStars;
    [SerializeField] private Vector2 spawnArea;

    void Start()
    {
        for (int i = 0; i < numberOfStars; i++)
        {
            Vector3 spawnPos = new Vector3(
            Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f), Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f), 0f) + transform.position;

            GameObject prefab = starPrefabs[Random.Range(0, starPrefabs.Length)];
            GameObject star = Instantiate(prefab, spawnPos, Quaternion.identity, transform);

        }
    }
}

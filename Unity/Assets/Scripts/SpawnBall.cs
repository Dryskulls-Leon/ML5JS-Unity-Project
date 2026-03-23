using UnityEngine;

public class SpawnBall : MonoBehaviour
{

    [SerializeField] 
    private GameObject ballPrefab;
    [SerializeField]
    private float maxSpawnInterval = 0.3f;

    private float spawnInterval;


    private void Start()
    {
        spawnInterval = maxSpawnInterval;
    }
    void Update()
    {
        if (spawnInterval < 0) { 
        Instantiate(ballPrefab, transform.position, Quaternion.identity);
        spawnInterval = maxSpawnInterval;
        }
        spawnInterval -= Time.deltaTime;
    }
}

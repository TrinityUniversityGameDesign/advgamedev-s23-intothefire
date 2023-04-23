using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_MicroController : MonoBehaviour
{
    
    public GameObject meteorPrefab;     // Prefab to spawn
    public float spawnDelay = 1f;     // Delay between spawns in seconds
    public float spawnRadius = 40.0f;   // Radius around the spawner where meteors can spawn
    public float maxMeteorsPerSpawn = 10; // Spawns this many meteors per wave of meteors

    private float spawnTimer = 0.0f;

    private void Start()
    {
        spawnRadius = GameManager.Instance.DistanceApart * (GameManager.Instance.LabyrinthSize / 2);
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            spawnTimer = 0.0f;
            for(int x = 0; x < maxMeteorsPerSpawn; x++)
            {
                Vector2 xy = Random.insideUnitCircle * spawnRadius;

                Vector3 spawnPos = transform.position + new Vector3(xy.x, 0, xy.y);

                GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity); //TODO should change so we spawn them in inactive at the beginning of the event to reduce lag
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_MicroController : MonoBehaviour
{
    
    public GameObject meteorPrefab;     // Prefab to spawn
    public float spawnDelay = 2f;     // Delay between spawns
    public float spawnRadius = 40.0f;   // Radius around the spawner where meteors can spawn

    private float spawnTimer = 0.0f;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            spawnTimer = 0.0f;

            Vector2 xy = Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPos = transform.position + new Vector3(xy.x, 0, xy.y);

            GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
        }
    }
}

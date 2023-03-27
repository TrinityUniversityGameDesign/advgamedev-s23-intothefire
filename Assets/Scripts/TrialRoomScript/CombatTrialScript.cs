using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrialScript : TrialRoomScript
{
    [SerializeField]
    public List<GameObject> enemyPool = new List<GameObject>();
    int currentWave;

    void Start()
    {   
		SetDoorPresence(false);
        currentWave = 0;
		currRoomState = RoomState.empty;
    }

    // Update is called once per frame
    void Update()
    {   
        Debug.Log("Current Enemy Count: " + GetEnemyCount());
        if (currRoomState == RoomState.trialing && GetEnemyCount() <= 0) {
            ++currentWave;
            if (TrySpawnEnemyWave()) {
                Debug.Log("Spawning Enemy Wave " + currentWave);
            } else {
                TrialCompleted();
            }
        }
    }

    bool TrySpawnEnemyWave(){
        bool didSpawn = false;
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        foreach (GameObject spawnPoint in spawnPoints) {   
            EnemySpawnPoint script = spawnPoint.GetComponent<EnemySpawnPoint>();
            if (script.GetWaveNumber() == currentWave) {
                GameObject enemy = script.SpawnEnemy();
                RegisterEnemy(enemy);
                didSpawn = true;
            }
        }
        return didSpawn;
    }

    void SpawnRandomEnemyWave(){
        int numEnemies = Random.Range(3,5);
        for (int i = 0; i < numEnemies; i++) {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(0,30), 2, Random.Range(0,30));
            GameObject enemyPrefab = enemyPool[Random.Range(0, enemyPool.Count)];
            enemyPrefab = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, this.transform);
            RegisterEnemy(enemyPrefab);
        }
    }

    void RegisterEnemy(GameObject enemy){
        enemy.GetComponent<EnemyUpdate>().hostRoom = this;
        enemy.GetComponent<EnemyUpdate>().trialSpawned = true;
        IncrementEnemyCount();
    }

    public override void PlaceStartPad(){
        GameObject startPadReference = startPad;
		startPadReference.GetComponent<StartPadScript>().hostRoom = this;
        startPadReference = Instantiate(startPad, transform.position, Quaternion.identity, this.transform);
        trialGeometry.Add(startPadReference);
    }

    public override void StartTrial(){
        if (currRoomState == RoomState.closed) {
            SetEnemyCount(0);
            currRoomState = RoomState.trialing;
        }
    }

    public override void DespawnTrialGeometry(){
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                thing.SetActive(false);
            }
        }
    }
}
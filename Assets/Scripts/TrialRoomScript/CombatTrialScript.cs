using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrialScript : TrialRoomScript
{
[SerializeField]
    public List<GameObject> enemyPool = new List<GameObject>();
    int wavesRemaining;

    void Start()
    {
		SetDoorPresence(false);
		currRoomState = RoomState.empty;
    }

    // Update is called once per frame
    void Update()
    {   
        Debug.Log("Current Enemy Count: " + GetEnemyCount());
        if (currRoomState == RoomState.trialing && GetEnemyCount() <= 0) {
            Debug.Log("Waves Remaining: " + wavesRemaining);
            if (wavesRemaining <= 0){
                TrialCompleted();
            } else {
                SpawnEnemyWave();
            }
		}
    }

    void SpawnEnemyWave(){
        int numEnemies = Random.Range(3,5);
        for (int i = 0; i < numEnemies; i++) {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(0,30), 2, Random.Range(0,30));
            GameObject enemyPrefab = enemyPool[Random.Range(0, enemyPool.Count)];
            enemyPrefab = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, this.transform);
            enemyPrefab.GetComponent<EnemyUpdate>().hostRoom = this;
            enemyPrefab.GetComponent<EnemyUpdate>().trialSpawned = true;
            IncrementEnemyCount();
        }
        wavesRemaining -= 1;
    }

    public override void PlaceStartPad(){
        GameObject startPadReference = startPad;
		startPadReference.GetComponent<StartPadScript>().hostRoom = this;
        startPadReference = Instantiate(startPad, transform.position, Quaternion.identity, this.transform);
        trialGeometry.Add(startPadReference);
    }

    public override void StartTrial(){
        if (currRoomState == RoomState.closed) {
            wavesRemaining = 3;
            SetEnemyCount(0);
            SpawnEnemyWave();
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
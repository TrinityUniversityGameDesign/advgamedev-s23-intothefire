using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatTrialScript : TrialRoomScript
{
    [SerializeField]
    public List<GameObject> enemyPool = new List<GameObject>();
    List<GameObject> currentEnemies = new List<GameObject>();
    int currentWave;

    void Start()
    {   
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                thing.SetActive(false);
            }
        }
		SetDoorPresence(false);
        currentWave = 0;
		currRoomState = RoomState.empty;
    }

    // Update is called once per frame
    void Update()
    {   
        //Debug.Log("Current Enemy Count: " + enemyCount);
        if (currRoomState == RoomState.trialing && enemyCount <= 0 && playerList.Count > 0 ) {
            ++currentWave;
            if (TrySpawnEnemyWave()) {
                //Debug.Log("Spawning Enemy Wave " + currentWave);
            } else {
                TrialCompleted();
            }
        }
    }

    void FixedUpdate() {
        List<Transform> copy = new List<Transform>(playerList);
        if(playerList.Count > 0){
            foreach(Transform thing in playerList){
                if(thing && !thing.GetComponent<Collider>().bounds.Intersects(transform.GetComponent<Collider>().bounds)){
                    copy.Remove(thing);
                }
            }
            playerList = copy;
        }

        if (playerList.Count == 0){
            currentWave = 0;
            DespawnEnemies();
            SetDoorPresence(false);
        }
    }

    void DespawnEnemies() {
        foreach(GameObject enemy in currentEnemies){
            if(enemy){
                enemy.GetComponent<EnemyUpdate>().Kill();
            }
        }
    }

    bool TrySpawnEnemyWave(){
        enemyCount = 0;
        currentEnemies = new List<GameObject>();

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
        currentEnemies.Add(enemy);
        enemy.GetComponent<EnemyUpdate>().hostRoom = this;
        enemy.GetComponent<EnemyUpdate>().trialSpawned = true;
        ++enemyCount;
    }

    public override void PlaceStartPad(){
        GameObject startPadReference = startPad;
		startPadReference.GetComponent<StartPadScript>().hostRoom = this;
        startPadReference = Instantiate(startPad, transform.position, Quaternion.identity, this.transform);
        trialGeometry.Add(startPadReference);
    }

    public override void StartTrial(){
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                thing.SetActive(true);
            }
        }
        //transform.Find("NavMeshGeometry").GetComponent<NavMeshSurface>().BuildNavMesh();
        if (currRoomState == RoomState.closed) {
            enemyCount = 0;
            currRoomState = RoomState.trialing;
        }
    }

    public override void DespawnTrialGeometry(){
        SetDoorPresence(false);
        foreach (GameObject thing in trialGeometry)
        {
            if(thing){
                thing.SetActive(false);
            }
        }
    }

    new public void OnTriggerEnter(Collider other){
		if(other.transform.tag == "Player"){
            if(currRoomState == RoomState.empty){
                RoomClose();
                StartTrial();
            }

            if(!playerList.Contains(other.transform)){
                playerList.Add(other.transform);
            }
		}
	}
}
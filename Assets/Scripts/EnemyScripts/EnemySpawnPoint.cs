using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{   
    //Public values to be set in the editor
    public GameObject enemy;
    public int waveNumber;

    public GameObject GetEnemy(){return enemy;}

    public int GetWaveNumber(){return waveNumber;}

    public GameObject SpawnEnemy(){
        return Instantiate(GetEnemy(), transform.position, transform.rotation);
    }
}

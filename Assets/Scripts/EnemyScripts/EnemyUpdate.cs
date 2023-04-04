using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUpdate : MonoBehaviour
{   
    //Public values to be set in the editor
    public float maxHitPoints;
    public float detectionRange;

    //Public values to be ignored in the editor
    public TrialRoomScript hostRoom;
    public bool trialSpawned;

    protected float hitPoints;
    protected float frameCount;
    protected Rigidbody rb;

    void Start() {   
        hitPoints = maxHitPoints;
        frameCount = 0;
        rb = GetComponent<Rigidbody>();
        EnemyInit();
    }

    protected virtual void EnemyInit() {}

    void Update() {   
        if (hitPoints <= 0 || transform.position.y < -50){Kill();}
    }

    void FixedUpdate() {
        frameCount += 1;
    }

    public GameObject GetTarget() { //Get the closest player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject target = gameObject;
        float distance = detectionRange;
        for (int i = 0; i < players.Length; i++) {
            GameObject player = players[i];
            float playerDist = Vector3.Distance(transform.position, player.transform.position);
            if (playerDist < distance) {
                distance = playerDist;
                target = player;
            }
        }
        return target;
    }

    public Vector3 GetTargetPosition() {
        GameObject player = GetTarget();
        if (player) {return player.transform.position;}
        else {return transform.position;}
    }

    public bool TakeDamage(float amount) {
        hitPoints = hitPoints - amount;
        return true;
    }

    public void Kill() {
        if (trialSpawned) {hostRoom.DecrementEnemyCount();}
        Destroy(gameObject);
    }

	private void OnTriggerEnter(Collider other) {
		if(other.transform.tag == "Damage") {
            TakeDamage(other.gameObject.GetComponent<DamageScript>().GetDamage());
        }
	}
} 
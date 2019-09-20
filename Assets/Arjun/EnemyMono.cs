using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMono : MonoBehaviour {
    public EnemySpawner enemySpawner;
    public GameObject armor;
    public GameObject armor2;
    public GameObject[] armors;
    public GameObject pSystem;
    public GameObject target;
    public Enemy enemyData = new Enemy(0, new Vector3(0,0,0), true);
    public float angle;
    public Collider2D enemyCollider;


    private void Start()
    {
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }
    void Update()
    {
        //transform.localPosition = transform.localPosition + (enemyData.spawnLocation * God.GSM.Speed);
        transform.position += (God.GSM.target.transform.position - transform.position) * God.GSM.Speed * Time.deltaTime;
    }

    public void KillMePlease()
    {
        enemySpawner.RandomSpawn();
        Destroy(gameObject);
    }
}

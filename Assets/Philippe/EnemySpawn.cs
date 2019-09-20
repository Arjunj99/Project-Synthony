using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnState
{
    Enemy, Walls
}

public class EnemySpawn : MonoBehaviour
{
    public float spawnTimer = 5f, spawnDuration = 30f;
    public float wallTimer = 2f, wallDuration = 14f;
    private GameManager gameManager;
    public GameObject[] walls;
    public GameObject[] spawnObject;
    public int initCount = 12;
    public SpawnState spawnState = SpawnState.Walls;
    float elapsed = 0, stateElapsed = 0;
    bool stateSwitch = true, vertical = true;

    private void Start()
    {
        gameManager = GameObject.Find("Main").GetComponent<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        elapsed += gameManager.globalTimeDelta;
        stateElapsed += gameManager.globalTimeDelta;

        if(spawnState == SpawnState.Walls)
        {
            if (stateSwitch)
            {
                vertical = false;
                /*
                if(Random.Range(-1f, 1f) > 0)
                {
                    vertical = true;
                }
                else
                {
                    vertical = false;
                }
                */
            }

            if(elapsed > wallTimer)
            {
                elapsed = 0;
                SpawnWall(vertical);
            }

            stateSwitch = false;
            if (stateElapsed > wallDuration)
            {
                spawnState = SpawnState.Enemy;
                stateElapsed = 0;
                stateSwitch = true;
            }
            
        }
        else if(spawnState == SpawnState.Enemy)
        {
            if (stateSwitch)
            {
                for (int i = 0; i < initCount; i++)
                {
                    SpawnRand();
                }
                elapsed = 0;
            }

            if(elapsed > spawnTimer)
            {
                SpawnRand();
                SpawnRand();
                SpawnRand();
                elapsed = 0;
            }


            stateSwitch = false;
            if (stateElapsed > spawnDuration)
            {
                spawnState = SpawnState.Walls;
                stateElapsed = 0;
                stateSwitch = true;
            }
        }
    }

    public void SpawnRand()
    {
        float randomAngle = Random.Range(0f, 360f);
        Vector3 spawn = Quaternion.Euler(0, 0, randomAngle) * new Vector3(0, 1, 0) * 15f;
        GameObject obj = Instantiate(spawnObject[Random.Range(0, spawnObject.Length-1)], spawn, Quaternion.identity);
    }

    public void SpawnWall(bool vertical)
    {
        GameObject wall = Instantiate(walls[Random.Range(0, walls.Length - 1)], Vector3.one * 50f, Quaternion.identity);
        wall.GetComponent<WallSlam>().horizontal = !vertical;
    }
}

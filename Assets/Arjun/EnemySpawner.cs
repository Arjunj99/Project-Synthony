using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public int initialEnemies;
    public GameObject enemyPrefab;
    private EnemyMono enemyMono;
    private float timer = 0f;

    public int score = 0;


    [System.Serializable]
    public struct ArmorPiece
    {
        [SerializeField] public GameObject armorPrefab;
        [SerializeField] public Vector3 offset;
    }
    [System.Serializable]
    public struct ArmorView
    {
        [SerializeField] public ArmorPiece[] armorPieces;
    }
    public ArmorView[] armorViews;
    
    void Start() {
        // Spawns initialEnemies amount of enemies at Spawn
        for (int i = 0; i < initialEnemies; i++) {
            RandomSpawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // When Timer Finishes, Spawn Enemy
        timer += Time.deltaTime;
        if (timer >= God.GSM.SpawnDelay)
        {
            RandomSpawn();
            God.GSM.Speed += God.GSM.SpeedIncrease * Time.deltaTime;
            God.GSM.SpawnDelay += God.GSM.delayDecrease;
            timer = 0f;
        }
    }
   
    public void RandomSpawn()
    {
        float randomAngle = Random.Range(0f, 360f);
        Vector3 spawn = Quaternion.Euler(0, 0, randomAngle) * new Vector3(0, 1, 0) * 15f;
        GameObject enemy = null;
        if(randomAngle < 15f || randomAngle > 345f)
        {
            //FACING DOWN
            enemy = ArmorMeDaddy(0, spawn);
        }
        else if(randomAngle < 75f || randomAngle > 285f)
        {
            //DOWN ANGLE
            enemy = ArmorMeDaddy(1, spawn);
        }
        else if(randomAngle < 165f || randomAngle > 195f)
        {
            //SIDE
            enemy = ArmorMeDaddy(2, spawn);
        }
        else if(randomAngle < 105f || randomAngle > 255f)
        {
            //UP ANGLE
            enemy = ArmorMeDaddy(3, spawn);
        }
        else
        {
            //UP
            enemy = ArmorMeDaddy(4, spawn);
        }

        

        if (randomAngle > 180f)
        {
            enemy.transform.localScale = new Vector3(-0.13f, 0.13f, 0.13f);
        }
        else
        {
            enemy.transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
        }
    }

    GameObject ArmorMeDaddy(int inVal, Vector3 spawn)
    {
        GameObject enemy = Instantiate(armorViews[inVal].armorPieces[armorViews[inVal].armorPieces.Length-1].armorPrefab, spawn, Quaternion.identity);
        enemy.layer = LayerMask.NameToLayer("Enemy");

        List<int> numbs = new List<int>();
        int numberArmor = Random.Range(0, armorViews[inVal].armorPieces.Length - 3);
        for(int i = 0; i < armorViews[inVal].armorPieces.Length - 2; i++){numbs.Add(i);}
        for(int i = 0; i < numberArmor; i++)
        {
            int listIndex = Random.Range(0, numbs.Count);
            numbs.RemoveAt(listIndex);
        }
        for(int i = 0; i < numbs.Count; i++)
        {
            GameObject pieceofshitfuckingarmor = Instantiate(armorViews[inVal].armorPieces[numbs[i]].armorPrefab, spawn, Quaternion.identity);
            pieceofshitfuckingarmor.layer = LayerMask.NameToLayer("Armor");
            pieceofshitfuckingarmor.transform.SetParent(enemy.transform);
            pieceofshitfuckingarmor.transform.localPosition = armorViews[inVal].armorPieces[numbs[i]].offset;
        }
        enemy.AddComponent<EnemyMono>();
        enemyMono = enemy.GetComponent<EnemyMono>();
        enemyMono.target = GameObject.Find("Player");


        return enemy;
    }
}

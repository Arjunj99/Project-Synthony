using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public HealthBar healthBar;
    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.12f);
        for(int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                EnemyMono enemyMono = colliders[i].GetComponent<EnemyMono>();
                enemyMono.KillMePlease();
                healthBar.hp_change(-1);
            }
        }
    }
}

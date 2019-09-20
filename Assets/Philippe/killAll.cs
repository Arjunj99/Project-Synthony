using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killAll : powerUpBase, powerUpInterface
{
    private bool expanding = false;
    // Start is called before the first frame update
     void Start()
    {
        base.setTime(5);
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (expanding) {
    
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    public void powerUp()
    {
        //    GameObject[] objects = FindObjectsOfType<GameObject>();
        //  for(int i = 0; i < objects.Length; i++)
        //    {
        //        if(objects[i].layer == LayerMask.NameToLayer("Enemy"))
        //        {
        //            Destroy(objects[i]);
        //        }
        //    }
        expanding = true;
        StartCoroutine(timeKill());

    }

    public void OnTriggerEnter2D(Collider2D collider)
    {

        if(expanding)
        {
            if(collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collider.gameObject.GetComponent<EnemyMono>().KillMePlease();
            }
        }
    }

    private IEnumerator timeKill()
    {
        yield return new WaitForSeconds(1);
        base.powerUp();
    }
}

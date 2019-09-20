using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpBase : MonoBehaviour
{

    //=========================================================================================================
    // V A R I A B L E S
    int time;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        StartCoroutine(timeToDeath());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTime(int newTime)
    {
        time = newTime;
    }

    public IEnumerator timeToDeath()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    virtual protected void powerUp() {
        Destroy(gameObject);
    }
}

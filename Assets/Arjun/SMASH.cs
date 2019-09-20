using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMASH : powerUpBase, powerUpInterface
{
    Mouse mouse;
    // Start is called before the first frame update
    void Start()
    {
       mouse = GameObject.Find("Cursor").GetComponentInChildren<Mouse>();
        base.setTime(5);
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void powerUp()
    {
        mouse.toggleSmash();
        base.powerUp();
    }

    IEnumerator unSMASH()
    {
        yield return new WaitForSeconds(5);
        mouse.toggleSmash();
    }
}

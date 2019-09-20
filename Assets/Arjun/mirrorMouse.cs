using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mirrorMouse : powerUpBase, powerUpInterface
{
    private bool thisshouldntbeathingbutithastobehere = true;
    // Start is called before the first frame update
    void Start()
    {
        base.setTime(5);
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void powerUp()
    {
        if (thisshouldntbeathingbutithastobehere)
        {
            thisshouldntbeathingbutithastobehere = false;
            GameObject cursor = GameObject.Find("Cursor");
            GameObject mirror = Instantiate(cursor, transform.position, Quaternion.identity);
            mirror.GetComponentInChildren<Mouse>().teleportMouse(mirror.transform.position.x, mirror.transform.position.y);
            mirror.GetComponentInChildren<Mouse>().reverseMouse();
            mirror.GetComponentInChildren<Mouse>().triggerKill(5);
        }
        base.powerUp();
    }



}
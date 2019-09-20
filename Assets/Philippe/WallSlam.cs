using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlam : MonoBehaviour
{
    public AnimationCurve animationCurve;
    Vector3 startPos, endPos;
    float speed, time = 0f;
    public bool horizontal = true;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("Main").GetComponent<GameManager>();
        speed = 1f;// Random.Range(1f, 3f);

        //50/50 WALL VERTICAL OR HORIZONTAL
        if (horizontal)
        {
            speed = 12f;
            transform.position = new Vector3(15f, Random.Range(4.0f, -4.64f), 0);//Mathf.Sign(Random.Range(-1f, 1f)) * 15f, Random.Range(4.0f, -4.64f), 0);
            startPos = transform.position;
            endPos = transform.position;
            endPos.x = -endPos.x;
        }
        else
        {
            speed = 7f;
            transform.Rotate(Vector3.forward, 90);
            transform.position = new Vector3(Random.Range(-5.4f, 7.1f), 8f, 0);//Mathf.Sign(Random.Range(-1f, 1f)) * 6f, 0);
            startPos = transform.position;
            endPos = transform.position;
            endPos.y = -endPos.y;
        }
    }
    
    void Update()
    {
        time += gameManager.globalTimeDelta;
        transform.position = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(time /( (endPos-startPos).magnitude/ speed)));
        if(Vector3.Distance(transform.position, endPos) < 0.2f)
        {
            Destroy(gameObject);
        }
    }
}

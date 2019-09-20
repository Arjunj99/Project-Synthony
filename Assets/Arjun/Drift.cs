using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drift : MonoBehaviour
{
    private GameManager gameManager;
    private Vector3 finalPosition, startPosition;
    public Vector3 center;
    MeshRenderer meshRenderer;
    AnimationCurve animationCurve;
    private float speed = 20f, decay = 1f, time = 0f;

    private void Start()
    {
        gameManager = GameObject.Find("Main").GetComponent<GameManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        animationCurve = gameManager.driftCurve;
    }

    public void UpdatePosition(Vector3 mousePos)
    {
        startPosition = transform.position;
        finalPosition = center - mousePos;
    }

    void Update()
    {
        //move towards relative position at speed times time factor
        time += gameManager.globalTimeDelta;
        transform.position = Vector3.Lerp(startPosition, finalPosition, animationCurve.Evaluate(time));
        Color32 color = meshRenderer.material.color;
        meshRenderer.material.color = new Color32(color.r, color.g, color.b, (byte)Mathf.Lerp(255, 1, animationCurve.Evaluate(time)));
        if(time  > 1f)
        {
            Destroy(this);
        }

    }
}

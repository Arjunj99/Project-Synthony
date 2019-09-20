using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailScript : MonoBehaviour
{

    public TrailRenderer trail;
    // Start is called before the first frame update
    void Start()
    {
        trail.sortingLayerName = "Trail";
        trail.sortingOrder = 500;
        trail.sortingLayerName = "Character";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

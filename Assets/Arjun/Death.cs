using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour {
    // =========================================================================
    // VARAIBLES
    private Rigidbody2D rb; // RigidBody of Each Shape
    public float speed; // Speed Value of Each Shape
    float timer = 0; // Timer used for Update
    public Vector2 direction; // Direction of the Slice
    // =========================================================================

    // =========================================================================
    // FUNCTIONS
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //if (timer > 0.2f) // Actually causes velocity, but now that i think about it, i dont know if it has to be in this if looping thing cuz i used transform before :^]
        //{
        //    rb.velocity = direction / speed;
        //}
    }
    // =========================================================================
}

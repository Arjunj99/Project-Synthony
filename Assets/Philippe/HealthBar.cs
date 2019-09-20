using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public GameObject health_bar;
    public GameObject health_bar_lag;
    public GameObject cursor;
    public float hp_max = 8;
    public float hp_current = 8;
    public float currentTime = 0;
    public float delay = 0.2f;
    public float speed = 1f;
    private Coroutine hp_bar;
    private GameManager gameManager;
    private SpriteRenderer hp_bar_render, hp_bar_lag_render;


    private void Start()
    {
        gameManager = GameObject.Find("Main").GetComponent<GameManager>();
        hp_bar_render = health_bar.GetComponentInChildren<SpriteRenderer>();
        hp_bar_lag_render = health_bar_lag.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            hp_change(1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            hp_change(-1);
        }

    }

    public void hp_change(int amount)
    {
        hp_current += amount;
        if(hp_current <= 0)
        {
            if(cursor != null)
            {
                Destroy(cursor);
                gameManager.gameOver = true;
            }
        }
        hp_current = Mathf.Clamp(hp_current, 0, hp_max);
        health_bar.transform.localScale = new Vector3(hp_current / hp_max, 1, 1);
        if (amount > 0)
        {
            health_bar_lag.transform.localScale = health_bar.transform.localScale;
        }
        else
        {
            if (hp_bar != null)
            {
                StopCoroutine(hp_bar);
            }
            hp_bar = StartCoroutine(update_lag());
        }
    }


    public IEnumerator update_lag()
    {
        float elapsed = 0.0f;
        Vector3 start_scale = health_bar_lag.transform.localScale;
        while (health_bar_lag.transform.localScale != health_bar.transform.localScale)
        {
            elapsed += Time.deltaTime;
            
            if(elapsed > delay)
            {
                health_bar_lag.transform.localScale = Vector3.Lerp(start_scale, health_bar.transform.localScale, animationCurve.Evaluate(speed * (elapsed - delay)));
            }
            yield return null;
        }
    }

    public void ApplyStyle()
    {
        //hp_bar_render.color = Themes
        //hp_bar_lag_render = 
    }
}

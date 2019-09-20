using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    public AnimationCurve animationCurve;
    public GameObject energy_bar;
    public GameObject health_bar_lag;
    public GameObject cursor;
    private GameManager gameManager;
    public float max_energy = 8;
    public float current_energy = 8;
    public float currentTime = 0;
    public float delay = 1f;
    public float useSpeed = 2f;
    public float chargeSpeed = 2f;

    private void Start()
    {
        gameManager = GameObject.Find("Main").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(gameManager.timeSlow == true)
        {
            currentTime = 0;
            energy_change(-Time.deltaTime * useSpeed);
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime > delay)
            {
                energy_change(Time.deltaTime * chargeSpeed);
            }
        }

        energy_bar.transform.localScale = new Vector3(current_energy / max_energy, 1, 1);
    }

    public void energy_change(float amount)
    {
        current_energy += amount;
        current_energy = Mathf.Clamp(current_energy, 0, max_energy);
    }
}

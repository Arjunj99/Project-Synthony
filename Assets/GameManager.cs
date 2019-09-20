using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float globalTime = 0;
    public bool timeSlow = false;
    public float timeFactor = 0.02f;
    public float globalTimeDelta = 0;
    public EnergyBar energyBar;
    public AudioManager audioManager;
    public bool gameOver = false;
    public int highScore = 0;
    public AnimationCurve driftCurve;

    private void Start()
    {
        highScore = 0;
        audioManager = GetComponent<AudioManager>();
    }

    void Update()
    {
        if(gameOver == false)
        {
            if (timeSlow == true)
            {
                globalTimeDelta = Time.deltaTime * timeFactor;
            }
            else
            {
                globalTimeDelta = Time.deltaTime;
            }

            if (energyBar.current_energy < 0.001f)
            {
                timeSlow = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))// || Input.GetMouseButtonDown(0))
            {
                if (energyBar.current_energy > 0.001f)
                {
                    timeSlow = !timeSlow;
                }
            }

            globalTime += globalTimeDelta;
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", highScore);
            timeSlow = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource KillSFX;
    private int musicVolume;
    private int sfxVolume;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
        musicVolume = 3;//PlayerPrefs.GetInt("MVolume", 2);
        sfxVolume = 10;//PlayerPrefs.GetInt("SFXVolume", 7);

        MusicSource.volume = (float)musicVolume * 0.1f;
        KillSFX.volume = (float)sfxVolume * 0.1f;

        MusicSource.Play();
        MusicSource.loop = true;
    }


    public void OnEnemyKill()
    {
        if (KillSFX.isPlaying)
        {
            KillSFX.pitch += 0.02f;
            KillSFX.time = 0.5f;
        }
        else
        {
            KillSFX.pitch = 1.0f;
            KillSFX.Play();
            KillSFX.time = 0.5f;
        }
    }
}

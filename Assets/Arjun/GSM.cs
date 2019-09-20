using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GSM : MonoBehaviour {
    [Header("Public Variable")]
    public GameObject target;

    [Header("Spawn Settings")]
    public float SpawnDelay;
    public float Speed;
    public float SpeedIncrease;
    public float delayDecrease;
    public List<Sprite> armorSprites = new List<Sprite>();

    private void Awake() {
        God.GSM = this;
    }
}

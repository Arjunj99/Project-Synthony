using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Base, Shield, Arm, Helmet, Shield_Helmet, Arm_Helmet, Shield_Arm
}

public class Enemy {
    public Type type;
    public Vector3 spawnLocation;
    public bool isAlive;

    public Enemy (Type Type, Vector3 SpawnLocation, bool IsAlive) {
        type = Type;
        spawnLocation = SpawnLocation;
        isAlive = IsAlive;
    }
}

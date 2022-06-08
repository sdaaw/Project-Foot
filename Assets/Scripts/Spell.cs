using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Spell : MonoBehaviour
{

    public float movementSpeed;

    public string spellName;
    public string description;

    public float dotDamage;
    public float debuffDuration;

    public int rank;

    public float hitDamage;
    public float knockbackAmount;

    public float totalDamageDealt;
    public float dps;

    public Type spellType;

    public bool isFrozen;

    public GameManager gm;


    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        spellType = GetType();
    }

    void LateUpdate()
    {
        if(totalDamageDealt != 0 && !gm.isGameFrozen)
        {
            dps = totalDamageDealt / Time.realtimeSinceStartup;
        }
    }
}

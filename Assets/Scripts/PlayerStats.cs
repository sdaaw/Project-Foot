using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStats : MonoBehaviour
{


    public int playerLevel;

    public float playerCurrentExp;
    public float playerExpToLevel;

    public float playerMaxHealth;
    public float playerHealth;
    public float playerBaseDamage;

    public float movementSpeed;

    public float expIncreaseMultiplier;
    private int orbitAddIndex = 1;

    public int MonstersKilled;

    public float expApproachDist;

    public bool isGod;

    public GameManager gm;

    public ProgressionManager progManager;



    // Start is called before the first frame update

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        progManager = gm.GetComponent<ProgressionManager>();
    }

    public void GainExperience(float amount)
    {
        playerCurrentExp += amount;
        if(playerCurrentExp >= playerExpToLevel)
        {
            //LevelUp();
            float remainingExp = amount - playerExpToLevel;
            if(remainingExp > 0)
            {
                GainExperience(remainingExp);
            }
        }
        gm.UpdateExpText(playerCurrentExp.ToString("F0") + "/" + playerExpToLevel.ToString("F0"));
        float sliderChange = amount / playerExpToLevel * 100f / 100f;
        progManager.UpdateProgress(sliderChange);
    }

    public void TakeDamage(float amount)
    {
        if(!isGod || !gm.isGameFrozen)
        {
            if (playerHealth > 0)
            {
                playerHealth -= amount;
                gm.UpdateHealthText(playerHealth.ToString("F0"));

            }
            if (playerHealth <= 0)
            {
                GetComponent<PlayerController>().Dead();
                gm.UpdateHealthText("OH NWYOOOOOO....");
                gm.GameOverScreen();
            }
        }
    }

    IEnumerator UpdateBarVisual()
    {
        yield return new WaitForSeconds(0.1f);
    }


    public void LevelUp()
    {
        orbitAddIndex++;
        if(orbitAddIndex == 5)
        {
            orbitAddIndex = 0;
            //gm.orbitingCube.AddOrbitingObject();
            //gm.orbitingCube.player = gm.player; //pass the player gameobject
            //gm.orbitingCube.orbitRadius += 2;
        }
        playerLevel++;
        playerExpToLevel *= expIncreaseMultiplier;
        playerCurrentExp = 0;
        playerBaseDamage += 1;
        //gm.orbitingCube.AddOrbitingObject();
        //gm.orbitingCube.orbitRadius += 0.5f;
        gm.UpdateLevelText("Lv. " + playerLevel);
        //gm.SpawnBounceCube();
        gm.FreezeGame();
    }
}

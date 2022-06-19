using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotBonus : MonoBehaviour
{
    public float bonusLuck;
    public TMP_Text bonusAnnouncementText;
    public TMP_Text bonusInfoText;

    public int freeSpins;
    private int _ogFreeSpins;

    public bool isBonusActive;
    private bool _isBonusAvailable;

    public int goodObjects;

    private CasinoManager _casinoManager;

    public int bonusPayout;
    public int bonusMultiplier;

    private void Start()
    {
        _casinoManager = GetComponent<CasinoManager>();
    }
    public void StartBonus(int bonusLevel)
    {
        goodObjects = 0;
        _isBonusAvailable = true;
        freeSpins = bonusLevel * 2;
        _ogFreeSpins = freeSpins;
        bonusAnnouncementText.text = freeSpins.ToString() + " FREE SPINS AVAILABLE\nPress SPACE to begin";
        bonusInfoText.text = "Spins left: " + freeSpins + "\nMultiplier: " + bonusMultiplier + "x";
    }

    public void FinishBonus()
    {
        isBonusActive = false;
        _isBonusAvailable = false;
        bonusAnnouncementText.text = "YOU WON " + bonusPayout + " in " + _ogFreeSpins.ToString() + " SPINS" +"\nPress SPACE to continue.";
        freeSpins = 0;
        bonusPayout = 0;
        bonusMultiplier = 0;
    }

    public void Update()
    {   
        if(_isBonusAvailable)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                isBonusActive = true;
                bonusAnnouncementText.text = "";
            }
        }
    }
}

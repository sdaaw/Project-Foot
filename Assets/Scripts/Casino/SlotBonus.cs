using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotBonus : MonoBehaviour
{
    public float bonusLuck;
    public GameObject bonusObject;
    public TMP_Text bonusAnnouncementText;

    public int freeSpins;

    private bool _isBonusRolling;
    public void StartBonus(int bonusLevel)
    {
        _isBonusRolling = true;
        freeSpins = bonusLevel * 2;
        bonusAnnouncementText.text = freeSpins.ToString() + " FREE SPINS AVAILABLE\nPress SPACE to begin";
        StartCoroutine(FreeSpinReady());
    }

    public void RollBonus()
    {

    }

    IEnumerator FreeSpinReady()
    {
        float timer = 0;
        while(!_isBonusRolling)
        {
            timer += 1 * Time.deltaTime;
            if(Input.GetKeyUp(KeyCode.Space))
            {
                _isBonusRolling = true;
                RollBonus();
            }
            yield return null;
        }
    }

}

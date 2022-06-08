using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CasinoManager : MonoBehaviour
{
    public List<SlotLine> SlotLines = new List<SlotLine>();
    public GameObject slotLineObject;

    public GameObject slotsParentObject;
    public GameObject gridObject;

    private AudioSource _audioSource;
    public AudioClip rollAudioClip;

    public float turboModifier;

    private bool _isTurbo;

    public float rollDelay;


    public TMP_Dropdown dropdownTest;


    private struct RollResult
    {
        public int commonCount;
        public int uncommonCount;
        public int rareCount;
        public int ultraRareCount;
        public int legendaryCount;
    }

    public float CommonWinMult;
    public float UncommonWinMult;
    public float RareWinMult;
    public float UltraRareWinMult;
    public float LegendaryWinMult;

    public int CommonWinObjectCount;
    public int UncommonWinObjectCount;
    public int RareWinMObjectCount;
    public int UltraRareWinObjectCount;
    public int LegendaryWinObjectCount;


    public float WinAmount;
    public float BetAmount;

    private float _houseBalance;
    private float _myBalance;
    private float _moneyLost;
    private float _profitPercentage;

    public bool isAutoRoll;

    public TMP_Text winText;
    public TMP_Text statText;

    public int slotLineCount;
    void Start()
    {
        List<string> resStrings = new List<string>();
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            resStrings.Add(Screen.resolutions[i].ToString());
        } 
        dropdownTest.AddOptions(resStrings);


        _audioSource = GetComponent<AudioSource>();
        float screenWidthScale = 8 / slotLineCount;
        float xBuffer = 0;
        for(int i = 0; i < slotLineCount; i++)
        {
            xBuffer = (screenWidthScale * i) - (10 / (slotLineCount * 2));

            GameObject a = Instantiate(slotLineObject, new Vector3(xBuffer, 0, 5), Quaternion.identity);
            a.transform.SetParent(slotsParentObject.transform);
            SlotLines.Add(a.GetComponent<SlotLine>());
        }
        slotsParentObject.transform.position = new Vector3(0 - (gridObject.transform.localScale.x), 0, 2);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) && !isAutoRoll)
        {
            isAutoRoll = true;
            StartCoroutine(RollLines());
        }
    }

    public void GatherRollResult()
    {
        List<SlotObject> resultObjects = new List<SlotObject>();
        foreach(SlotLine line in SlotLines)
        {
            foreach(GameObject o in line.currentLineObjects)
            {
                resultObjects.Add(o.GetComponent<SlotObject>());
            }
        }
        RollResult result = new RollResult();
        foreach(SlotObject o in resultObjects)
        {
            if(o.objectData.type == SlotObject.SlotObjectType.Common)
            {
                result.commonCount++;
            }
            if (o.objectData.type == SlotObject.SlotObjectType.Uncommon)
            {
                result.uncommonCount++;
            }
            if (o.objectData.type == SlotObject.SlotObjectType.Rare)
            {
                result.rareCount++;
            }
            if (o.objectData.type == SlotObject.SlotObjectType.UltraRare)
            {
                result.ultraRareCount++;
            }
            if (o.objectData.type == SlotObject.SlotObjectType.Legendary)
            {
                result.legendaryCount++;
            }
        }

        if(result.commonCount >= CommonWinObjectCount)
        {
            foreach(SlotObject o in resultObjects)
            {
                if(o.objectData.type == SlotObject.SlotObjectType.Common)
                {
                    o.DoVisual();
                }
            }
            WinAmount += CommonWinMult * BetAmount;
        }
        if (result.uncommonCount >= UncommonWinObjectCount)
        {
            foreach (SlotObject o in resultObjects)
            {
                if (o.objectData.type == SlotObject.SlotObjectType.Uncommon)
                {
                    o.DoVisual();
                }
            }
            WinAmount += UncommonWinMult * BetAmount;
        }
        if (result.rareCount >= RareWinMObjectCount)
        {
            foreach (SlotObject o in resultObjects)
            {
                if (o.objectData.type == SlotObject.SlotObjectType.Rare)
                {
                    o.DoVisual();
                }
            }
            WinAmount += RareWinMult * BetAmount;
        }
        if (result.ultraRareCount >= UltraRareWinObjectCount)
        {
            foreach (SlotObject o in resultObjects)
            {
                if (o.objectData.type == SlotObject.SlotObjectType.UltraRare)
                {
                    o.DoVisual();
                }
            }
            WinAmount += UltraRareWinMult * BetAmount;
        }
        if (result.legendaryCount >= LegendaryWinObjectCount)
        {
            foreach (SlotObject o in resultObjects)
            {
                if (o.objectData.type == SlotObject.SlotObjectType.Legendary)
                {
                    o.DoVisual();
                }
            }
            WinAmount += LegendaryWinMult * BetAmount;
        }

        _myBalance += WinAmount;
        if (WinAmount > 0)
        {
            _houseBalance -= WinAmount;
        }
        _houseBalance += BetAmount;

        if (WinAmount == 0)
        {
            winText.text = "No win";
        } else
        {
            winText.text = WinAmount.ToString() + "!!";
        }
        WinAmount = 0;
        _moneyLost += BetAmount;

        _profitPercentage = (_myBalance / _moneyLost) * 100;

        statText.text = "House Profit: " + _houseBalance.ToString() + "\nMy Profit: " + _myBalance.ToString() + "\nProfit%: " + _profitPercentage.ToString("F2");
        StartCoroutine(AutoRoll());
    }

    IEnumerator AutoRoll()
    {
        isAutoRoll = true;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RollLines());
    }



    IEnumerator RollLines()
    {
        _myBalance -= BetAmount;
        winText.text = "";
        if (!SlotLines[0].GetComponent<SlotLine>().isRolling)
        {
            _audioSource.PlayOneShot(rollAudioClip);
            foreach (SlotLine line in SlotLines)
            {
                line.RollLine();
                yield return new WaitForSeconds(rollDelay);
            }
        }
        while(SlotLines[slotLineCount - 1].isRolling)
        {
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        GatherRollResult();
    }
}

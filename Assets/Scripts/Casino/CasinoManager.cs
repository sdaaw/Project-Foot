using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class CasinoManager : MonoBehaviour
{



    [Serializable]
    public struct OddsStruct
    {
        public SlotObject.SlotObjectType type;
        public float odds;
        public int winCount;
        public float winMult;
        public GameObject obj;
    }

    public List<OddsStruct> odds = new List<OddsStruct>();

    public List<SlotLine> SlotLines = new List<SlotLine>();
    public GameObject slotLineObject;

    public GameObject slotsParentObject;
    public GameObject gridObject;

    private AudioSource _audioSource;
    public AudioClip rollAudioClip;

    public float turboModifier;

    private bool _isInBonus;

    public float rollDelay;

    public List<SlotResult> slotResults = new List<SlotResult>();

    public TMP_Dropdown dropdownTest;

    private SlotBonus _slotBonus;


    private struct RollResult
    {
        public SlotObject.SlotObjectType type;
        public int objCount;
    }

    private List<RollResult> rollResults = new List<RollResult>();


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

        SlotObject sObj;
        foreach(OddsStruct o in odds)
        {
            sObj = o.obj.GetComponent<SlotObject>();
            sObj.objectData.type = o.type;
        }
        /*
        List<string> resStrings = new List<string>();
        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            resStrings.Add(Screen.resolutions[i].ToString());
        } 
        dropdownTest.AddOptions(resStrings);
        */
        _slotBonus = GetComponent<SlotBonus>();


        _audioSource = GetComponent<AudioSource>();
        float screenWidthScale = 8 / slotLineCount;
        float xBuffer = 0;
        for(int i = 0; i < slotLineCount; i++)
        {
            xBuffer = (screenWidthScale * i) - (10 / (slotLineCount * 2));

            GameObject a = Instantiate(slotLineObject, new Vector3(xBuffer, 0, 5), Quaternion.identity);
            a.transform.SetParent(slotsParentObject.transform);
            a.GetComponent<SlotLine>().cManager = this;
            SlotLines.Add(a.GetComponent<SlotLine>());
        }
        slotsParentObject.transform.position = new Vector3(0 - (gridObject.transform.localScale.x), 0, 2);


    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) && !isAutoRoll && !_isInBonus)
        {
            //isAutoRoll = true;
            StartCoroutine(RollLines());
        }
    }

    public void PayoutWin(GameObject[] winObjects)
    {

    }

    public void GatherRollResult()
    {
        slotResults = new List<SlotResult>();
        List<SlotObject> resultObjects = new List<SlotObject>();
        bool found = false;
        foreach(SlotLine sl in SlotLines)
        {
            foreach(GameObject a in sl.currentLineObjects)
            {
                SlotResult slotResult = new SlotResult();
                resultObjects.Add(a.GetComponent<SlotObject>());
                foreach(SlotResult r in slotResults)
                {
                    if(r.type == a.GetComponent<SlotObject>().objectData.type)
                    {
                        r.objCount++;
                        found = true;
                    }
                }
                if(!found)
                {
                    slotResult.type = a.GetComponent<SlotObject>().objectData.type;
                    slotResult.objCount = 1;
                    slotResults.Add(slotResult);
                }
                found = false;
            }
        }

        foreach(OddsStruct o in odds)
        {
            foreach(SlotResult r in slotResults)
            {
                if(o.type == r.type && r.objCount >= o.winCount)
                {
                    //win?
                    foreach(SlotObject so in resultObjects)
                    {
                        if(so.objectData.type == o.type)
                        {
                            so.DoVisual();
                            WinAmount = o.winMult * BetAmount;
                        }
                    }
                }
            }
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

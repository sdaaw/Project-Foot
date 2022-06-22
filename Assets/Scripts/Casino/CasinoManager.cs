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

    public string CurrencyName;

    private struct RollResult
    {
        public SlotObject.SlotObjectType type;
        public int objCount;
    }

    public List<OddsStruct> odds = new List<OddsStruct>();

    public List<GameObject> bonusObjects;


    public List<SlotLine> SlotLines = new List<SlotLine>();
    public GameObject slotLineObject;

    public GameObject slotsParentObject;
    public GameObject gridObject;

    private AudioSource _audioSource;
    public AudioClip rollAudioClip;

    public float turboModifier;

    public float rollDelay;

    public List<SlotResult> slotResults = new List<SlotResult>();

    public TMP_Dropdown dropdownTest;

    public SlotBonus slotBonus;

    private List<RollResult> rollResults = new List<RollResult>();


    public float WinAmount;
    public float BetAmount;

    private float _houseBalance;
    private float _myBalance;
    private float _moneyLost;
    private float _profitPercentage;

    public float MAXBET;

    private float _myCurrentBet;


    public float rollTime;

    public bool isAutoRoll;

    public TMP_Text winText;
    public TMP_Text statText;
    public TMP_Text betText;
    public TMP_Text balanceText;

    public int slotLineCount;

    private int _totalRolls;
    private float _biggestWin = 0;

    public bool isRolling;
    public bool simulationMode;
    void Start()
    {
        _myBalance = 5000;
        _myCurrentBet = BetAmount;
        balanceText.text = "Balance: " + "$" + _myBalance;
        betText.text = "Bet: " + "$" + BetAmount;
        //Application.targetFrameRate = 500;


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
        slotBonus = GetComponent<SlotBonus>();


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
        if(Input.GetKeyDown(KeyCode.Space) && !isAutoRoll && !isRolling)
        {
            if(_myBalance < BetAmount)
            {
                return;
            }
            //isAutoRoll = true;
            isRolling = true;
            StartCoroutine(RollLines());
        }
        if(Input.GetKey(KeyCode.Space) && simulationMode)
        {
            StartCoroutine(RollLines());
        }
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
        int bonusCount = 0;
        slotBonus.goodObjects = 0;

        if (slotBonus.isBonusActive)
        {
            foreach (SlotObject so in resultObjects)
            {
                if (so.objectData.type == SlotObject.SlotObjectType.BonusGood)
                {
                    slotBonus.bonusMultiplier += 1;
                }
            }
        }
        else
        {
            foreach (OddsStruct o in odds)
            {
                foreach (SlotResult r in slotResults)
                {
                    if (o.type == r.type && r.objCount >= o.winCount)
                    {
                        //win?
                        foreach (SlotObject so in resultObjects)
                        {
                            if (so.objectData.type == o.type)
                            {
                                if (so.objectData.type == SlotObject.SlotObjectType.Legendary)
                                {
                                    bonusCount++;
                                }
                                so.DoVisual();
                                int mult = 1;
                                if (r.objCount - o.winCount != 0)
                                {
                                    mult = r.objCount - o.winCount;
                                }
                                WinAmount = o.winMult * BetAmount * (mult + 1);
                            }
                        }
                    }
                }
            }
        }



        if (slotBonus.isBonusActive)
        {
            slotBonus.bonusInfoText.text = "Spins left: " + slotBonus.freeSpins + "\nMultiplier: " + slotBonus.bonusMultiplier + "x";
            if (slotBonus.freeSpins == 0)
            {
                WinAmount = BetAmount * slotBonus.bonusMultiplier;
                slotBonus.bonusPayout = (int)WinAmount;
                slotBonus.FinishBonus();
            }
        }
        if(!slotBonus.isBonusActive)
        {
            _myBalance += WinAmount;
            if (WinAmount > 0)
            {
                _houseBalance -= WinAmount;
            }
            _houseBalance += BetAmount;

            if (WinAmount == 0 && bonusCount < 3)
            {
                winText.text = "No win";
            }
            else if (WinAmount > 0)
            {
                winText.text = "$" + WinAmount.ToString() + "!!";
            }
            if (_biggestWin < WinAmount)
            {
                _biggestWin = WinAmount;
            }
            _moneyLost += BetAmount;
            _totalRolls++;
            _profitPercentage = (_myBalance / _moneyLost) * 100;
        }
        WinAmount = 0;

        statText.text = "House Profit: $" + _houseBalance.ToString() + "\nMy Profit: $" + _myBalance.ToString() + "\nProfit%: " + _profitPercentage.ToString("F2") + "\nRolls: " + _totalRolls + "\nBiggest Win: $" + _biggestWin;
        if(isAutoRoll)
        {
            StartCoroutine(AutoRoll());
        }
        if(simulationMode)
        {
            foreach (SlotLine sl in SlotLines)
            {
                sl.isSimRolling = false;
            }
        }
        isRolling = false;

        if(bonusCount >= 3 && !slotBonus.isBonusActive)
        {
            slotBonus.StartBonus(bonusCount);
        }
    }

    IEnumerator AutoRoll()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(RollLines());
    }



    IEnumerator RollLines()
    {
        if (slotBonus.isBonusActive)
        {
            slotBonus.freeSpins--;
            slotBonus.bonusInfoText.text = "Spins left: " + slotBonus.freeSpins + "\nMultiplier: " + slotBonus.bonusMultiplier + "x";
        }else
        {
            slotBonus.bonusAnnouncementText.text = "";
            slotBonus.bonusInfoText.text = "";
            _myBalance -= BetAmount;
            balanceText.text = "Balance: " + "$" + _myBalance;
        }
        if(!simulationMode)
        {
            winText.text = "";
            if (!SlotLines[0].GetComponent<SlotLine>().isRolling)
            {
                float idx = 0.3f;
                _audioSource.PlayOneShot(rollAudioClip);
                foreach (SlotLine line in SlotLines)
                {
                    idx += 0.1f;
                    line.rollTime = rollTime + idx;
                    line.RollLine();
                    yield return new WaitForSeconds(rollDelay);
                }
            }
            while (SlotLines[slotLineCount - 1].isRolling)
            {
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForSeconds(0.5f);
        } else
        {
            foreach (SlotLine line in SlotLines)
            {
                if(!line.isSimRolling)
                {
                    line.RollInstantLine();
                    yield return null;
                }
            }
        }
        GatherRollResult();
    }

    public void IncreaseBet()
    {

        if (!slotBonus.isBonusAvailable)
        {
            if (_myCurrentBet < MAXBET)
            {
                _myCurrentBet += 10;
                BetAmount = _myCurrentBet;
                betText.text = "Bet: " + "$" + BetAmount;
            }
        }
    }
    public void DecreaseBet()
    {
        if (!slotBonus.isBonusAvailable)
        {
            if (_myCurrentBet > 0)
            {
                _myCurrentBet -= 10;
                BetAmount = _myCurrentBet;
                betText.text = "Bet: " + "$" + BetAmount;
            }
        }
    }

}

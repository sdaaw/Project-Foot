using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotLine : MonoBehaviour
{

    public List<GameObject> slotObjects;

    public List<GameObject> currentLineObjects;

    public List<GameObject> gridObjects;

    [SerializeField]
    private GameObject gridObject;

    public float moveSpeed;
    private float _ogMoveSpeed;

    public float slowdownSpeed;

    public float rollTime;

    public float timeBetweenObject;

    public float spawnYPadding;

    public float stopEasing;

    public bool isRolling;

    [HideInInspector]
    public Vector3 startPos;
    public Vector3 direction;

    public float maxLineLength;

    public float lineGap;

    public int lineObjectCount;


    public float rollDuration;


    private AudioSource _audioSource;
    public AudioClip audioClip;

    public CasinoManager cManager;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        FillLines(SlotObject.SlotObjectType.Legendary);
        _ogMoveSpeed = moveSpeed;
        startPos.x = transform.position.x;
        moveSpeed = 0;
        SpawnGrid(lineObjectCount);
    }

    public void SpawnGrid(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject a = Instantiate(gridObject, new Vector3(transform.position.x, transform.position.y + i, transform.position.z), Quaternion.identity);
            a.transform.SetParent(transform);
            gridObjects.Add(a);
        }
    }

    public void AddSlotObject(float yPos, SlotObject.SlotObjectType type)
    {
        SlotObject so;
        foreach(GameObject a in slotObjects)
        {
            so = a.GetComponent<SlotObject>();
            if(type == so.objectData.type)
            {
                GameObject b = Instantiate(a, new Vector3(transform.position.x, yPos, transform.position.z), Quaternion.identity);
                currentLineObjects.Add(b);
                b.transform.SetParent(transform);
            }
        }
    }

    private void FillLines(SlotObject.SlotObjectType type)
    {
        for(int i = 0; i < lineObjectCount; i++)
        {
            AddSlotObject(i, type);
        }
    }

    public GameObject GetSlotObjectByOdds(float odds)
    {
        float closest = 9999999f;
        GameObject rObj = null;

        foreach(CasinoManager.OddsStruct o in cManager.odds)
        {
            float objDiff = Mathf.Abs(odds - o.odds);
            if(objDiff < closest)
            {
                closest = objDiff;
                rObj = o.obj;
            }
        }
        return rObj;
    }

    private void HandleObjectClearing()
    {
        float rNum = Random.Range(0f, 1000f);
        for (int i = 0; i < currentLineObjects.Count; i++)
        {
            currentLineObjects[i].transform.position += direction * moveSpeed;
            if (currentLineObjects[i].transform.position.y < gridObjects[0].transform.position.y - (gridObjects[0].transform.localScale.y / 2) + spawnYPadding)
            {
                Destroy(currentLineObjects[i]);
                currentLineObjects.Remove(currentLineObjects[i]);
                GameObject a = Instantiate(GetSlotObjectByOdds(rNum));
                a.transform.SetParent(transform);
                currentLineObjects.Add(a);
                a.transform.position = new Vector3(transform.position.x, gridObjects[gridObjects.Count - 1].transform.position.y + (gridObjects[gridObjects.Count - 1].transform.localScale.y / 2) - spawnYPadding, transform.position.z);
            }
        }
    }

    IEnumerator StartRoll()
    {
        float time = 0;
        //float randTime = Random.Range(2f, 5f);
        float randTime = rollTime;
        while (time < randTime)
        {
            while (moveSpeed < _ogMoveSpeed)
            {
                moveSpeed += slowdownSpeed;
                HandleObjectClearing();
                yield return new WaitForSeconds(slowdownSpeed);
            }
            time += 1f * Time.deltaTime;
            HandleObjectClearing();
            yield return null;
        }
        while (moveSpeed > 0f)
        {
            moveSpeed -= slowdownSpeed;
            HandleObjectClearing();
            yield return new WaitForSeconds(slowdownSpeed);
        }
        foreach (GameObject a in currentLineObjects)
        {
            SnapToClosestGrid(a);
        }
        isRolling = false;
    }

    public void SnapToClosestGrid(GameObject o)
    {
        float bestDist = 99f;
        GameObject bestObject = null;
        foreach(GameObject a in gridObjects)
        {
            if(Vector3.Distance(a.transform.position, o.transform.position) < bestDist) 
            {
                if(!a.GetComponent<SlotGrid>().isTaken)
                {
                    bestDist = Vector3.Distance(a.transform.position, o.transform.position);
                    bestObject = a;
                    a.GetComponent<SlotGrid>().isTaken = true;
                }
            }
        }
        o.transform.position = new Vector3(o.transform.position.x, bestObject.transform.position.y, o.transform.position.z);
        _audioSource.PlayOneShot(audioClip);
    }

    private void ClearGridFlags()
    {
        foreach(GameObject a in gridObjects)
        {
            a.GetComponent<SlotGrid>().isTaken = false;
        }
    }

    public void RollLine()
    {
        ClearGridFlags();
        moveSpeed = 0;
        if (!isRolling)
        {
            isRolling = true;
            StartCoroutine(StartRoll());
        }
    }

    public Vector3 GetObjectPosition(int idx)
    {
        return new Vector3(gridObjects[idx].transform.position.x, transform.position.y, transform.position.z);
    }
}

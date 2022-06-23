using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmDirector : MonoBehaviour
{
    public float spawnRate;
    private float timer;

    public List<GameObject> monsterList = new List<GameObject>();
    public List<GameObject> spawnPoints = new List<GameObject>();

    public float groupOffset;

    public float boxBound;

    public float healthIncreaseMultiplierPerLevel; //hehe

    public int monsterCountLimit;

    public bool allowSpawning;

    private GameManager _gm;
    // Start is called before the first frame update
    void Start()
    {
        _gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!_gm.isGameFrozen)
        {
            if(allowSpawning)
            {
                timer += 1 * Time.deltaTime;
                if (timer > spawnRate)
                {
                    //SpawnMonsterInRandomArea(monsterList[Random.Range(0, monsterList.Count)]);
                    SpawnInGroup(monsterList[1], 20);
                    timer = 0;
                }
            }
        }
    }

    public void SpawnInGroup(GameObject monster, int amount)
    {
        if (_gm.monstersAlive.Count + amount > monsterCountLimit)
        {
            return;
        }
        float minDistance = 50f;
        float maxDistance = 55f;
        float distance = Random.Range(minDistance, maxDistance);
        float angle = Random.Range(-Mathf.PI, Mathf.PI);

        Vector3 spawnPosition = _gm.player.transform.position;
        spawnPosition += new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
        spawnPosition.x = Mathf.Clamp(spawnPosition.x, _gm.player.transform.position.x - boxBound, _gm.player.transform.position.x + boxBound);
        spawnPosition.y = 1;
        spawnPosition.z = Mathf.Clamp(spawnPosition.z, _gm.player.transform.position.z - boxBound, _gm.player.transform.position.z + boxBound);
        for(int i = 0; i < amount; i++)
        {
            Vector3 spawnOffset = new Vector3(spawnPosition.x + Random.Range(-groupOffset, groupOffset), 0, spawnPosition.y + Random.Range(-groupOffset, groupOffset));
            GameObject a = Instantiate(monster, spawnPosition + spawnOffset, Quaternion.identity);
            a.GetComponent<Entity>().maxHealth = a.GetComponent<Entity>().maxHealth * (_gm.pStats.playerLevel * healthIncreaseMultiplierPerLevel);
            GetComponent<GameManager>().monstersAlive.Add(a);
        }
    }

    public void SpawnMonsterInRandomArea(GameObject monster)
    {
        if(_gm.monstersAlive.Count > monsterCountLimit)
        {
            return;
        }
        float minDistance = 50f;
        float maxDistance = 55f;
        float distance = Random.Range(minDistance, maxDistance);
        float angle = Random.Range(-Mathf.PI, Mathf.PI);

        Vector3 spawnPosition = _gm.player.transform.position;
        spawnPosition += new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
        spawnPosition.x = Mathf.Clamp(spawnPosition.x, _gm.player.transform.position.x - boxBound, _gm.player.transform.position.x + boxBound);
        spawnPosition.y = 1;
        spawnPosition.z = Mathf.Clamp(spawnPosition.z, _gm.player.transform.position.z - boxBound, _gm.player.transform.position.z + boxBound);


        //GameObject a = Instantiate(monster, spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position, Quaternion.identity);
        GameObject a = Instantiate(monster, spawnPosition, Quaternion.identity);
        a.GetComponent<Entity>().maxHealth = a.GetComponent<Entity>().maxHealth * (_gm.pStats.playerLevel * healthIncreaseMultiplierPerLevel);
        GetComponent<GameManager>().monstersAlive.Add(a);
    }
}

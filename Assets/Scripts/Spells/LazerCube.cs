using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCube : Spell
{

    public int cubeCount;
    public GameObject lazerCubeObject;
    public float interval;

    private float _timer;

    public List<GameObject> lazerCubes = new List<GameObject>();

    public bool isActive = false;

    private Vector3 _updatedPos;

    private float _curveTimer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer += 1 * Time.deltaTime;
        if(_timer > interval)
        {
            _timer = 0;
            SpawnCubes();
        }
        if(lazerCubes.Count != 0)
        {
            for (int i = 0; i < lazerCubes.Count; i++)
            {
                _updatedPos = new Vector3(gm.player.transform.position.x + Mathf.Sin(i * 2) * 5, 1, gm.player.transform.position.z + Mathf.Cos(i * 2) * 10);
                lazerCubes[i].transform.position = _updatedPos;
            }
        }
    }

    public void SpawnCubes()
    {
        Vector3 spawnPos;
        for(int i = 0; i < cubeCount; i++)
        {
            spawnPos = new Vector3(gm.player.transform.position.x + Mathf.Sin(i * 2) * i, 1, gm.player.transform.position.z + Mathf.Cos(i * 2) * i);
            GameObject a = Instantiate(lazerCubeObject, spawnPos, Quaternion.identity);
            lazerCubes.Add(a);
            a.transform.localScale = new Vector3(Random.Range(0.8f, 1f), Random.Range(0.8f, 1f), Random.Range(0.8f, 1f));
        }
    }
}

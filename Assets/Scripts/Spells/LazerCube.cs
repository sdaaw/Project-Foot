using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCube : Spell
{

    public GameObject lazerCubeObject;
    public float interval;

    private float _timer;

    public List<GameObject> lazerCubes = new List<GameObject>();

    public bool isActive = false;

    private Vector3 _updatedPos;

    public int cubeCount;

    [Range(-0.3f, 0.3f)]
    public float galaxySpacingX;
    [Range(-0.3f, 0.3f)]
    public float galaxySpacingY;
    public float galaxyRotationX;
    public float galaxyRotationY;
    public Color colorProgress;
    public float galaxyColorProgressIntensity;
    public float galaxySpeed;

    public Vector3 cubeScale;
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
            Renderer r;
            for (int i = 0; i < lazerCubes.Count; i++)
            {
                _updatedPos = new Vector3(


                    gm.player.transform.position.x + Mathf.Sin(i + 1 * (-Time.time * galaxySpeed * galaxyRotationX)) * (i * galaxySpacingX), 
                    1, 
                    gm.player.transform.position.z + Mathf.Cos(i + 1 * (Time.time * galaxySpeed * galaxyRotationY)) * (i * galaxySpacingY)
                    );




                r = lazerCubes[i].GetComponent<Renderer>();
                r.material.color = lazerCubes[i].GetComponent<LazerCubeProperties>().MyColor + colorProgress * (i * galaxyColorProgressIntensity);
                /*r.material.color = new Color(
                    lazerCubes[i].GetComponent<LazerCubeProperties>().MyColor.r + (i * 0.02f), 
                    lazerCubes[i].GetComponent<LazerCubeProperties>().MyColor.g + (i * 0.01f),
                    lazerCubes[i].GetComponent<LazerCubeProperties>().MyColor.b + (i * 0.01f));*/
                lazerCubes[i].transform.position = _updatedPos;
            }
        }
    }

    public void SpawnCubes()
    {
        Vector3 spawnPos;
        for(int i = 0; i < cubeCount; i++)
        {
            spawnPos = new Vector3(


                    gm.player.transform.position.x + Mathf.Sin(i + 1 * (-Time.time * galaxySpeed * galaxyRotationX)) * (i * galaxySpacingX),
                    1,
                    gm.player.transform.position.z + Mathf.Sin(i + 1 * (Time.time * galaxySpeed * galaxyRotationY)) * (i * galaxySpacingY));
            GameObject a = Instantiate(lazerCubeObject, spawnPos, Quaternion.identity);
            lazerCubes.Add(a);
            a.transform.localScale = cubeScale;
        }
    }
}

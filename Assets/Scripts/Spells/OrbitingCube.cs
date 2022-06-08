using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingCube : Spell
{

    public GameObject orbitObject;

    private float _currAngle;

    public List<GameObject> orbitingObjects = new List<GameObject>();

    public GameObject player;

    public float orbitRadius;
    public float orbitSpeed;
    private float _ogOrbitRadius;
    private float _ogOrbitSpeed;

    public bool isActive;
    void Start()
    {
        _ogOrbitRadius = orbitRadius;
        _ogOrbitSpeed = orbitSpeed;
        player = gm.player;
    }

    private void FixedUpdate()
    {
        if (isActive && !gm.isGameFrozen)
        {
            OrbitAround(orbitRadius, orbitSpeed);
        }
    }

    public void AddOrbitingObject()
    {
        if (orbitingObjects.Count > 5) 
        {
            hitDamage += 1;
            return;
        }

        GameObject o = Instantiate(orbitObject, Vector3.zero, Quaternion.identity);
        print("added object");
        orbitingObjects.Add(o);
        float theta = (Mathf.PI * 2) / orbitingObjects.Count;
        float angle;
        for (int i = 0; i < orbitingObjects.Count; i++)
        {
            angle = theta * i;
            orbitingObjects[i].GetComponent<OrbitCubeProperties>().currOrbitAngle = angle;
        }
    }

    public void ClearOrbitCubes()
    {
        foreach(GameObject s in orbitingObjects)
        {
            Destroy(s);
        }
        orbitingObjects = new List<GameObject>();
        isActive = false;
    }

    public void OrbitAround(float radius, float speed)
    {
        if (orbitingObjects.Count != 0)
        {
            foreach (GameObject o in orbitingObjects)
            {
                o.GetComponent<OrbitCubeProperties>().currOrbitAngle += speed * Time.deltaTime;
                Vector3 offset = new Vector3(Mathf.Sin(o.GetComponent<OrbitCubeProperties>().currOrbitAngle), 0, Mathf.Cos(o.GetComponent<OrbitCubeProperties>().currOrbitAngle)) * radius;
                o.transform.position = player.transform.position + offset;
            }
        }
    }
}

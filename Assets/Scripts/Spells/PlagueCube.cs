using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueCube : Spell
{
    // Start is called before the first frame update

    public GameObject target;

    private float timer;
    public float dotInterval;
    public float spreadDist;

    public Color textColor;


    private List<GameObject> nearbyEnemies = new List<GameObject>();
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += 1 * Time.deltaTime;
        if(timer > dotInterval)
        {
            timer = 0;
            target.GetComponent<Entity>().damageTextColor = textColor;
            target.GetComponent<Entity>().TakeDamage(dotDamage, this);
            //foreach(GameObject a in )
        }
    }

    public void ApplyPlague(GameObject target)
    {
        float dist;
        foreach(GameObject a in gm.monstersAlive)
        {
            dist = Vector3.Distance(target.transform.position, a.transform.position);
            if(dist < spreadDist)
            {
                nearbyEnemies.Add(a);
            }
        }
    }
}

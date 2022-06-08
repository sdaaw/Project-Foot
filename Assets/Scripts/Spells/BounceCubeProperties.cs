using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCubeProperties : Spell
{
    private GameObject _player;

    public int bounceCount;

    public bool isInfinite;

    private int _currBounces;

    public List<GameObject> possibleTargets = new List<GameObject>();
    private List<GameObject> _hitMonsterList = new List<GameObject>();

    public float hitDistance;

    private float _closestDist;

    public GameObject target;

    public float distanceThreshold;

    // Start is called before the first frame update

    void Start()
    {
        _closestDist = 999999f;
        _player = gm.player;
        foreach (GameObject a in gm.monstersAlive)
        {
            float dist = Vector3.Distance(a.transform.position, transform.position);
            if(dist < _closestDist)
            {
                _closestDist = dist;
                target = a;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * movementSpeed);
        if(gm.monstersAlive.Count != 0 && !gm.isGameFrozen)
        {
            if (target != null)
            {
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                transform.LookAt(target.transform);
                float dist = Vector3.Distance(transform.position, target.transform.position);
                if (dist <= hitDistance)
                {
                    if (!isInfinite)
                    {
                        _currBounces++;
                        if (_currBounces >= bounceCount)
                        {
                            Destroy(gameObject);
                        }
                    }
                    //target.GetComponent<Entity>().TakeDamage(hitDamage, this);
                    /*closestDist = 999f;
                    foreach (GameObject a in gm.monstersAlive)
                    {
                        float dist2 = Vector3.Distance(a.transform.position, transform.position);
                        if (dist2 < closestDist && dist2 > distanceThreshold)
                        {
                            closestDist = dist2;
                            target = a;
                        }
                    }*/
                    target = gm.monstersAlive[Random.Range(0, gm.monstersAlive.Count)];
                }
            }
            else
            {
                target = gm.monstersAlive[Random.Range(0, gm.monstersAlive.Count)];
            }
        }
    }
}

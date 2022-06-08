using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{

    public float expiredTotalExperience;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

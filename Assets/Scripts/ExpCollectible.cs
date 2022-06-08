using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCollectible : MonoBehaviour
{

    public float collectDist;
    public float approachDist;

    public float expValue;

    public float approachSpeed;

    public GameObject player;
    public GameObject gameManager;

    public PlayerStats pStats;

    private Renderer _rend;

    private float _dist;

    public float expireTimer;
    public float expireDistance;

    private float _timer;
    // Start is called before the first frame update

    // Update is called once per frame

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
    }

    public void SetExpAmount(float amount)
    {
        expValue = amount;
        if (amount >= 500)
        {
            transform.localScale *= 2;
            _rend.material.color = new Color(4f, 0.5f, 4f);
        }
    }

    void FixedUpdate()
    {
        _timer += Time.deltaTime * 1f;
        _dist = Vector3.Distance(transform.position, player.transform.position);
        if (_dist < approachDist) //creep the objects toward the player
        {
            if (_dist < collectDist)
            {
                pStats.GainExperience(expValue);
                pStats.gm.expCollectibles.Remove(gameObject);
                Destroy(gameObject);
            }
            //(moveTowardsPlayerSpeed / dist) meaning obviously that the speed will increase as they approach the player, not that apparent currently
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, approachSpeed * _dist * Time.deltaTime);
        }

        if(_timer > expireTimer || _dist > expireDistance)
        {
            pStats.gm.collectibleManager.expiredTotalExperience += expValue;
            Destroy(gameObject);
        }
    }

}

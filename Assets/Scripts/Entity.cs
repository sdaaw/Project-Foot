using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject entityObject;
    public float movementSpeed;
    public GameObject followTarget;
    public float attackRange;
    public float attackDamage;

    public float maxHealth;
    public float currHealth;

    private GameManager _gm;
    private Color _ogColor;
    private bool _isVulnerable = true;

    public GameObject expObject;

    public GameObject damageText;

    public Color damageTextColor;

    public int expObjectDropCount;

    public AudioSource audioSource;
    public AudioClip hitSoundClip;

    public float attackCoolDown;
    private bool _isOnCD;

    private Renderer _rend;
    private Color _originalColor;

    public bool isDead;

    private Color _blinkColor;

    private PlayerStats _pStats;

    private float _dist;

    public bool isFrozen;

    private void Awake()
    {
        StartCoroutine(SpawnProtection());
        _gm = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        _blinkColor = new Color(2f, 2f, 2f);
        _rend = GetComponent<Renderer>();
        _originalColor = _rend.material.color;
        audioSource = FindObjectOfType<AudioSource>();
        followTarget = _gm.player;

        _pStats = followTarget.GetComponent<PlayerStats>();
        currHealth = maxHealth;

        StartCoroutine(PositionCheck());
    }

    IEnumerator SpawnProtection()
    {
        _isVulnerable = false;
        yield return new WaitForSeconds(0.1f);
        _isVulnerable = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (followTarget != null && _pStats.playerHealth > 0 && !_gm.isGameFrozen)
        {
            transform.LookAt(followTarget.transform);
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            if(!_isOnCD)
            {
                _dist = Vector3.Distance(transform.position, followTarget.transform.position);
                if (_dist < attackRange)
                {
                    StartCoroutine(attackCD());
                    _pStats.TakeDamage(attackDamage);
                }
            }
        }

    }

    IEnumerator PositionCheck()
    {
        if (_dist > 150f)
        {
            ResetPosition();
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(PositionCheck());
    }

    IEnumerator attackCD()
    {
        _isOnCD = true;
        yield return new WaitForSeconds(attackCoolDown);
        _isOnCD = false;
    }

    public void Die()
    {
        isDead = true;
        _gm.monstersAlive.Remove(gameObject);
        _pStats.MonstersKilled++;
        Destroy(gameObject);
        //StartCoroutine(RemoveGameObject());
    }

    IEnumerator RemoveGameObject()
    {
        Destroy(gameObject);
        yield return new WaitForSeconds(2f);
    }

    public void Knockback(float amount)
    {
        transform.Translate(-Vector3.forward * amount);
    }

    public void OnTriggerEnter(Collider other) 
    { 
        Spell spell = other.GetComponent<Spell>();
    
        //remember the tag in prefab
        if(other.gameObject.tag == "mobkiller")
        {
            float offsetDmg = 10f / 100f * spell.hitDamage;
            TakeDamage(Random.Range(spell.hitDamage - offsetDmg, spell.hitDamage + offsetDmg), spell);
            Knockback(spell.knockbackAmount);
        }
    }

    IEnumerator DamageTakeVisual()
    {
        if (_rend != null)
        {
            _rend.material.color = _blinkColor;
            yield return new WaitForSeconds(0.1f);
            _rend.material.color = _originalColor;
        }
    }

    public void TakeDamage(float dmg, Spell spell)
    {
        if (!_isVulnerable || _gm.isGameOver) return;

        StartCoroutine(DamageTakeVisual());
        dmg = dmg + _pStats.playerBaseDamage;

        spell.totalDamageDealt += dmg;
        GameObject a = Instantiate(damageText, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        //a.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z));
        a.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        a.GetComponent<TMP_Text>().text = dmg.ToString("F0");
        a.GetComponent<DamageText>().ownerObject = transform.gameObject;
        a.GetComponent<TMP_Text>().colorGradient = new VertexGradient(damageTextColor);
        currHealth -= dmg;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(hitSoundClip);
        if(currHealth <= 0)
        {
            for(int i = 0; i < expObjectDropCount; i++)
            {
                ExpCollectible e;
                GameObject b = Instantiate(expObject, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
                e = b.GetComponent<ExpCollectible>();
                b.transform.rotation = Random.rotation;
                //b.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(10f, 20f));
                e.pStats = _pStats;
                e.player = followTarget;
                e.approachDist = _pStats.expApproachDist;

                /*if (Random.Range(0, 10000) > 9995)
                {
                    e.SetExpAmount(1000);
                }*/
                _gm.expCollectibles.Add(b);
            }
            Die();
        }
    }

    public void ResetPosition()
    {
        float minDistance = 20f;
        float maxDistance = 40f;
        float distance = Random.Range(minDistance, maxDistance);
        float angle = Random.Range(-Mathf.PI, Mathf.PI);

        Vector3 newPos = _gm.player.transform.position;
        newPos += new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * distance;
        newPos.x = Mathf.Clamp(newPos.x, _gm.player.transform.position.x - _gm.gameObject.GetComponent<SwarmDirector>().boxBound, _gm.player.transform.position.x + _gm.gameObject.GetComponent<SwarmDirector>().boxBound);
        newPos.y = 1;
        newPos.z = Mathf.Clamp(newPos.z, _gm.player.transform.position.z - _gm.gameObject.GetComponent<SwarmDirector>().boxBound, _gm.player.transform.position.z + _gm.gameObject.GetComponent<SwarmDirector>().boxBound);
        transform.position = newPos;
    }
}

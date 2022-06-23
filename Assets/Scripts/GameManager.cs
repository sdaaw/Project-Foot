using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;
    public GameObject spawnPoint;

    public GameObject player;
    public PlayerStats pStats;
    public CameraFollow cFollow;

    public GameObject effectHolder;
    public OrbitingCube orbitingCube;
    public LazerCube lazerCube;

    public TMP_Text expText;
    public TMP_Text levelText;
    public TMP_Text healthText;
    public TMP_Text DPSText;
    public TMP_Text monstersKilledText;

    public GameObject GameOverPanel;
    public CollectibleManager collectibleManager;

    public List<GameObject> monstersAlive = new List<GameObject>();
    public List<GameObject> expCollectibles = new List<GameObject>();

    public List<GameObject> activeBounceCubes = new List<GameObject>();


    public List<Spell> activeSpells = new List<Spell>();

    public GameObject bounceCube;

    public GameObject spellBannerObject;
    public GameObject canvasObject;

    private int _oldPlayerLevel;

    public bool isGameOver;

    public bool isGameFrozen;
    // Start is called before the first frame update
    void Awake()
    {
        orbitingCube = effectHolder.GetComponent<OrbitingCube>();
        lazerCube = effectHolder.GetComponent<LazerCube>();
        isGameFrozen = false;
        SpawnPlayer();
        StartCoroutine(DPSDisplay());
        Physics.IgnoreLayerCollision(6, 6, true);
    }

    public void SpawnPlayer()
    {
        player = Instantiate(_playerPrefab, spawnPoint.transform.position, Quaternion.identity);

        player.GetComponent<PlayerStats>().gm = this;
        collectibleManager = GetComponent<CollectibleManager>();

        pStats = player.GetComponent<PlayerStats>();
        cFollow = FindObjectOfType<CameraFollow>();
        cFollow.m_follow = player.transform;
        _oldPlayerLevel = pStats.playerLevel;
        UpdateHealthText(pStats.playerHealth.ToString("F0"));
        UpdateExpText(pStats.playerCurrentExp.ToString("F0"));
        UpdateLevelText(pStats.playerLevel.ToString());
        orbitingCube.player = player;
        StartCoroutine(SpawnEffectWithDelay());
    }


    IEnumerator SpawnEffectWithDelay()
    {
        yield return new WaitForSeconds(1f);
        SpawnBounceCube();
    }

    IEnumerator DPSDisplay()
    {
        //TODO: Make it more dynamic, display currently equipped spells only with automatic text formatting and implement spell name strings.
        if (activeSpells.Count != 0)
        {
            float bCubeDPS = GetSpellDPS(activeBounceCubes);
            float oCubeDPS = GetSpellDPS(orbitingCube.orbitingObjects);
            float lCubeDPS = GetSpellDPS(lazerCube.lazerCubes);
            DPSText.text = "Bouncing Cube       " + bCubeDPS.ToString("F1") + "\nOrbiting Cube        " + oCubeDPS.ToString("F1") + "\nLazer Cube           " + lCubeDPS.ToString("F1");
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(DPSDisplay());
    }

    public float GetSpellDPS(List<GameObject> spell)
    {
        float dps = 0;
        foreach (GameObject s in spell)
        {
            dps += s.GetComponent<Spell>().dps;
        }
        return dps;
    }
    public void GameOverScreen()
    {
        isGameOver = true;
        GameOverPanel.SetActive(true);
        monstersKilledText.text = pStats.MonstersKilled.ToString();
    }

    public void FreezeGame()
    {
        ShowSpellBanners();
        isGameFrozen = !isGameFrozen;
    }

    public List<Spell> GetRandomSpells(int count)
    {
        List<Spell> spells = new List<Spell>();
        return spells;
    }

    public void ShowSpellBanners()
    {
        int spellCount = 3;
        float screenWidthScale = Screen.width / spellCount;
        float xBuffer = 0;
        for(int i = 1; i < spellCount + 1; i++)
        {
            xBuffer = (screenWidthScale * i) - (Screen.width / (spellCount * 2));
            GameObject a = Instantiate(spellBannerObject);
            a.transform.SetParent(canvasObject.transform, false);
            a.GetComponent<RectTransform>().anchoredPosition = new Vector3(xBuffer, 0, 0);
        }
    }

    public void CleanUp()
    {

        orbitingCube.ClearOrbitCubes();
        //TODO: Run automatic clean up with the current active spells, no need to run through empty lists(?) for nothing(?), idk how C# handles empty lists.

        //is cleaning activeBounceCubes list necessary if we clear activeSpells already?
        foreach (GameObject s in activeBounceCubes)
        {
            Destroy(s);
        }
        foreach (GameObject a in monstersAlive)
        {
            Destroy(a);
        }
        foreach (GameObject a in expCollectibles)
        {
            Destroy(a);
        }
        foreach (Spell a in activeSpells)
        {
            Destroy(a);
        }
        activeBounceCubes = new List<GameObject>();
        activeSpells = new List<Spell>();
        monstersAlive = new List<GameObject>();
        expCollectibles = new List<GameObject>();
        Destroy(player);
    }

    public void Retry()
    {
        CleanUp();
        SpawnPlayer();
        GameOverPanel.SetActive(false);
        isGameOver = false;
    }
    public void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            foreach(GameObject a in expCollectibles)
            {
                a.GetComponent<ExpCollectible>().approachDist = 99f;
                Physics.IgnoreLayerCollision(6, 6, true);
            }
        } else
        {
            foreach (GameObject a in expCollectibles)
            {
                a.GetComponent<ExpCollectible>().approachDist = 2f;
                Physics.IgnoreLayerCollision(6, 6, false);
            }
        }


        if(Input.GetKeyDown(KeyCode.T))
        {

            //pStats.GainExperience(500000);
            //SpawnBounceCube();
            orbitingCube.AddOrbitingObject();
            //lazerCube.isActive = true;

        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            foreach(Spell s in activeSpells)
            {
                s.movementSpeed += 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pStats.LevelUp();
            //isGameFrozen = false;
        }
    }

    public void SpawnOrbitCube()
    {
        if (!orbitingCube.isActive)
        {
            orbitingCube.isActive = true;
        }
        orbitingCube.AddOrbitingObject();
    }

    IEnumerator ExpGainVisual()
    {

        yield return new WaitForSeconds(0.01f);
    }

    public void SpawnBounceCube()
    {
        GameObject a = Instantiate(bounceCube, player.transform.position, Quaternion.identity);
        a.GetComponent<BounceCubeProperties>().isInfinite = true;
        activeBounceCubes.Add(a);
        activeSpells.Add(a.GetComponent<Spell>());
    }

    public void UpdateExpText(string text)
    {
        expText.text = text;
    }

    public void UpdateLevelText(string text)
    {
        levelText.text = text;
    }

    public void UpdateHealthText(string text)
    {
        healthText.text = text;
    }

}

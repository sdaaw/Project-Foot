using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SpellBanner : MonoBehaviour
{
    // Start is called before the first frame update

    public ParticleSystem particle;

    public TMP_Text spellText;
    public Button button;
    private ParticleSystem.MainModule settings;

    private bool isInTransition;
    public float transitionSpeed;
    private float alphaVal;
    private Image image;

    void Start()
    {
        if(GetComponent<Image>() != null)
        {
            image = GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        } else
        {
            Debug.LogWarning("IMAGE IS NULL");
        }
        spellText.color = new Color(spellText.color.r, spellText.color.g, spellText.color.b, 0f);
        settings = particle.GetComponent<ParticleSystem>().main;
        settings.startColor = new Color(Random.Range(0.5f, 1f), Random.Range(0f, 1f), Random.Range(0.5f, 1f));
        particle.Play();
        OnSpawn();
    }

    public void OnSpawn()
    {
        alphaVal = 0;
        isInTransition = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isInTransition)
        {
            if(alphaVal >= 1f)
            {
                isInTransition = false;
            }
            alphaVal += transitionSpeed;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alphaVal);
            spellText.color = new Color(spellText.color.r, spellText.color.g, spellText.color.b, alphaVal);
        }
    }

    public void OnClick()
    {
        print("click");
    }

    public void OnHover()
    {
        print("enter");
        settings.simulationSpeed *= 2f;
        //settings.startSizeMultiplier *= 2f;
        settings.startColor = new Color(Random.Range(0.5f, 1f), Random.Range(0f, 1f), Random.Range(0.5f, 1f));
    }
    public void OnHoverExit()
    {
        print("exit");
        settings.simulationSpeed /= 2f;
        //settings.startSizeMultiplier /= 2f;
    }
}

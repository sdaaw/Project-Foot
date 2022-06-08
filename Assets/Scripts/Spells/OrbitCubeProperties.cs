using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCubeProperties : Spell
{
    public Material objectMaterial;

    [Range(0f, 1f)]
    public float glowStrength;

    public Color baseColor;

    private Renderer rend;


    public float currOrbitAngle;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        Color color = new Color(glowStrength, glowStrength, glowStrength);
        color = new Color(glowStrength + baseColor.r, glowStrength + baseColor.g, glowStrength + baseColor.b);
        if(rend != null)
        {
            rend.material.color = color;
        } else
        {
            objectMaterial.color = color;
        }
    }
}

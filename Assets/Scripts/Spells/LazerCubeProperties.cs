using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCubeProperties : Spell
{

    private Renderer _renderer;

    private float _rotationSpeed;

    public float colorVariation;

    private float _timer;
    public float _alphaVal;

    public float lifeTime;

    private Color _aColor;

    private Material _material;
    private Vector3 _randDir;
    // Start is called before the first frame update
    void Start()
    {

        if(GetComponent<Renderer>().material != null)
        {
            _material = new Material(GetComponent<Renderer>().material);
            GetComponent<Renderer>().material = _material;
        }
        _randDir = Random.insideUnitCircle.normalized;
        _alphaVal = 0f;
        _rotationSpeed = Random.Range(50f, 100f);
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = new Color(_renderer.material.color.r + Random.Range(-colorVariation, colorVariation),
                                             _renderer.material.color.g + Random.Range(-colorVariation, colorVariation),
                                             _renderer.material.color.b + Random.Range(-colorVariation, colorVariation), _alphaVal);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _timer += 1f * Time.deltaTime;
        if(_timer > lifeTime)
        {
            _alphaVal -= 0.05f;
            if(_alphaVal <= 0)
            {
                gm.lazerCube.lazerCubes.Remove(gameObject);
                Destroy(gameObject);
            }

            _aColor = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, _alphaVal);
            _renderer.material.color = _aColor;
            print(_aColor);
        } else
        {
            _alphaVal += 0.05f;
            _aColor = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, _alphaVal);
            _renderer.material.color = _aColor;
        }
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotationSpeed);
    }
}

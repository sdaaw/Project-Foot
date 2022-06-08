using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionManager : MonoBehaviour
{

    private GameManager _gm;

    public Slider slider;

    public ParticleSystem particle;
    public GameObject sliderImageObject;
    private Image _sliderImage;

    private float _targetProgress = 0;

    public float fillSpeed = 0.5f;

    private Color _originalColor;

    void Awake()
    {
        _gm = GetComponent<GameManager>();
        slider.value = 0;
        _sliderImage = sliderImageObject.GetComponent<Image>();
        _originalColor = _sliderImage.color;
    }

    public Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    void FixedUpdate()
    {
        if (_gm.isGameFrozen)
        {
            _sliderImage.color = Color.Lerp(_originalColor, Color.magenta, Mathf.PingPong(Time.time, 1f));
        } else
        {
            _sliderImage.color = _originalColor;
        }
        if(slider.value < _targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
            if(slider.value >= _targetProgress)
            {
                particle.Stop();
            }
            if(slider.value >= 1)
            {
                _gm.pStats.LevelUp();
                slider.value = 0;
                _targetProgress = 1f - _targetProgress;
            }
        }
    }

    ///<summary>
    ///Value has to be between 0f to 1f :P
    ///</summary>
    public void UpdateProgress(float progVal)
    {
        if(slider.value < _targetProgress)
        {
            slider.value = _targetProgress;
        }
        particle.Play();
        _targetProgress = slider.value + progVal;
    }
}

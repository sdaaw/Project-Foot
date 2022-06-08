using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SlotObject : MonoBehaviour
{
    public enum SlotObjectType
    {
        Common,
        Uncommon,
        Rare,
        UltraRare,
        Legendary
    }

    [Serializable]
    public struct ObjectData
    {
        public SlotObjectType type;
        public float odds;
    }
    public ObjectData objectData;

    private Vector3 _objectScale;
    private Vector3 _destObjectScale;
    private bool _doAnim;
    private float _timer;

    public void Start()
    {
        _objectScale = transform.localScale;
        _destObjectScale *= 2;
    }

    public void DoVisual()
    {
        //transform.localScale *= 2;
        StartCoroutine(ExpandObject());
    }

    public void FixedUpdate()
    {
        if(_doAnim)
        {
            _timer += 1 * Time.deltaTime;
            float sinval = Mathf.Sin(_timer / 0.5f) / 30f;
            float cosval = Mathf.Cos(_timer / 0.5f) / 30f;
            transform.localScale = new Vector3(transform.localScale.x + sinval, transform.localScale.y + cosval, transform.localScale.z + sinval);
        }
    }

    IEnumerator ExpandObject()
    {
        _doAnim = true;
        yield return null;
    }

}

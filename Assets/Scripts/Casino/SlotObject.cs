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
    }

    [HideInInspector]
    public ObjectData objectData;
    private bool _doAnim;
    private float _timer;

    public void Start()
    {
    }

    public void DoVisual()
    {
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

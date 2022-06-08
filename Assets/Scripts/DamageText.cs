using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{

    public float lifeTime;
    private float timer;
    public Vector3 shrinkSpeed;
    public Vector3 driftSpeed;
    public GameObject ownerObject;

    public GameObject canvas;

    private Vector3 randomPosOffset;

    public Vector3 textScale;

    private Entity ownerEntity;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.SetParent(FindObjectOfType<Canvas>().transform);

        randomPosOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        ownerEntity = ownerObject.GetComponent<Entity>();
        StartCoroutine(TextSpawnAnimation());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += 1 * Time.deltaTime;
        if(timer > lifeTime)
        {
            transform.localScale -= shrinkSpeed;
            if(transform.localScale.x <= 0 || transform.localScale.y <= 0)
            {
                Destroy(gameObject);
            }
        }
        if(ownerEntity.isDead)
        {
            timer = lifeTime;
        }
        if(ownerObject != null)
        {
            RectTransform ownerPos = ownerObject.GetComponent<Entity>().damageText.GetComponent<RectTransform>();
            //transform.position = Camera.main.WorldToScreenPoint(new Vector3(ownerObject.transform.position.x, ownerObject.transform.position.y, ownerObject.transform.position.z) + randomPosOffset);
            GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(new Vector3(ownerPos.transform.position.x, ownerObject.transform.position.y, ownerObject.transform.position.z));
        }
    }
    

    IEnumerator TextSpawnAnimation()
    {
        for(float i = 0; i < 1; i += 0.1f)
        {
            transform.localScale = textScale * i;
            yield return new WaitForSeconds(0.01f);
        }
    }
}

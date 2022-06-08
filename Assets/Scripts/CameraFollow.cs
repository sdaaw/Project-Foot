using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{

    public static CameraFollow s_mainCamera;

    public Queue<IEnumerator> cameraMoves = new Queue<IEnumerator>();

    public static void MoveTo(Vector3 position, float duration)
    {
        if (!s_mainCamera.dontFollow)
        {
            s_mainCamera.StartCoroutine(s_mainCamera.MoveCamera(position, duration));
        }
        else
        {
            s_mainCamera.cameraMoves.Enqueue(s_mainCamera.MoveCamera(position, duration));
        }
    }

    public bool dontFollow;


    public Transform m_follow;

    [SerializeField]
    private float m_distance;

    [SerializeField]
    private float m_offsetX;

    [SerializeField]
    private float m_offsetZ;

    [SerializeField]
    private float m_smoothTime;

    private Transform m_transform;
    private Vector3 m_followPos;
    private Vector3 m_velocity;

    private void Awake()
    {
        s_mainCamera = this;
        m_transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (!dontFollow && m_follow != null)
        {
            FollowTarget();
        }
    }

    /*void OnGUI()
	{
		if(GUI.Button(new Rect(0,0,55,55),"Move"))
		{
			MoveTo(new Vector3(-22f,5.5f,27f),4);
			MoveTo(new Vector3(22f,5.5f,27f),4);
		}
	}*/

    private void FollowTarget()
    {
        m_followPos.x = m_follow.position.x + m_offsetX;
        m_followPos.z = m_follow.position.z + m_offsetZ;

        //m_distance = Mathf.Clamp(m_distance - Input.GetAxis("Mouse ScrollWheel") * 5, m_distanceMin, m_distanceMax);
        m_followPos.y = m_distance;

        m_transform.position = Vector3.SmoothDamp(m_transform.position, m_followPos, ref m_velocity, m_smoothTime);
    }

    private IEnumerator MoveCamera(Vector3 moveToPos, float duration)
    {
        dontFollow = true;
        //GameResources.StopMovingEntities = true;
        Vector3 startPos = m_transform.position;
        float lerpTime = 0;

        while (lerpTime < 1)
        {
            lerpTime += Time.deltaTime * 5;
            m_transform.position = Vector3.Lerp(startPos, moveToPos, lerpTime);
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        dontFollow = false;
        //GameResources.StopMovingEntities = false;

        if (cameraMoves.Count > 0)
        {
            s_mainCamera.StartCoroutine(cameraMoves.Dequeue());
        }
    }
}
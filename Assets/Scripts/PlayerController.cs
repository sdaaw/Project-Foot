using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public bool AllowMovement;
    public float speed;

    public float score;
    public string heroName;
    private Transform m_transform;

    private Quaternion m_oldRotation;
    private float m_horAxis;
    private float m_verAxis;
    private Vector3 m_move;
    private Vector3 m_mousePos;

    [SerializeField]
    private Camera m_playerCamera;

    [SerializeField]
    private LayerMask m_layerMask;

    public Animator animator;

    [SerializeField]
    private LayerMask m_dashMask;

    public AudioSource bgMusic;

    private GameManager gm;

    private bool isDead = false;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        m_transform = transform;
        m_playerCamera = FindObjectOfType<Camera>();
    }

    void FixedUpdate()
    {
        if (AllowMovement && !gm.isGameFrozen)
        {
            DoMovement();
            Rotate(m_move);
            //Rotate((MouseDir() - m_transform.position).normalized);
        }
    }

    private void Update()
    {
        if (isDead)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //SceneManager.LoadScene("MikkoHideOut");
            }


            return;
        }


        if (m_transform.position.y < -20)
            //GetComponent<Stats>().TakeDmg(10000);

        if (gm.isGameFrozen)
            return;

        /*if(Input.GetKeyDown(KeyCode.E))
        {
            if(m_allowDash && canDash)
            {
                StartCoroutine(Dash());
                dashTimerForUI = 0;
                m_anim.SetTrigger("Charge");
            }     
        }*/
    }

    void AttackPrimary()
    {
        //e.Explosion(new Vector3(MouseDir().x, 2, MouseDir().z), a);
    }

    void DoMovement()
    {
        m_oldRotation = m_playerCamera.transform.rotation;

        Vector3 temp = m_oldRotation.eulerAngles;
        temp.x = 0;
        m_playerCamera.transform.rotation = Quaternion.Euler(temp);

        m_horAxis = Input.GetAxis("Horizontal");
        m_verAxis = Input.GetAxis("Vertical");

        m_move.x = m_horAxis;
        m_move.y = 0;
        m_move.z = m_verAxis;

        m_move = m_playerCamera.transform.TransformDirection(m_move);

        m_playerCamera.transform.rotation = m_oldRotation;

        m_move.y = 0;

        Move(m_move);
    }


    protected void Move(Vector3 moveVector)
    {
        if (AllowMovement)
        {
            //m_transform.Translate(moveVector * Time.fixedDeltaTime * speed, Space.World);
            m_transform.position += moveVector * speed;
        }
    }

    protected void Rotate(Vector3 rotateVector)
    {
        if (AllowMovement && rotateVector != Vector3.zero)
        {
            m_transform.rotation = Quaternion.LookRotation(new Vector3(rotateVector.x, 0, rotateVector.z));
        }
    }

    private Vector3 MouseDir()
    {
        m_mousePos.x = Input.mousePosition.x;
        m_mousePos.y = Input.mousePosition.y;
        m_mousePos.z = Camera.main.WorldToScreenPoint(m_transform.position).z;

        return Camera.main.ScreenToWorldPoint(m_mousePos);
    }



    public void Dead()
    {
        AllowMovement = false;
        isDead = true;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameManager gm;

    private float currSpeed = 0.0f;
    public float maxRunSpeed = 7.0f;
    public float runAccel = 0.2f;
    public float maxWalkBackwardsSpeed = 3.0f;
    public float drag = 0.05f;
    public float turnSpeed = 300f;
    public float minMove = 0.1f;
    private Rigidbody rb;
    private Animator animator;
    public float animationSpeed = 1.5f;

    private bool isRunning;
    private bool isIdle;
    private bool isWalkingBackwards;

    private Vector3 movementInput;
    private Vector3 turnInput;
    private float vertInput;
    private float horzInput;
    
    private float curveHeight = 0.5f;
    private CapsuleCollider col;
    private float colliderHeight;
    private Vector3 colliderCenter;

    [HideInInspector] public bool paused = false;

    private float timestamp = 0f;
    [SerializeField] private float slideCooldown = 2.0f;
    private bool isSliding = false;

    private AnimatorStateInfo animInfo;

    private bool healthCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;
        col = GetComponent<CapsuleCollider>();
        colliderHeight = col.height;
        colliderCenter = col.center;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Take input
        vertInput = Input.GetAxis("Vertical");
        horzInput = Input.GetAxis("Horizontal");
        animator.SetFloat("CurrSpeed", currSpeed);
        animator.SetFloat("InputSpeed", vertInput);
        animator.SetFloat("Direction", horzInput);
        movementInput = transform.forward * vertInput;
        turnInput = transform.up * horzInput * turnSpeed;

        animInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.Space) && animInfo.IsName("Movement") && Time.time > timestamp + slideCooldown && currSpeed == maxRunSpeed)
        {
            Slide();
        }
        isSliding = animInfo.IsName("Slide");
    }

    // On collision with enemy, attack enemy if sliding or take damage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (isSliding)
            {
                collision.gameObject.GetComponent<Zombie>().DecreaseHealth(1);
            }
            else if (!healthCooldown)
            {
                gm.DecreaseHealth(1);
                healthCooldown = true;
                Invoke("ResetInvul", 2);
            }
        }
    }

    // Method to invoke after 3 seconds of invulnerability when hit
    private void ResetInvul()
    {
        healthCooldown = false;
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            Move();
            transform.Rotate(turnInput * Time.deltaTime);
            Animate();
        }
    }

    private void Move()
    {
        if (vertInput > minMove)
        {
            isRunning = true;
            isWalkingBackwards = false;
            isIdle = false;
            ApplyAccel();
        }
        else if (vertInput < -minMove)
        {
            isWalkingBackwards = true;
            isRunning = false;
            isIdle = false;
            ApplyAccel();
        }
        else
        {
            isRunning = false;
            isWalkingBackwards = false;
            isIdle = true;
            ApplyDrag();
        }
        transform.localPosition += (movementInput * Time.fixedDeltaTime * currSpeed);
    }

    private void Slide()
    {
        animator.SetTrigger("IsSliding");
        timestamp = Time.time;
    }

    private void ApplyAccel()
    {
        if (isRunning)
        {
            if (currSpeed < maxRunSpeed)
            {
                currSpeed += runAccel;
            }
            if (currSpeed > maxRunSpeed)
            {
                currSpeed = maxRunSpeed;
            }
        }
        else if (isWalkingBackwards)
        {
            currSpeed = maxWalkBackwardsSpeed;
        }
    }

    private void ApplyDrag()
    {
        if (currSpeed > 0f)
        {
            currSpeed -= drag;
        }
        if (currSpeed < 0f)
        {
            currSpeed = 0f;
        }
    }

    private void Animate()
    {
        animator.SetBool("IsIdle", isIdle);
        animator.SetBool("IsRunning", isRunning);
        animator.SetBool("IsWalkingBackwards", isWalkingBackwards);
    }
}


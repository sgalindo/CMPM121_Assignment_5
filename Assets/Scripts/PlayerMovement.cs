using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float currSpeed = 0.0f;
    public float maxRunSpeed = 7.0f;
    public float runAccel = 0.2f;
    public float maxWalkBackwardsSpeed = 3.0f;
    public float drag = 0.05f;
    public float turnSpeed = 300f;
    public float minMove = 0.1f;
    public float jumpForce = 3.0f;
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

    private float jumpHeight;
    private float curveHeight = 0.5f;
    private CapsuleCollider col;
    private float colliderHeight;
    private Vector3 colliderCenter;
    public float jumpCooldown = 1.0f;
    private float timestamp;

    [HideInInspector] public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.speed = animationSpeed;
        col = GetComponent<CapsuleCollider>();
        colliderHeight = col.height;
        colliderCenter = col.center;
        timestamp = 0f;
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
        turnInput = transform.up * horzInput * Time.deltaTime * turnSpeed;

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (!animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Movement") && (Time.time - timestamp) > jumpCooldown)
        //    {
        //        Jump();
        //    }
        //}
        //
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        //{
        //    if (!animator.IsInTransition(0))
        //    {
        //        CheckJumpHeight();
        //        animator.SetBool("IsJumping", false);
        //    }
        //}
    }

    private void FixedUpdate()
    {
        if (!paused)
        {
            Move();
            transform.Rotate(turnInput);
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

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        isRunning = false;
        animator.SetBool("IsJumping", true);
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

    private void CheckJumpHeight()
    {
        jumpHeight = animator.GetFloat("JumpHeight");
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance > curveHeight)
            {
                col.height = colliderHeight - jumpHeight;
                float colCenterY = colliderCenter.y + jumpHeight;
                col.center = new Vector3(0, colCenterY, 0); 
            }
            else
            {
                col.height = colliderHeight;
                col.center = colliderCenter;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    private Rigidbody2D rb;
    public SpriteRenderer sr;
    GhostController gc;
    private float CurrentDashTimer;

    [SerializeField] private float dirX = 0;
    [SerializeField] private float moveSpeed = 12.0f;
    [SerializeField] private float jumpForce = 25.0f;
    [SerializeField] private bool isGrounded = true;


    [SerializeField]
    private float dashForce = 30f;

    [SerializeField]
    private float StartDashTimer;


    [SerializeField]
    private bool isDashing = false;

    [SerializeField]
    private float dashDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gc = GetComponent<GhostController>();
        gc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        parseInput();
        checkIfGrounded();
        UpdateAnimation();
    }

    private void parseInput()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
        if (Input.GetKeyDown(KeyCode.F) && rb.velocity.y > 0f && dirX != 0)
        {
            isDashing = true;
            CurrentDashTimer = StartDashTimer;
            rb.velocity = Vector2.zero;
            dashDirection = dirX;
        }
        dashEffect();
    }

    private void dashEffect()
    {
        if (isDashing)
        {
            gc.enabled = true;
            rb.velocity = transform.right * dashDirection * dashForce;
            CurrentDashTimer -= Time.deltaTime;

            if (CurrentDashTimer <= 0)
            {
                isDashing = false;
                gc.enabled = false;
            }
        }
    }
    private void checkIfGrounded()
    {
        if (rb.velocity.y != 0)
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
        else
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
    }

    void UpdateAnimation()
    {
        Debug.Log("<color=orange>" + $"{isGrounded}" + "</color>");
        if (isGrounded)
        {   
            // NORMAL ATTACK
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetTrigger("attack1");
            }

            // RUNNING / IDLE STATES
            if (dirX > 0f)
            {
                sr.flipX = false;
                animator.SetBool("isRunning", true);
            }
            else if (dirX < 0f)
            {
                sr.flipX = true;
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            // JUMP ATTACK
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("<color=red>E pressed</color>");
                animator.SetTrigger("attack2");
            }

            // BEING ABLE TO CHANGE DIRECTION IN THE AIR
            if (dirX > 0f)
            {
                sr.flipX = false;
            }
            else if (dirX < 0f)
            {
                sr.flipX = true;
            }

            if (rb.velocity.y > 0f)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
            else if (rb.velocity.y < 0f)
            {
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
        }
    }
}

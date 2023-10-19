using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    private SpriteRenderer sr;

    [SerializeField] private float dirX = 0;
    [SerializeField] private float moveSpeed = 12.0f;
    [SerializeField] private float jumpForce = 25.0f;
    [SerializeField] private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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
    }

    private void checkIfGrounded()
    {
        if (rb.velocity.y == 0f)
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void UpdateAnimation()
    {
        if (isGrounded)
        {
            Debug.Log("<color=orange>" + $"{isGrounded}" + "</color>");
            
            // NORMAL ATTACK
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.SetBool("isAttack1", true);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log("<color=red>E not pressed</color>");
                animator.SetBool("isAttack1", false);
                animator.SetBool("isAttack2", false);
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
                animator.SetBool("isAttack2", true);
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

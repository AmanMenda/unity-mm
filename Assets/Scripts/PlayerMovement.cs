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
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
        UpdateAnimation();
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
            if (dirX > 0f)
            {
                Debug.Log("Righttttt");
                sr.flipX = false;
                animator.SetBool("isRunning", true);
            }
            else if (dirX < 0f)
            {
                Debug.Log("Lefttttt");
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
            if (rb.velocity.y > 0f)
            {
                Debug.Log("Jump !!!");
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
            else if (rb.velocity.y < 0f)
            {
                Debug.Log("Falling !!!");
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }
        }
    }
}

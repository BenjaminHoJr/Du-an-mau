using System.Collections;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    public float moveSpeed = 2f; // Speed of the player movement
    public float jumpForce = 3f; // Force applied when jumping
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput < -0.01f) 
        {
            sprite.flipX = true; 
            animator.SetBool("isRunning", true); 
            animator.SetBool("isIdle", false); 
        }
        else if(moveInput > 0.01f) 
        {
            sprite.flipX = false; 
            animator.SetBool("isRunning", true); 
            animator.SetBool("isIdle", false); 
        }
        else // If not moving
        {
            animator.SetBool("isRunning", false); 
            animator.SetBool("isIdle", true); 
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("isRolling", true); 
            rb.linearVelocity = new Vector2(5, rb.linearVelocity.y); 
        }
    }
    IEnumerator Roll()
    {
        animator.SetBool("isRolling", true);
        rb.linearVelocity = new Vector2(5, rb.linearVelocity.y);
        yield return new WaitForSeconds(0.5f); 
        animator.SetBool("isRolling", false);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
}

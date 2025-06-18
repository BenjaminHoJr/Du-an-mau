using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float jumpForce = 7f;
    public float rollSpeed = 5f;
    public float climbSpeed = 3f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    private bool isGrounded;
    private bool isClimbing = false;
    private bool isRolling = false;

    private float originalGravity;
    private float rollDuration = 0.4f;
    private float rollTimer;

    private int jumpCount = 0;
    private int maxJumps = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        originalGravity = rb.gravityScale;
    }

    void Update()
    {
        CheckGround();
        HandleMovement();
        HandleJump();
        HandleRoll();
        HandleClimbing();
        UpdateAnimations();
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded && !isClimbing)
        {
            jumpCount = 0; // reset khi chạm đất
        }
    }

    void HandleMovement()
    {
        if (isRolling || isClimbing) return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(moveInput) > 0.01f)
            sprite.flipX = moveInput < 0;
    }

    void HandleJump()
    {
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps && !isClimbing && !isRolling)
            {
                jumpCount++;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
    }

    void HandleRoll()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !isRolling && !isClimbing)
        {
            isRolling = true;
            rollTimer = rollDuration;

            float direction = sprite.flipX ? -1 : 1;
            rb.linearVelocity = new Vector2(direction * rollSpeed, rb.linearVelocity.y);
            animator.SetTrigger("Roll");
        }

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                isRolling = false;
            }
        }
    }

    void HandleClimbing()
    {
        if (!isClimbing) return;

        float vertical = Input.GetAxisRaw("Vertical");
        rb.linearVelocity = new Vector2(0f, vertical * climbSpeed);

        animator.SetBool("isClimbing", Mathf.Abs(vertical) > 0.01f);
    }

    void UpdateAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isClimbing", isClimbing);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = originalGravity;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            animator.SetBool("isClimbing", false); // đảm bảo thoát trạng thái climbing
        }
    }
}

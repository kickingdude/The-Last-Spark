using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // initial movement variables; speed adjusts player speed, then jumping power adjusts height
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr; // trail for dashing gets referenced in the object

    //serializing values to alternate between the dash stats
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCD;
    
    //serializing values for jump count
    private bool isJumping;
    private int remainingJumps;
    [SerializeField] private int maxJumps;

    //wall sliding and jumping vars
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    //wall jumping vars
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        //check to see if Dashing and Jumping is true (prevents someone from spamming)
        if (isDashing)
        {
            return;
        }

        //if we are on the ground, reset the jump count
        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            isJumping = false;
            remainingJumps = maxJumps;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || (isJumping && remainingJumps > 0))
            {
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                remainingJumps--;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown("c") && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();

        if (!isWallJumping){
            Flip();
        }
    }

    private void FixedUpdate()
    {
        //ensures no actions can be done while jumping
        if (isDashing)
        {
            return;
        }

        if (!isWallJumping)

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    //checks to see if we're on the ground
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //checks to see if we're on the wall
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);

    }

    //flips the sprite in the other direction (you can't see it bc its a square lol)
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

    }


    //Movement methods! Rn I just have the dash, wall slide, and the wall jump
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGrav = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        tr.emitting = false;
        rb.gravityScale = originalGrav;
        isDashing = false;
        yield return new WaitForSeconds(dashCD);
        canDash = true;
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
using System;
using AGDDPlatformer;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : KinematicObject
{

    [Header("Movement")]
    public float maxSpeed = 7;
    public float jumpSpeed = 7;
    public float jumpDeceleration = 0.5f; // Upwards slow after releasing jump button
    public float coyoteTime = 0.1f; // Lets player jump just after leaving ground
    public float jumpBufferTime = 0.1f; // Lets the player input a jump just before becoming grounded

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
    public Color canDashColor;
    public Color cantDashColor;
    private float lastDashTime;
    private Vector2 dashDirection;
    private Vector2 mouseDashDirection;
    private bool isDashing;
    private bool canDash;
    private bool wantsToDash;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip jumpSound;
    public AudioClip dashSound;

    private Vector2 startPosition;

    private float lastJumpTime;
    private float lastGroundedTime;
    private bool canJump;
    private bool jumpReleased;
    private Vector2 move;
    private float defaultGravityModifier;
    private bool _isDead = false;
    private Animator _animator;
    private GameObject _lightAnchor;
    
    private SpriteRenderer spriteRenderer;
    public Light spotLight;
    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        lastJumpTime = -jumpBufferTime * 2;

        startPosition = transform.position;

        _animator = gameObject.GetComponentInChildren<Animator>();
        _lightAnchor = GameObject.Find("LightAnchor");

        defaultGravityModifier = gravityModifier;
    }

    void Update()
    {
        isFrozen = GameManager.instance.timeStopped;
        //Do no actions if player is dead
        if (_isDead)
        {
            return;
        }

        /* --- Read Input --- */

        move.x = Input.GetAxisRaw("Horizontal");
        if (gravityModifier < 0)
        {
            move.x *= -1;
        }

        move.y = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            // Store jump time so that we can buffer the input
            lastJumpTime = Time.time;
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumpReleased = true;
        }

        // Clamp directional input to 8 directions for dash
        Vector2 desiredDashDirection = new Vector2(
            move.x == 0 ? 0 : (move.x > 0 ? 1 : -1),
            move.y == 0 ? 0 : (move.y > 0 ? 1 : -1));
        if (desiredDashDirection == Vector2.zero)
        {
            // Dash in facing direction if there is no directional input;
            desiredDashDirection = spriteRenderer.flipX ? -Vector2.right : Vector2.right;
        }
        desiredDashDirection = desiredDashDirection.normalized;
        //var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        //mouseDashDirection = new Vector2(dir.x, dir.y).normalized;
        if (Input.GetButtonDown("Dash") || Input.GetMouseButtonDown(1))
        {
            wantsToDash = true;
        }

        /* --- Compute Velocity --- */

        if (canDash && wantsToDash)
        {
            isDashing = true;
            dashDirection = desiredDashDirection;
            lastDashTime = Time.time;
            canDash = false;
            gravityModifier = 0;

            source.PlayOneShot(dashSound);
        }
        wantsToDash = false;

        if (isDashing)
        {
            //Is dashing, play dash animation
            velocity = dashDirection * dashSpeed;
            if (Time.time - lastDashTime >= dashTime)
            {
                isDashing = false;
                    
                gravityModifier = defaultGravityModifier;
                if ((gravityModifier >= 0 && velocity.y > 0) ||
                    (gravityModifier < 0 && velocity.y < 0))
                {
                    velocity.y *= jumpDeceleration;
                }
            }
        }
        else
        {
            if (isGrounded)
            {
                // Store grounded time to allow for late jumps
                lastGroundedTime = Time.time;
                canJump = true;
                if (!isDashing && Time.time - lastDashTime >= dashCooldown)
                    canDash = true;
            }

            // Check time for buffered jumps and late jumps
            float timeSinceJumpInput = Time.time - lastJumpTime;
            float timeSinceLastGrounded = Time.time - lastGroundedTime;

            if (canJump && timeSinceJumpInput <= jumpBufferTime && timeSinceLastGrounded <= coyoteTime)
            {
                velocity.y = Mathf.Sign(gravityModifier) * jumpSpeed;
                canJump = false;
                isGrounded = false;
                    
                source.PlayOneShot(jumpSound);
            }
            else if (jumpReleased)
            {
                // Decelerate upwards velocity when jump button is released
                if ((gravityModifier >= 0 && velocity.y > 0) ||
                    (gravityModifier < 0 && velocity.y < 0))
                {
                    velocity.y *= jumpDeceleration;
                }
                jumpReleased = false;
            }

            velocity.x = move.x * maxSpeed;
        }

        /* --- Adjust Sprite --- */

        // Assume the sprite is facing right, flip it if moving left
        if (move.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (move.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }

        spriteRenderer.color = canDash ? canDashColor : cantDashColor;
        spotLight.color = canDash ? canDashColor : cantDashColor;
        
        setAnimation();
    }

    private void setAnimation()
    {

        if (isDashing)
        {
            //Play dash animation
            _animator.Play("PlayerDash");
        }
        else if ((move.x < 0.1 && move.x > -0.1) && (move.y < 0.1 && move.y > -0.1) && isGrounded)
        {
            //Play run animation
            _animator.Play("PlayerIdle");
        }
        else if (!canJump)
        {
            //Play jump animation
            _animator.Play("PlayerJump");
        }
        else
        {
            //Play idle animation
            _animator.Play("PlayerRun");
        }
        
        
    }

    public void ResetPlayer()
    {
        transform.position = startPosition;
        
        //spriteRenderer.sprite = spriteNotFlipped;
        spriteRenderer.flipX = false;
        
        lastJumpTime = -jumpBufferTime * 2;

        velocity = Vector2.zero;
    }

    public void ResetDash()
    {
        canDash = true;
    }

    public void setIsDead()
    {
        velocity.x = 0;
        velocity.y = -Mathf.Abs(velocity.y);
        Destroy(_lightAnchor);
        _animator.Play("PlayerDead"); 
        _isDead = true;
    }

    public bool getIsDashing()
    {
        return isDashing;
    }
}
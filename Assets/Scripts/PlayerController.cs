using System;
using UnityEngine;
using UnityEngine.Events;

public enum PlayerState
{
    Idle,
    Walk,
    Up,
    Down,
    Special
}

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 3.0f;
    public float jumpForce = 5.0f;
    public UnityEvent special;

    [Header("Ground Settings")] public LayerMask groundLayer;
    public Vector2 groundSize = new Vector2(0.4f, 0.2f);

    Rigidbody2D rb2d;
    float axisH = 0.0f;
    PlayerState currentState = PlayerState.Idle;
    private float onGroundTimer = 0.1f;
    int jumpCount = 0;

    private bool onGround;
    private bool wasGround;

    Animator animator;

    protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameManager.gameState = "playing";
    }

    public void ChangeState(PlayerState newState)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Walk:
                break;
            case PlayerState.Up:
                break;
            case PlayerState.Down:
                break;
            case PlayerState.Special:
                animator.SetBool("Special", false);
                break;
        }

        switch (newState)
        {
            case PlayerState.Idle:
                animator.SetBool("isMoving", false);
                break;
            case PlayerState.Walk:
                animator.SetBool("isMoving", true);
                break;
            case PlayerState.Up:
                animator.SetInteger("ySpeed", 1);
                break;
            case PlayerState.Down:
                animator.SetInteger("ySpeed", -1);
                break;
            case PlayerState.Special:
                special.Invoke();
                rb2d.linearVelocityX = 0;
                break;
        }

        currentState = newState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position - new Vector3(0, 1, 0), groundSize);
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.gameState != "playing")
            return;

        axisH = Input.GetAxis("Horizontal");

        if (axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGroundTimer > 0 && jumpCount > 0)
        {
            rb2d.linearVelocityY = jumpForce;
            jumpCount -= 1;
        }
    }

    private void FixedUpdate()
    {
        onGround = false;
        if (Physics2D.OverlapBox(transform.position - new Vector3(0, 1, 0), groundSize, 0f, groundLayer))
        {
            onGroundTimer = 0.3f;
            onGround = true;
        }
        else if (onGroundTimer > 0)
        {
            onGroundTimer -= Time.deltaTime;
        }

        if (!wasGround && onGround)
        {
            jumpCount = 1;
        }

        wasGround = onGround;
        if (currentState != PlayerState.Special)
        {
            rb2d.linearVelocity = new Vector2(axisH * speed, rb2d.linearVelocity.y);
        }

        if (GameManager.gameState != "playing")
            return;

        if (onGroundTimer > 0)
        {
            if (currentState.Equals(PlayerState.Down) || currentState.Equals(PlayerState.Up))
            {
                animator.SetInteger("ySpeed", 0);
                ChangeState(PlayerState.Idle);
            }

            if (axisH != 0.0f)
            {
                if (currentState.Equals(PlayerState.Idle))
                {
                    ChangeState(PlayerState.Walk);
                }
            }
            else
            {
                if (!currentState.Equals(PlayerState.Idle))
                {
                    ChangeState(PlayerState.Idle);
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeState(PlayerState.Special);
            }
        }

        if (rb2d.linearVelocity.y > 0.0f)
        {
            ChangeState(PlayerState.Up);
        }
        else if (rb2d.linearVelocity.y < 0.0f)
        {
            ChangeState(PlayerState.Down);
        }
    }
}
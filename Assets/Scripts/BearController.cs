using System;
using UnityEngine;
using UnityEngine.Events;

public enum BearState
{
    Idle,
    Walk,
    Up,
    Down,
    Special,
    Dead
}

public class BearController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 3.0f;
    public float jumpForce = 6.0f;
    public UnityEvent special;
    public GameObject tagPlayer;

    [Header("Ground Settings")] public LayerMask groundLayer;
    public Vector2 groundSize = new Vector2(0.4f, 0.2f);

    Rigidbody2D rb2d;
    Collider2D c2d;
    float axisH = 0.0f;
    public BearState currentState = BearState.Idle;
    private float onGroundTimer = 0.1f;
    int jumpCount = 0;

    private bool onGround;
    private bool wasGround;

    Animator animator;

    protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        c2d = GetComponent<Collider2D>();
        GameManager.gameState = "playing";
    }

    public void ChangeState(BearState newState)
    {
        switch (currentState)
        {
            case BearState.Idle:
                break;
            case BearState.Walk:
                break;
            case BearState.Up:
                animator.SetInteger("ySpeed", 0);
                break;
            case BearState.Down:
                animator.SetInteger("ySpeed", 0);
                break;
            case BearState.Special:
                animator.SetBool("Special", false);
                break;
        }

        switch (newState)
        {
            case BearState.Idle:
                animator.SetBool("isMoving", false);
                break;
            case BearState.Walk:
                animator.SetBool("isMoving", true);
                break;
            case BearState.Up:
                animator.SetInteger("ySpeed", 1);
                break;
            case BearState.Down:
                animator.SetInteger("ySpeed", -1);
                break;
            case BearState.Special:
                special.Invoke();
                rb2d.linearVelocityX = 0;
                break;
            case BearState.Dead:
                animator.SetBool("Dead", true);
                rb2d.AddForce(new Vector2(0, 7), ForceMode2D.Impulse);
                c2d.enabled = false;
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

        if (Input.GetKeyDown(KeyCode.UpArrow) && onGroundTimer > 0 && jumpCount > 0)
        {
            rb2d.linearVelocityY = jumpForce;
            jumpCount -= 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            tagPlayer.SetActive(true);
            tagPlayer.transform.position = transform.position - new Vector3(0, 0.31f, 0);
            tagPlayer.transform.localScale = transform.localScale;
            tagPlayer.GetComponent<Rigidbody2D>().linearVelocity = rb2d.linearVelocity; //속도 공유(캐릭터가 움직이고 있을때 태그시)
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        onGround = false;
        if (Physics2D.OverlapBox(transform.position - new Vector3(0, 1, 0), groundSize, 0f, groundLayer))
        {
            onGroundTimer = 0.1f;
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
        if (currentState != BearState.Special)
        {
            rb2d.linearVelocity = new Vector2(axisH * speed, rb2d.linearVelocity.y);
        }

        if (GameManager.gameState != "playing")
            return;

        if (onGroundTimer > 0)
        {
            if (currentState.Equals(BearState.Down) || currentState.Equals(BearState.Up))
            {
                ChangeState(BearState.Idle);
            }

            if (axisH != 0.0f)
            {
                if (currentState.Equals(BearState.Idle))
                {
                    ChangeState(BearState.Walk);
                }
            }
            else
            {
                if (!currentState.Equals(BearState.Idle))
                {
                    ChangeState(BearState.Idle);
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeState(BearState.Special);
            }
        }

        else
        {
            if (rb2d.linearVelocity.y > 0.0f)
            {
                ChangeState(BearState.Up);
            }
            else if (rb2d.linearVelocity.y <= 0.0f)
            {
                ChangeState(BearState.Down);
            }
        }
    }

    protected void Dead()
    {
        ChangeState(BearState.Dead);
    }
}
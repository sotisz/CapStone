using System;
using UnityEngine;
using UnityEngine.Events;

public enum TigerState
{
    Idle,
    Walk,
    Up,
    Down,
    Special
}
public class TigerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 6.0f;
    public float jumpForce = 8.0f;
    public UnityEvent special;
    public GameObject tagPlayer;
    public GameObject Renderer;

    [Header("Ground Settings")] public LayerMask groundLayer;
    public Vector2 groundSize = new Vector2(0.4f, 0.2f);
    public Vector2 groundOffset = new Vector2(0.4f, -0.73f);

    Rigidbody2D rb2d;
    float axisH = 0.0f;
    public TigerState currentState = TigerState.Idle;
    private float onGroundTimer = 0.1f;
    int jumpCount = 0;

    private bool onGround;
    private bool wasGround;

    Animator animator;

    protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        GameManager.gameState = "playing";
    }

    public void ChangeState(TigerState newState)
    {
        switch (currentState)
        {
            case TigerState.Idle:
                break;
            case TigerState.Walk:
                break;
            case TigerState.Up:
                animator.SetInteger("ySpeed", 0);
                break;
            case TigerState.Down:
                animator.SetInteger("ySpeed", 0);
                break;
            case TigerState.Special:
                animator.SetBool("Special", false);
                break;
        }

        switch (newState)
        {
            case TigerState.Idle:
                animator.SetBool("isMoving", false);
                break;
            case TigerState.Walk:
                animator.SetBool("isMoving", true);
                break;
            case TigerState.Up:
                animator.SetInteger("ySpeed", 1);
                break;
            case TigerState.Down:
                animator.SetInteger("ySpeed", -1);
                break;
            case TigerState.Special:
                special.Invoke();
                rb2d.linearVelocityX = 0;
                break;
        }

        currentState = newState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 offset = groundOffset;
        if (transform.localScale.x < 0)
        {
            offset.x = -offset.x;
        }
        Gizmos.DrawWireCube(transform.position + (Vector3)offset, groundSize);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tagPlayer.SetActive(true);
            tagPlayer.transform.position = transform.position + new Vector3(0, 0.31f, 0);
            tagPlayer.transform.localScale = transform.localScale;
            tagPlayer.GetComponent<Rigidbody2D>().linearVelocity = rb2d.linearVelocity;//속도 공유(캐릭터가 움직이고 있을때 태그시)
            gameObject.SetActive(false);
        }

    }

    private void FixedUpdate()
    {
        onGround = false;
        Vector2 offset = groundOffset;
        if (transform.localScale.x < 0)
        {
            offset.x = -offset.x;
        }
        if (Physics2D.OverlapBox(transform.position + (Vector3)offset, groundSize, 0f, groundLayer))
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
        if (currentState != TigerState.Special)
        {
            rb2d.linearVelocity = new Vector2(axisH * speed, rb2d.linearVelocity.y);
        }

        if (GameManager.gameState != "playing")
            return;

        if (onGroundTimer > 0)
        {
            if (currentState.Equals(TigerState.Down) || currentState.Equals(TigerState.Up))
            {
                ChangeState(TigerState.Idle);
            }

            if (axisH != 0.0f)
            {
                if (currentState.Equals(TigerState.Idle))
                {
                    ChangeState(TigerState.Walk);
                }
            }
            else
            {
                if (!currentState.Equals(TigerState.Idle))
                {
                    ChangeState(TigerState.Idle);
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeState(TigerState.Special);
            }
        }

        else
        {
            if (rb2d.linearVelocity.y > 0.0f)
            {
                ChangeState(TigerState.Up);
            }
            else if (rb2d.linearVelocity.y <= 0.0f)
            {
                ChangeState(TigerState.Down);
            }
        }
    }
}
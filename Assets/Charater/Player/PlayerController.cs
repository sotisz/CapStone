using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public enum PlayerState
{
    Idle,
    Walk,
    Up,
    Down,
    Punch
}

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rb2d;
    public float axisH = 0.0f;
    public float speed = 3.0f;

    [Header("Ground Settings")] public LayerMask groundLayer;
    public Vector2 groundSize = new Vector2(0.4f, 0.2f);

    public float jumpForce = 5.0f;
    bool onGround = false;
    public PlayerState currentState = PlayerState.Idle;
    public static string gameState = "playing";
    
    public GameObject gaugeBarbackground;

    Animator animator;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameState = "playing";
    }
    
    void ChangeState(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Idle:
                animator.SetBool("BearPunch", false);
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
            case PlayerState.Punch:
                animator.SetBool("BearPunch", true);
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
        if (gameState != "playing")
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
            
        

        if (Input.GetKeyDown(KeyCode.Space) && onGround && !currentState.Equals(PlayerState.Punch))
        {
            Vector2 jumpP2 = new Vector2(0, jumpForce);
            rb2d.AddForce(jumpP2, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        onGround = Physics2D.OverlapBox(transform.position - new Vector3(0, 1, 0), groundSize, 0f, groundLayer);
        rb2d.linearVelocity = new Vector2(axisH * speed, rb2d.linearVelocity.y);
        if (gameState != "playing")
            return;

        if (onGround)
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
                if (currentState.Equals(PlayerState.Walk))
                {
                    ChangeState(PlayerState.Idle);
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeState(PlayerState.Punch);
            }
        }
        else
        {
            if (rb2d.linearVelocity.y > 0.0f)
            {
                ChangeState(PlayerState.Up);
            }
            else
            {
                ChangeState(PlayerState.Down);
            }
        }
    }
  

}
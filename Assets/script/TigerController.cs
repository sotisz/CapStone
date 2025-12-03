using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TigerState
{
    Idle,
    Walk,
    Floating,
    Special,
    Dead
}

public class TigerController : MonoBehaviour, IKillable
{
    public AudioSource climbingSound;
    public AudioSource footstepSound;
    public AudioSource jumpSound;
    public AudioSource deathSound;

    private float footstepTimer = 0f;
    public float footstepInterval = 0.4f; // 발자국 간격
    private float climbSoundTimer = 0f;
    public float climbSoundInterval = 0.35f; // 벽 오르기 사운드 간격

    
    public float speed = 6.0f;
    public float climbSpeed = 4.0f;
    public float jumpForce = 8.0f;
    public GameObject tagPlayer;
    public Tagbar tagbar;

    [Header("Ground Settings")] public LayerMask groundLayer;
    public Vector2 groundSize = new Vector2(0.4f, 0.2f);
    public Vector2 groundOffset = new Vector2(0.4f, -0.73f);

    Rigidbody2D rb2d;
    Collider2D c2d;
    float axisH = 0.0f;
    float axisV = 0.0f;
    public TigerState currentState = TigerState.Idle;
    private float onGroundTimer = 0.1f;


    public bool onGround;
    private int wallDir;
    private bool jumpCount = false;
    private bool walljump = false;
    private bool groundjump = false;
    private bool checkWall = true;

    Animator animator;

    protected void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        c2d = GetComponent<Collider2D>();
    }

    public void ChangeState(TigerState newState)
    {
        switch (currentState)
        {
            case TigerState.Idle:
                groundjump = false;
                break;
            case TigerState.Walk:
                groundjump = false;
                break;
            case TigerState.Floating:
                animator.SetInteger("ySpeed", 0);
                break;
            case TigerState.Special:
                rb2d.gravityScale = 1;
                animator.SetBool("Special", false);
                animator.speed = 1;
                walljump = false;
                StartCoroutine(Cooldown());
                if (rb2d.linearVelocity.y > 0)
                {
                    rb2d.AddForce(Vector2.up * 3.0f, ForceMode2D.Impulse);
                }

                break;
        }

        switch (newState)
        {
            case TigerState.Idle:
                groundjump = true;
                animator.SetBool("isMoving", false);
                break;
            case TigerState.Walk:
                groundjump = true;
                animator.SetBool("isMoving", true);
                break;
            case TigerState.Floating:
                jumpCount = false;
                break;
            case TigerState.Special:
                animator.SetBool("Special", true);
                rb2d.gravityScale = 0;
                rb2d.linearVelocityX = 0;
                walljump = true;
                break;
            case TigerState.Dead:
                animator.SetBool("Dead", true);
                rb2d.linearVelocity = new Vector2(0, 7.0f);
                c2d.enabled = false;
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
    public void Update()
    {
        if (GameManager.Instance.gameState != "playing" || currentState.Equals(TigerState.Dead))
        {
            return;
        }

        rb2d.bodyType = RigidbodyType2D.Dynamic;
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");

        if (axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (walljump || groundjump)
        {
            jumpCount = true;
        }

        RaycastHit2D hit = default;
        if (checkWall)
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(axisH, 0), 1f,
                1 << LayerMask.NameToLayer("Wall"));
        }

        if (hit && currentState != TigerState.Special && (currentState == TigerState.Floating))
        {
            wallDir = (int)axisH;
            ChangeState(TigerState.Special);
        }
        else if (!hit && currentState == TigerState.Special)
        {
            if (onGroundTimer <= 0)
            {
                ChangeState(TigerState.Floating);
            }
            else
            {
                ChangeState(TigerState.Idle);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount)
        {
            if (currentState != TigerState.Special)
            {
                rb2d.linearVelocityY = jumpForce;
                jumpCount = false;
            }
            else
            {
                rb2d.linearVelocityY = jumpForce;
                rb2d.linearVelocityX = jumpForce * -wallDir;
                jumpCount = false;
            }

            ChangeState(TigerState.Floating);
            
            if (jumpSound != null)
                jumpSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.R) && onGround && tagbar.tagAble)
        {
            tagbar.TagPlayer();
            tagPlayer.SetActive(true);
            tagPlayer.transform.position = transform.position + new Vector3(0, 0.31f, 0);
            tagPlayer.transform.localScale = transform.localScale;
            tagPlayer.GetComponent<Rigidbody2D>().linearVelocity = rb2d.linearVelocity;
            gameObject.SetActive(false);
        }
        
        if (onGround && currentState == TigerState.Walk)
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                footstepSound.Play();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.gameState != "playing" || currentState.Equals(TigerState.Dead))
            return;

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

        if (currentState != TigerState.Special)
        {
            var veloTarget = axisH * speed;
            if (currentState == TigerState.Floating)
            {
                rb2d.linearVelocityX = Mathf.Lerp(rb2d.linearVelocityX, veloTarget, Time.deltaTime * 2);
            }
            else
            {
                rb2d.linearVelocityX = veloTarget;
            }
        }
        else
        {
            rb2d.linearVelocity = new Vector2(axisH * speed, axisV * climbSpeed);
            if (Mathf.Approximately(rb2d.linearVelocityY, 0))
            {
                animator.speed = 0;
            }
            else if (rb2d.linearVelocityY > 0)
            {
                animator.speed = 1;
                animator.SetBool("isClimbingUp", true);
            }
            else
            {
                animator.speed = 1;
                animator.SetBool("isClimbingUp", false);
            }
        }
        
        // 벽 오르기 사운드
        if (currentState == TigerState.Special && rb2d.linearVelocityY > 0.1f)
        {
            climbSoundTimer -= Time.deltaTime;

            if (climbSoundTimer <= 0f)
            {
                if (climbingSound != null)
                    climbingSound.Play();

                climbSoundTimer = climbSoundInterval;
            }
        }
        else
        {
            climbSoundTimer = 0f;
        }


        if (currentState == TigerState.Special)
            return;
        if (onGroundTimer > 0)
        {
            if (currentState.Equals(TigerState.Floating))
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
        }

        else
        {
            ChangeState(TigerState.Floating);
        }

        if (currentState == TigerState.Floating)
        {
            if (rb2d.linearVelocityY > 0)
            {
                animator.SetInteger("ySpeed", 1);
            }

            if (rb2d.linearVelocityY < 0)
            {
                animator.SetInteger("ySpeed", -1);
            }
        }
    }

    private IEnumerator Cooldown()
    {
        checkWall = false;
        yield return new WaitForSeconds(0.3f);
        checkWall = true;
    }

    public void Dead()
    {
        if (currentState == TigerState.Dead)
        {
            return;
        }

        ChangeState(TigerState.Dead);

        if (GameManager.Instance != null)
            GameManager.Instance.deathCount++;

        // 🔊 죽음 사운드 재생
        if (deathSound != null)
        {
            deathSound.Play();
        }
        StartCoroutine(RestartScene());
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
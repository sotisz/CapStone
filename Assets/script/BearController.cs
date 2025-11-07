using System;
using System.Collections;
using System.Collections.Generic;
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
    public float speed = 3.0f;
    public float jumpForce = 6.0f;
    public UnityEvent special;
    public GameObject tagPlayer;
    public Tagbar tagbar;


    [Header("Ground Settings")]
    public LayerMask groundLayer;
    public Vector2 groundSize = new Vector2(0.4f, 0.2f);

    [Header("Pathfinding")]
    [Tooltip("A* 경로 탐색의 목적지 (인스펙터에서 설정)")]
    public Transform pathfindingTarget;

    Rigidbody2D rb2d;
    Collider2D c2d;
    float axisH = 0.0f;
    public BearState currentState = BearState.Idle;
    private float onGroundTimer = 0.1f;
    int jumpCount = 0;

    public bool onGround;
    private bool wasGround;
    private bool canPunch = true;
    private float lookdir = 1f;

    Animator animator;

    private Pathfinder pathfinder;
    private bool isShowingPath = false;
    private List<WaypointNode> activePath = new List<WaypointNode>();
    private WaypointNode[] allNodesInScene;
    
    protected void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        c2d = GetComponent<Collider2D>();
    }

    protected void Start()
    {
        GameManager.gameState = "playing";
        
        FindPathfinderAndNodes();
    }
    
    private void FindPathfinderAndNodes()
    {
        if (pathfinder == null) 
        {
            pathfinder = FindObjectOfType<Pathfinder>();
        }
        if (pathfinder == null)
        {
            Debug.LogError("Pathfinder를 씬에서 찾을 수 없습니다!");
        }
        
        if (allNodesInScene == null || allNodesInScene.Length == 0)
        {
            allNodesInScene = FindObjectsOfType<WaypointNode>();
        }
    }

    private void OnEnable()
    {
        isShowingPath = false; 
    }

    private void OnDisable()
    {

        if (activePath != null)
        {
            foreach (WaypointNode node in activePath)
            {
                if (node != null) node.HideEffect();
            }
            activePath.Clear(); 
        }
        isShowingPath = false;
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
                animator.SetBool("Punch", false);
                animator.SetBool("Smell", false);
                animator.SetBool("Push", false);
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
        Gizmos.DrawWireCube(transform.position + new Vector3(1f * lookdir, 0, 0), new Vector3(1f, 1.8f, 0));
    }

    public void Update()
    {
        if (GameManager.gameState != "playing")
        {
            return;
        }

        rb2d.bodyType = RigidbodyType2D.Dynamic;
        axisH = Input.GetAxis("Horizontal");

        if (axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
            lookdir = 1f;
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
            lookdir = -1f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGroundTimer > 0 && jumpCount > 0 && canPunch)
        {
            rb2d.linearVelocityY = jumpForce;
            jumpCount -= 1;
        }

        if (Input.GetKeyDown(KeyCode.R) && onGround && tagbar.tagAble)
        {
            tagbar.TagPlayer();
            tagPlayer.SetActive(true);
            tagPlayer.transform.position = transform.position - new Vector3(0, 0.31f, 0);
            tagPlayer.transform.localScale = transform.localScale;
            tagPlayer.GetComponent<Rigidbody2D>().linearVelocity = rb2d.linearVelocity;
            gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F) && canPunch && onGround)
        {
            ChangeState(BearState.Special);
            StartCoroutine(Cooldown());
        }

        if (Input.GetKeyDown(KeyCode.S) && onGround)
        {
            if (!isShowingPath)
            {
                StartCoroutine(ShowNode());
            }
        }

        var raySize = c2d.bounds.size;
        raySize.y -= 0.1f;
        if (!Mathf.Approximately(axisH, 0.0f))
        {
            if (Physics2D.BoxCast(transform.position, raySize, 0f, axisH * Vector2.right, 0f,
                    1 << LayerMask.NameToLayer("Block")))
            {
                animator.SetBool("Push", true);
            }

            else
            {
                animator.SetBool("Push", false);
            }
        }
        else
        {
            animator.SetBool("Push", false);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.gameState != "playing")
            return;
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

    private IEnumerator Cooldown()
    {
        canPunch = false;
        Collider2D hit = Physics2D.OverlapBox(transform.position + new Vector3(1f * lookdir, 0, 0),
            new Vector3(1f, 1.8f, 0), 0f,
            groundLayer);
        if (hit != null)
        {
            if (hit.CompareTag("Breakable"))
            {
                hit.GetComponent<BreakSystem>().Break();
            }
        }

        yield return new WaitForSeconds(0.5f);
        canPunch = true;
    }

    private IEnumerator ShowNode()
    {
        if (pathfinder == null)
        {
            Debug.LogWarning("Pathfinder가 null입니다. S키를 눌렀을 때 다시 검색합니다...");
            FindPathfinderAndNodes();
            
            if (pathfinder == null) 
            {
                Debug.LogError("Pathfinder를 여전히 찾을 수 없습니다! ShowNode를 중단합니다.");
                yield break;
            }
        }

        if (pathfindingTarget == null)
        {
            Debug.LogWarning("pathfindingTarget이 설정되지 않았습니다! Bear의 인스펙터에서 Target을 지정해주세요.");
            yield break;
        }

        isShowingPath = true;
        Debug.Log("S키 눌렀음: A* 경로 탐색 및 표시 시작");

        WaypointNode startNode = pathfinder.FindClosestWaypoint(transform.position);
        WaypointNode targetNode = pathfinder.FindClosestWaypoint(pathfindingTarget.position);
        
        activePath.Clear();
        activePath = pathfinder.FindPath(startNode, targetNode);

        if (activePath != null && activePath.Count > 0)
        {
            for (int i = 0; i < activePath.Count; i++)
            {
                WaypointNode node = activePath[i];
                WaypointNode nextNode = (i < activePath.Count - 1) ? activePath[i + 1] : null;
                if(node != null) node.ShowEffect(nextNode);
            }
        }
        else
        {
            Debug.Log("경로를 찾을 수 없습니다.");
        }

        yield return new WaitForSeconds(7f);

        Debug.Log("7초 경과: A* 경로 표시 종료");

        if (activePath != null)
        {
            foreach (WaypointNode node in activePath)
            {
                if(node != null) node.HideEffect();
            }
            activePath.Clear();
        }

        isShowingPath = false;
    }

    protected void Dead()
    {
        ChangeState(BearState.Dead);
    }
}
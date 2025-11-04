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

    // A* 경로 탐색 관련 변수
    private Pathfinder pathfinder;
    private bool isShowingPath = false;
    private List<WaypointNode> activePath = new List<WaypointNode>();
    private WaypointNode[] allNodesInScene;
    
    // [수정] Awake: '자기 자신'의 컴포넌트를 가져옵니다.
    protected void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        c2d = GetComponent<Collider2D>();
    }

    // [수정] Start: '다른' 오브젝트(Pathfinder, Nodes)를 찾습니다.
    protected void Start()
    {
        GameManager.gameState = "playing";
        
        // Start에서 찾는 것이 OnEnable에서 찾는 것보다 안전합니다.
        FindPathfinderAndNodes();
    }
    
    // [신규] Pathfinder와 노드를 찾는 함수
    private void FindPathfinderAndNodes()
    {
        // 씬에서 Pathfinder를 찾습니다.
        if (pathfinder == null) 
        {
            pathfinder = FindObjectOfType<Pathfinder>();
        }
        if (pathfinder == null)
        {
            Debug.LogError("Pathfinder를 씬에서 찾을 수 없습니다!");
        }
        
        // 씬의 모든 노드를 찾습니다.
        if (allNodesInScene == null || allNodesInScene.Length == 0)
        {
            allNodesInScene = FindObjectsOfType<WaypointNode>();
        }
    }

    // [수정] OnEnable: 태그 시 플래그 리셋만 담당합니다.
    private void OnEnable()
    {
        // 활성화될 때는 플래그만 리셋합니다.
        isShowingPath = false; 
    }

    // [수정] OnDisable: 태그 시 이펙트/플래그 정리
    private void OnDisable()
    {
        Debug.Log("BearController가 비활성화(OnDisable)됩니다. 경로 이펙트를 강제 종료합니다.");

        // 코루틴이 중지되므로, 켜져 있던 이펙트를 수동으로 끕니다.
        if (activePath != null)
        {
            foreach (WaypointNode node in activePath)
            {
                if (node != null) node.HideEffect();
            }
            activePath.Clear(); 
        }
        // 플래그를 리셋합니다.
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

        // S키 로직 (중복 실행 방지)
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

    // [수정] ShowNode() 코루틴에 안전장치 추가
    private IEnumerator ShowNode()
    {
        // 0. [안전장치] Start에서 pathfinder를 못 찾았을 경우, 여기서 한 번 더 시도합니다.
        if (pathfinder == null)
        {
            Debug.LogWarning("Pathfinder가 null입니다. S키를 눌렀을 때 다시 검색합니다...");
            FindPathfinderAndNodes(); // 다시 찾아본다.
            
            // 그래도 null이면 코루틴을 종료합니다.
            if (pathfinder == null) 
            {
                Debug.LogError("Pathfinder를 여전히 찾을 수 없습니다! ShowNode를 중단합니다.");
                yield break; // 코루틴 즉시 종료
            }
        }

        // 0. Target 유효성 검사
        if (pathfindingTarget == null)
        {
            Debug.LogWarning("pathfindingTarget이 설정되지 않았습니다! Bear의 인스펙터에서 Target을 지정해주세요.");
            yield break; // 코루틴 즉시 종료
        }

        isShowingPath = true; // 유효성 검사 통과 후 플래그 설정
        Debug.Log("S키 눌렀음: A* 경로 탐색 및 표시 시작");

        // 1. A* 경로 계산
        WaypointNode startNode = pathfinder.FindClosestWaypoint(transform.position);
        WaypointNode targetNode = pathfinder.FindClosestWaypoint(pathfindingTarget.position);
        
        activePath.Clear(); // 이전 경로가 남아있을 수 있으니 비웁니다.
        activePath = pathfinder.FindPath(startNode, targetNode);

        // 2. 경로가 있다면 이펙트 켜기
        if (activePath != null && activePath.Count > 0)
        {
            for (int i = 0; i < activePath.Count; i++)
            {
                WaypointNode node = activePath[i];
                // '다음' 노드를 찾습니다. (경로의 마지막 노드는 nextNode가 null이 됨)
                WaypointNode nextNode = (i < activePath.Count - 1) ? activePath[i + 1] : null;
                if(node != null) node.ShowEffect(nextNode);
            }
        }
        else
        {
            Debug.Log("경로를 찾을 수 없습니다.");
        }

        // 3. 15초간 대기
        yield return new WaitForSeconds(7f);

        Debug.Log("7초 경과: A* 경로 표시 종료");

        // 4. 켰던 이펙트 끄기
        if (activePath != null)
        {
            foreach (WaypointNode node in activePath)
            {
                if(node != null) node.HideEffect();
            }
            activePath.Clear();
        }

        isShowingPath = false; // 플래그 해제
    }

    protected void Dead()
    {
        ChangeState(BearState.Dead);
    }
}
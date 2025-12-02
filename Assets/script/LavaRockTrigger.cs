using UnityEngine;

public class LavaRockTrigger : MonoBehaviour
{
    [Header("연결할 오브젝트")]
    public GameObject breakableFloor; // 부서질 바닥
    public Transform lava;            // 용암 (Transform만 있으면 움직일 수 있음!)

    [Header("설정값")]
    public float rockGravity = 3.0f;  // 돌 떨어지는 속도
    public float lavaRiseSpeed = 2.0f;// 용암 올라오는 속도
    public float lavaStopY = -2.0f;   // 용암이 멈출 Y 높이

    private Rigidbody2D rb;
    private bool hasFallen = false;   // 돌이 떨어졌는지 체크
    private bool isLavaRising = false;// 용암이 올라오는 중인지 체크

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 돌은 처음에 공중에 고정 (Kinematic)
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        // 용암을 위로 이동시키는 로직 (RisingLava 기능을 여기로 통합)
        if (isLavaRising && lava != null)
        {
            if (lava.position.y < lavaStopY)
            {
                lava.Translate(Vector3.up * lavaRiseSpeed * Time.deltaTime);
            }
        }
    }

    // 1. 플레이어 감지 (Trigger): 돌 떨구기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasFallen && (collision.CompareTag("Player") || collision.gameObject.name.Contains("Player")))
        {
            Debug.Log("플레이어 감지! 낙석 시작.");
            hasFallen = true;
            
            // 물리 엔진을 켜서 돌을 떨어뜨림
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = rockGravity;
        }
    }

    // 2. 바닥 충돌 감지 (Collision): 바닥 파괴 & 용암 시작
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 내가 부딪힌 게 '부서질 바닥'이라면
        if (collision.gameObject == breakableFloor)
        {
            Debug.Log("쿵! 바닥 파괴 & 용암 분출.");
            
            // 바닥 없애기
            breakableFloor.SetActive(false);
            
            rb.linearVelocity = Vector2.zero;       // 움직임 멈춤
            rb.angularVelocity = 0f;          // 회전 멈춤
            rb.bodyType = RigidbodyType2D.Kinematic; // 물리 연산 중지 (고정)
            
            // 용암 올리기 스위치 ON
            if (lava != null)
            {
                lava.gameObject.SetActive(true); // 혹시 꺼져있으면 켜고
                isLavaRising = true;
            }
        }
    }
}
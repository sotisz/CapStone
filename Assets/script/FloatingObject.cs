using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("부력 설정")]
    public float floatStrength = 5f; // 질량을 곱하므로 값을 좀 줄여서 시작해보세요 (예: 2~5)
    public float waterDrag = 2f;

    private Rigidbody2D rb;
    private float waterSurfaceY;
    private bool isInWater = false; // 물 안에 있는지 체크하는 변수 추가

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // 물 안에 있을 때만 부력 계산
        if (isInWater)
        {
            // 물 표면보다 아래에 있을 때
            if (transform.position.y < waterSurfaceY)
            {
                float displacement = waterSurfaceY - transform.position.y;

                // [핵심 수정] 부력 계산에 rb.mass(질량)를 곱함
                // 공식: 깊이 * 부력계수 * 질량 + (중력상쇄를 위한 보정)
                // 단순히 질량을 곱해주면 무게가 100이어도 잘 뜹니다.
                Vector2 buoyancy = Vector2.up * (displacement * floatStrength * rb.mass);

                rb.AddForce(buoyancy, ForceMode2D.Force);

                // 물 속 저항 (감속) 적용
                // Unity 6 이상: linearVelocity, 이전 버전: velocity
                rb.linearVelocity *= (1f - Time.fixedDeltaTime * waterDrag);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isInWater = true;
            // 물에 닿자마자 표면 높이 갱신
            waterSurfaceY = collision.bounds.max.y;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isInWater = true;
            // 물 높이가 변할 수 있으므로 계속 갱신
            waterSurfaceY = collision.bounds.max.y;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isInWater = false;
        }
    }
}
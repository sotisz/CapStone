using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float speed = 3f;        // 이동 속도
    public Transform stopPoint;     // 도착 지점
    private bool isMove = false;  // 플레이어 접촉 후 true

    void Update()
    {
        if (!isMove) return;

        // 오른쪽으로 이동
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // 도착지점 도달 → 멈춤
        if (Vector2.Distance(transform.position, stopPoint.position) < 0.1f)
        {
            isMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 접촉 시 움직임 시작
        if (collision.CompareTag("Player"))
        {
            isMove = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isMove = true;
        }
    }
}
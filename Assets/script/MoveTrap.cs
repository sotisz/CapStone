using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float speed = 2f;              // 이동 속도
    public Vector2 moveDirection = Vector2.right;  // 이동 방향
    public Transform stopPoint;           // 도착 지점
    private bool isMove = false;          // 움직임 여부

    void Update()
    {
        if (!isMove) return;
        
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
        
        if (Vector2.Distance(transform.position, stopPoint.position) < 0.1f)
        {
            isMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
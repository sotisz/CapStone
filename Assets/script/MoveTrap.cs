using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float speed = 2f; // 이동 속도
    public Vector2 moveDirection = Vector2.right; // 이동 방향
    public Transform stopPoint; // 도착 지점
    private bool isMove = false; // 움직임 여부

    void Update()
    {
        if (isMove) transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
        else if(!isMove)
        {
            transform.Translate(0 * speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isMove = true;
        }

        if (collision.CompareTag("EndPoint"))
        {
            isMove = false;
         
        }
    }
    
}
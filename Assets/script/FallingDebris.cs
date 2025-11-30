using UnityEngine;

public class FallingDebris : MonoBehaviour
{
    public float destroyDelay = 0.5f; 

    // [수정] OnCollisionEnter2D -> OnTriggerEnter2D 로 변경
    // [수정] 매개변수 Collision2D -> Collider2D 로 변경
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 플레이어에게 맞았을 때
        if (collision.CompareTag("Player")) // gameObject.CompareTag 대신 바로 접근 가능
        {
            IKillable player = collision.GetComponent<IKillable>();
            if(player != null)
            {
                player.Dead();
            }
            Destroy(gameObject);
        }
        
        // 2. 땅(Block)에 닿았을 때
        if (collision.CompareTag("Block"))
        {
            // 여기서 사운드/이펙트 실행
            Destroy(gameObject, destroyDelay);
        }
    }
    
    private void Update()
    {
        if (transform.position.y < -20f)
        {
            Destroy(gameObject);
        }
    }
}
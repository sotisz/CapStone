using UnityEngine;

public class FallingDebris : MonoBehaviour
{
    // 돌이 땅에 닿았을 때 바로 사라질지, 약간 이펙트를 줄지 결정
    public float destroyDelay = 0.5f; 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 플레이어에게 맞았을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            IKillable player = collision.gameObject.GetComponent<IKillable>();
            if(player != null)
            {
                player.Dead();
            }
            Destroy(gameObject);
        }
        
        // 2. 땅(Ground)이나 다른 블록에 닿았을 때
        if (collision.gameObject.CompareTag("Block"))
        {
            // 사운드 혹은 이펙트 삽입 구간 //
            
            Destroy(gameObject, destroyDelay); // 잠시 후 제거
        }
    }
    
    // 화면 밖으로 너무 멀리 떨어지면 성능을 위해 제거 (혹시 땅에 안 닿았을 경우 대비)
    private void Update()
    {
        if (transform.position.y < -20f) // 맵 아래로 한참 떨어지면
        {
            Destroy(gameObject);
        }
    }
}

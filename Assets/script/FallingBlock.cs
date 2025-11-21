using System;
using System.Collections;
using UnityEngine;


public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Vector3 startPos;
    public float shakeDuration = 0.5f; // 흔들리는 시간
    public float shakeAmount = 0.2f; // 흔들리는 거리
    public float fallDelay = 0.5f; // 떨어지기까지의 대기 시간

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPos = transform.position;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // 플레이어가 닿았을 때
        {
            StartCoroutine(ShakeAndFall());
        }
    }

    private IEnumerator ShakeAndFall()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < shakeDuration)
        {
            float xOffset = Mathf.Sin(Time.time * 50f) * shakeAmount; // 좌우 흔들림
            transform.GetChild(0).position = startPos + new Vector3(xOffset, 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(fallDelay); // 추가 대기 후 떨어짐

        box.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f; // 중력 증가 (더 빠르게 떨어지도록)
        Destroy(gameObject, 2f); // 2초 후 블록 제거
    }
}
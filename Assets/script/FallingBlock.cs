using System;
using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D box;
    private Vector3 startPos;

    public float shakeDuration = 0.5f; 
    public float shakeAmount = 0.2f; 
    public float fallDelay = 0.5f;

    // 흔들리는 사운드
    public AudioClip shakeSound;
    public float shakeVolume = 1f;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        startPos = transform.position;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShakeAndFall());
        }
    }

    private IEnumerator ShakeAndFall()
    {
        if (shakeSound != null)
            AudioSource.PlayClipAtPoint(shakeSound, transform.position, shakeVolume);

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float xOffset = Mathf.Sin(Time.time * 50f) * shakeAmount;
            transform.GetChild(0).position = startPos + new Vector3(xOffset, 0, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(fallDelay);

        box.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;

        Destroy(gameObject, 2f);
    }
}
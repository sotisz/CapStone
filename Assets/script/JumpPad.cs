using System;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private float bounce = 10f;
    public float targetScaleY = 3f; // 닿았을 때 Y축 목표 크기
    public float speed = 3f; // 스케일 변화 속도
    public AudioSource jumpPadSound;
    
    private bool isTouching = false; // 닿았는지 여부
    private Vector3 originalScale; // 원래 크기

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float targetY = isTouching ? targetScaleY : originalScale.y;

        Vector3 newScale = new Vector3(originalScale.x,
            Mathf.Lerp(transform.localScale.y, targetY, Time.deltaTime * speed),
            originalScale.z);
        transform.localScale = newScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().linearVelocityY = bounce;
            isTouching = true;
        }
            if (jumpPadSound != null)
                jumpPadSound.Play();
            
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTouching = false;
        }
    }
}
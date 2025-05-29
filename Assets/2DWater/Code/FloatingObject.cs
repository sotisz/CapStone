using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatStrength = 10f;
    private Rigidbody2D rb;
    private bool inWater = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (inWater)
        {
            // 위쪽으로 부력 가하기
            rb.AddForce(Vector2.up * floatStrength, ForceMode2D.Force);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            inWater = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            inWater = false;
        }
    }
}

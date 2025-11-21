using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatStrength = 10f;
    public float waterDrag = 2f;
    private Rigidbody2D rb;
    private float waterSurfaceY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        /* if (inWater)
         {
             // 위쪽으로 부력 가하기
             rb.AddForce(Vector2.up * floatStrength, ForceMode2D.Force);
         }
        */
        // 물 표면보다 아래 있을 때만 부력 작용
        if (transform.position.y < waterSurfaceY)
        {
            float displacement = waterSurfaceY - transform.position.y;
            Vector2 buoyancy = Vector2.up * (displacement * floatStrength);
            rb.AddForce(buoyancy, ForceMode2D.Force);

            // 물 속에서는 움직임을 둔화
            rb.linearVelocity *= (1f - Time.fixedDeltaTime * waterDrag);
        }
    }

    /* void OnTriggerEnter2D(Collider2D other)
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
    */

    private void OnTriggerStay2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            waterSurfaceY = collision.bounds.max.y;
        }   
    }
}

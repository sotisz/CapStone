using UnityEngine;

public class Falling_Spike : MonoBehaviour
{
    Rigidbody2D _rb;
    EdgeCollider2D _cc;
    public Transform FSpike;
    // GameObject PlayerC;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player(Bear)") || collision.gameObject.name.Equals("Player(Tiger)"))
        {
            _rb.isKinematic = false;
        }
        
         
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player(Bear)") || collision.gameObject.name.Equals("Player(Tiger)"))
        {
            // PlayerC.GetComponent<>().Dead(); // 플레이어 죽음
        }

        if (collision.gameObject.name.Equals("Tilemap") || collision.gameObject.tag == "Player")
        {
            FSpike.gameObject.SetActive(false);
        }
    }
}

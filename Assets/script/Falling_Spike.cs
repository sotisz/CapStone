using UnityEngine;

public class Falling_Spike : MonoBehaviour
{
    Rigidbody2D _rb;
    EdgeCollider2D _cc;
    public Transform FSpike;
    public AudioClip deactivateSound;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            IKillable player = collision.gameObject.GetComponent<IKillable>();
            if(player != null)
            {
                player.Dead();
            }
        }

        if (collision.gameObject.name.Equals("Tilemap") || collision.gameObject.tag == "Player" ||
            collision.gameObject.tag == "Breakable" || collision.gameObject.tag == "Ground" || collision.gameObject.name.Equals("Ground") ) 
        {   
            if (deactivateSound != null)
                AudioSource.PlayClipAtPoint(deactivateSound, transform.position);
            
            FSpike.gameObject.SetActive(false);
        }
    }
}

using UnityEngine;

public class Spike : MonoBehaviour
{
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
    }
}

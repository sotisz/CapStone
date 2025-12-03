using UnityEngine;

public class OffBtn : MonoBehaviour
{
    public Transform Object; //상호작용 매개체
    public Transform Target; // 비활성화 할 물체
    private bool OffObject = false;

    // Update is called once per frame
    void Update()
    {
        if (OffObject == true)
        {
            Target.gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BoxObject") || Object)
        {
            OffObject = true;
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
    }
}

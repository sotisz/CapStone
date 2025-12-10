using UnityEngine;

public class StageClear : MonoBehaviour
{
    public GameManager Instance => GameManager.Instance;
    private bool isLoadScene = false; 
    public AudioClip clearSound;
    
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLoadScene == false)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (clearSound != null)
                    SoundManager.Instance.PlaySFX(clearSound);
                
                isLoadScene = true;
                Instance.FadeIn();
            }
        }
    }


}

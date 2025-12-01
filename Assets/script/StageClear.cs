using UnityEngine;

public class StageClear : MonoBehaviour
{
    public GameManager Instance => GameManager.Instance;
    private bool isLoadScene = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLoadScene == false)
        {
            if (collision.gameObject.tag == "Player")
            {
                isLoadScene = true;
                Instance.FadeIn();
            }
        }
    }


}

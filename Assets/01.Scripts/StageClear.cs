using UnityEngine;

public class StageClear : MonoBehaviour
{
    public GameManager Instance => GameManager.Instance;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instance.FadeIn();
        }
    }
}

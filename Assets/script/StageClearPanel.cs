using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearPanel : MonoBehaviour
{
    public GameObject stageClearPanel; // ¶ç¿ï ÆÐ³Î
    public GameManager Instance => GameManager.Instance;
    private bool isOneTime= false;
    private bool isLoadScene = false;
    public AudioClip clearSound;
    public TextMeshProUGUI clearTimeText;
    public TextMeshProUGUI clearDeathText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOneTime == false)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (clearSound != null)
                    SoundManager.Instance.PlaySFX(clearSound);

                isOneTime = true;
                ShowStageClear();
            }
        }
    }

    void ShowStageClear()
    {
        if (stageClearPanel != null)
        {
            stageClearPanel.SetActive(true);
            Instance.gameState = "paused";
            Time.timeScale = 0f;
            clearTimeText.text = "<color=#FFE4B5>" + (int)Instance.playTime + "</color>" + "<color=#B5651D> ÃÊ</color>";
            clearDeathText.text = "<color=#FFE4B5>" + Instance.deathCount + "</color>" + "<color=#FF6347> Á×À½</color>";
        }
    }

    public void OnClick_NextScene()
    {
        if (isLoadScene == false)
        {
                Instance.FadeIn();
            isLoadScene = true;
        }
    }
}
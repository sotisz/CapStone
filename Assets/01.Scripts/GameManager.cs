using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string gameState = "playing";

    public CanvasGroup canvasGroup; // �гο� ���� CanvasGroup ����
    public float fadeDuration = 1f; // ���̵忡 �ɸ��� �ð�(��)


    private void Awake()
    {
        // �ν��Ͻ��� ����ִٸ� �Ҵ����ְ�, 
        //�ش� ������Ʈ�� �� �̵��� �ı����� �ʰ��մϴ�.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        // �ν��Ͻ��� �̹� �Ҵ���ִٸ�(2�� �̻��̶��) �ı��մϴ�.
        else
        {
            Destroy(gameObject);
        }

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvasGroup = GameObject.FindWithTag("Fade").transform.GetChild(0).GetComponent<CanvasGroup>();
        FadeOut();
    }


    public void LoadNextScene()
    {
        int nowIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(nowIndex + 1);
    }
    public void FadeIn()
    {
        canvasGroup.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(0, 1, true));
    }

    public void FadeOut()
    {
        canvasGroup.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(1, 0, false));
    }

    IEnumerator FadeCanvasGroup(float start, float end, bool isLoad)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (isLoad)
        {
            LoadNextScene();
        }

        else
        {
            canvasGroup.alpha = end;
            canvasGroup.gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string gameState = "playing";

    public CanvasGroup canvasGroup;// 패널에 붙인 CanvasGroup 연결
    public float fadeDuration = 1f;  // 페이드에 걸리는 시간(초)

    private void Awake()
    {
        // 인스턴스가 비어있다면 할당해주고, 
        //해당 오브젝트를 씬 이동간 파괴하지 않게합니다.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        // 인스턴스가 이미 할당돼있다면(2개 이상이라면) 파괴합니다.
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
        Time.timeScale = 1f;
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

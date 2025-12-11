using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string gameState = "playing";

    private CanvasGroup canvasGroup; // 패널에 붙인 CanvasGroup 연결
    public float fadeDuration = 1f; // 페이드에 걸리는 시간(초)
    public List<string> readDialogs = new List<string>();

    public int deathCount = 0; // 데스 카운트
    public TextMeshProUGUI deathText;

    public float playTime = 0f; // 게임 진행 시간
    public TextMeshProUGUI timerText; // UI에 표시할 텍스트

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
        gameState = "playing"; // 이 부분이 있어야 레벨 재시작 시 시간이 흐름 
        Time.timeScale = 1f;

        if (GameObject.FindWithTag("Fade"))
        {
            canvasGroup = GameObject.FindWithTag("Fade").transform.GetChild(0).GetComponent<CanvasGroup>();
            FadeOut();
        }
        if (timerText == null)
        {
            GameObject timerObj = GameObject.FindWithTag("TimerText");
            if (timerObj != null)
                timerText = timerObj.GetComponent<TextMeshProUGUI>();
            else
                timerText = null;
        }

        if (deathText == null)
        {
            GameObject deathObj = GameObject.FindWithTag("DeathText");
            if (deathObj != null)
                deathText = deathObj.GetComponent<TextMeshProUGUI>();
            else
                deathText = null;
        }
    }


    public void LoadNextScene()
    {
        int nowIndex = SceneManager.GetActiveScene().buildIndex;
        string scenePath = SceneUtility.GetScenePathByBuildIndex(nowIndex + 1);

        if (!string.IsNullOrEmpty(scenePath))
        {
            // Extract the scene name from the path
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            SaveManager.SaveScene(sceneName);
            Debug.Log(sceneName);
        }
        playTime = 0f;
        deathCount = 0;
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

            if (canvasGroup == null) yield break;

            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
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

    public bool IsDialogRead(string path)
    {
        return readDialogs.Contains(path);
    }

    public void MarkDialogAsRead(string path)
    {
        if (!readDialogs.Contains(path))
        {
            readDialogs.Add(path);
        }
    }

    private void Update()
    {
        if (gameState == "playing")
        {
            Time.timeScale = 1;
            playTime += Time.deltaTime;

            if (timerText != null)
            {

                timerText.text = "<color=#FFE4B5>" + (int)playTime + "</color>" + "<color=#B5651D> 초</color>";
            }

            if (deathText != null)
            {
                deathText.text = "<color=#FFE4B5>" + deathCount + "</color>" + "<color=#FF6347> 죽음</color>";
            }
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}


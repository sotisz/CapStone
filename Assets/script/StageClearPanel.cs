using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearPanel : MonoBehaviour
{
    // 자동으로 찾을 거라 인스펙터 연결 필요 없음 (Public은 확인용)
    public GameObject stageClearPanel; 
    public GameManager Instance => GameManager.Instance;
    
    private bool isOneTime= false;
    private bool isLoadScene = false;
    public AudioClip clearSound;
    
    // UI 텍스트 (자동 연결)
    public Text clearTimeText;
    public Text clearDeathText;

    private void Start()
    {

        if (stageClearPanel == null)
        {
            // 1. 먼저 항상 켜져 있는 최상위 'Canvases' (또는 Canvas)를 찾습니다.
            GameObject canvasObj = GameObject.Find("StageClearCanvas");
            if (canvasObj != null)
            {
                // 2. 캔버스 안에서 "StageClearPanel"이라는 이름을 가진 자식을 샅샅이 뒤집니다.
                // (꺼져 있는 오브젝트도 찾아내는 함수 사용)
                Transform panelTrans = FindChildByName(canvasObj.transform, "StageClearPanel");
                
                if (panelTrans != null)
                {
                    stageClearPanel = panelTrans.gameObject;
                }
            }
        }

        // 3. 패널을 찾았다면, 그 안에서 텍스트들을 찾습니다.
        if (stageClearPanel != null)
        {
            if (clearTimeText == null || clearDeathText == null)
            {
                // (true) 옵션은 비활성화된 자식도 찾는다는 뜻입니다.
                Text[] texts = stageClearPanel.GetComponentsInChildren<Text>(true);

                foreach (Text t in texts)
                {
                    if (t.name == "ClearTime") clearTimeText = t;
                    else if (t.name == "ClearDeathCount") clearDeathText = t;
                }
            }
        }
        else
        {
            Debug.LogError("오류: 'StageClearPanel'을 찾을 수 없습니다. Hierarchy에 이름이 정확한지, Canvases 아래에 있는지 확인해주세요.");
        }
    }

    // 재귀적으로 자식을 찾는 헬퍼 함수 (깊숙이 박혀 있어도 찾아냄)
    private Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            
            Transform result = FindChildByName(child, name);
            if (result != null) return result;
        }
        return null;
    }

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

            if (clearTimeText != null)
                clearTimeText.text = "<color=#FFE4B5>" + (int)Instance.playTime + "</color>" + "<color=#B5651D> 초</color>";
            
            if (clearDeathText != null)
                clearDeathText.text = "<color=#FFE4B5>" + Instance.deathCount + "</color>" + "<color=#FF6347> 회 죽음</color>";
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
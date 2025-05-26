using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelScript : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject SettingPanel;

<<<<<<< Updated upstream
    // 테스트용 업데이트 메소드 << 나중에 게임매니저 스크립트로 옮기기
    // (게임을 멈춰야 하기 때문에)
=======

    private void OnEnable() // 일시정지 메뉴가 활성화 되면
    {
        if (GameManager.instance != null)
        {
            GameManager.gameState = "paused";
            Time.timeScale = 0f; //게임 일시정지
        }
    }
    private void OnDisable() // 일시정지 메뉴가 꺼질 때
    {
        if (GameManager.instance != null)
        {
            GameManager.gameState = "playing";
            Time.timeScale = 1f; // 게임 재개
        }
    }
>>>>>>> Stashed changes
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (PausePanel.activeSelf) // 일시정지상태에서
            {
<<<<<<< Updated upstream
                if (SettingPanel.activeSelf) // 환경설정이 켜져있으면
                    return;
                else
                    PausePanel.SetActive(false); // 일시정지 OFF
            }
            else
                PausePanel.SetActive(true); //  ON
=======
                if (SettingPanel.activeSelf)
                { // 환경설정이 켜져있으면 환경설정만 OFF

                    SettingPanel.SetActive(false);
                }
                else // 환경설정 OFF 상태에서 ESC >> 일시정지 OFF
                    PausePanel.SetActive(false); 
            }
            else
                PausePanel.SetActive(true); // ON
>>>>>>> Stashed changes
        }
    }


    public void OnRestartButton() // 레벨 재시작
    {
<<<<<<< Updated upstream
        SceneManager.LoadScene("SampleScene");
=======
        GameManager.gameState = "playing";
        Time.timeScale = 1f; // 게임 재개
        SceneManager.LoadScene("Game Scene");
>>>>>>> Stashed changes
    }

    public void OnExitButton() // 나가기 버튼
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

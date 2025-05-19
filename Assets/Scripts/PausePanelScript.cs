using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanelScript : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject SettingPanel;

    // 테스트용 업데이트 메소드 << 나중에 게임매니저 스크립트로 옮기기
    // (게임을 멈춰야 하기 때문에)
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (PausePanel.activeSelf) // 일시정지상태에서
            {
                if (SettingPanel.activeSelf) // 환경설정이 켜져있으면
                    return;
                else
                    PausePanel.SetActive(false); // 일시정지 OFF
            }
            else
                PausePanel.SetActive(true); //  ON
        }
    }


    public void OnRestartButton() // 레벨 재시작
    {
        SceneManager.LoadScene("SampleScene");
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

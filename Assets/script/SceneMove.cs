using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    public void MoveToGameScene()
    {
        SceneManager.LoadScene("Level1");
    }

    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MoveToIntroScene()
    {
        SceneManager.LoadScene("Intro");
    }

   
    public void OnExitButton()
    {
        // 2. 에디터에서 실행 중일 때만 'EditorApplication' 코드를 사용하도록 감쌉니다.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 3. 실제 빌드된 게임에서는 이 코드가 실행됩니다.
        Application.Quit();
#endif
    }
}

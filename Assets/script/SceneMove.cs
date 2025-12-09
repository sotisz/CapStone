using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void MoveToGameScene()
    {
        GameManager.Instance.playTime = 0f;
        GameManager.Instance.deathCount = 0;
        SceneManager.LoadScene("Level1");
    }

    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MoveToIntroScene()
    {
        GameManager.Instance.playTime = 0f;
        GameManager.Instance.deathCount = 0;
        GameManager.Instance.readDialogs.Clear();
        SceneManager.LoadScene("Intro");
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
    public void GoToStage(int stageNum)
    {
        GameManager.Instance.playTime = 0f;
        GameManager.Instance.deathCount = 0;
        SceneManager.LoadScene("Level" + stageNum);

        Debug.Log("Level" + stageNum + " (��)�� �̵��մϴ�.");
    }

}
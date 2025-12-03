using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        GameManager.Instance.playTime = 0f;
        GameManager.Instance.deathCount = 0;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MoveToIntroScene()
    {
        GameManager.Instance.playTime = 0f;
        GameManager.Instance.deathCount = 0;
        SceneManager.LoadScene("Intro");
    }

    public void OnExitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void GoToStage(int stageNum)
    {
        GameManager.Instance.playTime = 0f;
        GameManager.Instance.deathCount = 0;
        SceneManager.LoadScene("Level" + stageNum);

        Debug.Log("Level" + stageNum + " (으)로 이동합니다.");
    }

}
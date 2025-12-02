using UnityEditor.Build.Content;
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
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    public void GoToStage(int stageNum)
    {
        SceneManager.LoadScene("Level" + stageNum);

        Debug.Log("Stage" + stageNum + " (으)로 이동합니다.");
    }

}
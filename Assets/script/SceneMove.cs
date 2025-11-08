using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    public void MoveToGameScene()
    {
<<<<<<<< HEAD:Assets/script/SceneMove.cs
        SceneManager.LoadScene("SampleScene");
    }

========
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

>>>>>>>> 진상현:Assets/01.Scripts/SceneMove.cs
    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}

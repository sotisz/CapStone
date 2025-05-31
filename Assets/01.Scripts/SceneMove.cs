using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    public void MoveToGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void MoveToIntroScene()
    {
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

}

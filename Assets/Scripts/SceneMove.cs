using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    public void MoveToGameScene()
    {
<<<<<<< Updated upstream
        SceneManager.LoadScene("SampleScene");
=======
        SceneManager.LoadScene("Game Scene");
>>>>>>> Stashed changes
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

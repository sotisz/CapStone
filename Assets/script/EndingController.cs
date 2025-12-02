using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingController : MonoBehaviour
{
    public string endingDialogPath;
    public string nextSceneName = "Intro";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogManager.instance.DialogStart(endingDialogPath, () => 
        {
            EndGameProcess();
        });
    }
    void EndGameProcess()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Application.Quit();
            Debug.Log("Game Quit");
        }
    }
}

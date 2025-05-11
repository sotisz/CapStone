using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{

    public GameObject SettingPanel;

    public void MoveToGameScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnSettingIn()
    {
        SettingPanel.SetActive(true);
    }
    public void OnSettingExit()
    {
        SettingPanel.SetActive(false);
    }
}

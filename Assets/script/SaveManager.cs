using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public Button loadButton;
    private string lastScene;

    void Start()
    {
        if (PlayerPrefs.HasKey("lastScene"))
        {
            lastScene = PlayerPrefs.GetString("lastScene");
            loadButton.interactable = true;
        }
        else
        {
            loadButton.interactable = false;
        }
    }

    public void LoadLastScene()
    {
        SceneManager.LoadScene(lastScene);
    }

    public static void SaveScene(string sceneName)
    {
        PlayerPrefs.SetString("lastScene", sceneName);
    }
    
}

using UnityEngine;

public class SettingPanelScript : MonoBehaviour
{

    public GameObject SettingPanel;

    public void OnSettingIn()
    {
        SettingPanel.SetActive(true);
    }
    public void OnSettingExit()
    {
        SettingPanel.SetActive(false);
    }
}

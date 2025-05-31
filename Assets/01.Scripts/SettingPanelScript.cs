using UnityEngine;

// 환경설정 창 prefab >> 하이어라키 창에 올려두고,
// 띄우는 키 혹은 버튼만 지정하면 어디서든 불러오기 가능!!

// 나중에 음량, 키지정, 해상도 조작 필요
public class SettingPanelScript : MonoBehaviour
{

    public GameObject SettingPanel;

   private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            OnSettingExit();
        }
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
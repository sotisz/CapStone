<<<<<<< Updated upstream
using UnityEngine;

// 환경설정 창 prefab >> 하이어라키 창에 올려두고,
// 띄우는 키 혹은 버튼만 지정하면 어디서든 불러오기 가능!!
=======
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

>>>>>>> Stashed changes

// 나중에 음량, 키지정, 해상도 조작 필요
public class SettingPanelScript : MonoBehaviour
{
<<<<<<< Updated upstream

    public GameObject SettingPanel;

   private void Update()
=======
    public TMP_Dropdown resolutionDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionsNum;

    public GameObject SettingPanel;

    private void Update()
>>>>>>> Stashed changes
    {
        if (Input.GetButtonDown("Cancel"))
        {
            OnSettingExit();
        }
    } 
<<<<<<< Updated upstream
=======

    void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++) 
        {
            if (Screen.resolutions[i].refreshRateRatio == 60)
                resolutions.Add(Screen.resolutions[i]);
        }
        resolutionDropdown.options.Clear();

        int optionNum = 2;
        foreach (Resolution item in resolutions) 
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = item.width + "x" + item.height + " ";
            resolutionDropdown.options.Add(optionData);
            if(item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;
        }

        resolutionDropdown.RefreshShownValue();
    }
    public void DropboxOptionsChange(int x)
    {
        resolutionsNum = x;

    }
    public void Mute()
    {
        SoundManager.Instance.SetAudioMute(EAudioMixerType.BGM);
    }
    public void ChangeVolume(float volume)
    {
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.BGM, volume);
    }
>>>>>>> Stashed changes
    public void OnSettingIn()
    {
        SettingPanel.SetActive(true);
    }
    public void OnSettingExit()
    {
        SettingPanel.SetActive(false);
    }
}
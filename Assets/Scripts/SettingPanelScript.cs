using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


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

    public void Mute()
    {
        SoundManager.Instance.SetAudioMute(EAudioMixerType.BGM);
    }
    public void ChangeVolume(float volume)
    {
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.BGM, volume);
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
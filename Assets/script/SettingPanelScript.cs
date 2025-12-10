
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


// ���߿� ����, Ű����, �ػ� ���� �ʿ�
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

    public void OnMasterVolumeChanged(float volume)
    {
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.Master, volume);
    }

    public void OnBGMVolumeChanged(float volume)
    {
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.BGM, volume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        SoundManager.Instance.SetAudioVolume(EAudioMixerType.SFX, volume);
    }

    //  ���� ��Ʈ �Լ�
    public void ToggleMuteByIndex(int mixerTypeIndex)
    {
        EAudioMixerType type = (EAudioMixerType)mixerTypeIndex;
        SoundManager.Instance.SetAudioMute(type);
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
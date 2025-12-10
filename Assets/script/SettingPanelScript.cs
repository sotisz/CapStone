using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SettingPanelScript : MonoBehaviour
{
    public GameObject SettingPanel;

    // [추가 1] 해상도 UI 연결 변수
    [Header("해상도 설정")]
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private List<Resolution> resolutions = new List<Resolution>(); // 해상도 목록 저장
    private int resolutionNum; // 선택된 해상도 번호

    void Start()
    {
        // 시작 시 해상도 목록 초기화
        InitResolutionUI();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (SettingPanel.activeSelf)
                OnSettingExit();
        }
    }


    // ---------------- [오디오 기능] ----------------
    public void OnMasterVolumeChanged(float volume)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetAudioVolume(EAudioMixerType.Master, volume);
    }

    public void OnBGMVolumeChanged(float volume)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetAudioVolume(EAudioMixerType.BGM, volume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetAudioVolume(EAudioMixerType.SFX, volume);
    }

    public void ToggleMuteByIndex(int mixerTypeIndex)
    {
        if (SoundManager.Instance != null)
        {
            EAudioMixerType type = (EAudioMixerType)mixerTypeIndex;
            SoundManager.Instance.SetAudioMute(type);
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

    // ---------------- [해상도 조절 기능] ----------------

    void InitResolutionUI()
    {
        resolutionDropdown.options.Clear();
        resolutions.Clear();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

        // [핵심 변경] 내가 원하는 해상도 4개를 직접 리스트로 만듭니다.
        // (너비, 높이) 순서입니다.
        Vector2Int[] targetResolutions = new Vector2Int[]
        {
            new Vector2Int(1280, 720),
            new Vector2Int(1600, 900),
            new Vector2Int(1920, 1080),
            new Vector2Int(2560, 1440)
        };

        for (int i = 0; i < targetResolutions.Length; i++)
        {
            // 1. Resolution 구조체에 데이터를 담아 리스트에 추가 (나중에 적용할 때 씀)
            Resolution item = new Resolution();
            item.width = targetResolutions[i].x;
            item.height = targetResolutions[i].y;
            resolutions.Add(item);

            // 2. 드롭다운에 보여줄 텍스트 만들기 (사진처럼 예쁘게!)
            string optionText = item.width + " x " + item.height;

            if (item.width == 1920 && item.height == 1080)
                optionText += " (FULL HD)";
            else if (item.width == 2560 && item.height == 1440)
                optionText += " (QHD)";

            options.Add(optionText);

            // 3. 현재 내 화면과 가장 비슷한 해상도를 기본값으로 선택
            // (너비와 높이가 정확히 일치하면 그 번호를 저장)
            if (item.width == Screen.width && item.height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // 전체화면 토글 초기화
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = Screen.fullScreen;
    }

    // 2. 드롭다운 선택 시 실행 (인스펙터의 On Value Changed에 연결)
    public void OnResolutionChanged(int index)
    {
        resolutionNum = index;
        ApplyResolution();
    }

    // 3. 전체화면 토글 시 실행
    public void OnFullScreenToggle(bool isFull)
    {
        Screen.fullScreen = isFull;
        ApplyResolution(); 
    }

    // 4. 실제 해상도 적용 로직
    void ApplyResolution()
    {
        Resolution targetRes = resolutions[resolutionNum];
        bool isFull = fullscreenToggle.isOn;
        Screen.SetResolution(targetRes.width, targetRes.height, Screen.fullScreen);
        Debug.Log($"해상도 변경: {targetRes.width} x {targetRes.height}, 전체화면: {Screen.fullScreen}");
    }
}
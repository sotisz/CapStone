using UnityEngine;
using UnityEngine.Audio;

public enum EAudioMixerType { Master, BGM, SFX }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Water Sounds")]
    public AudioClip waterEnterSound;

    private AudioSource audioSource;

    private bool[] isMute = new bool[3];
    private float[] audioVolumes = new float[3];

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 오디오 소스 설정
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    // 볼륨 설정 (0.0001 ~ 1)
    public void SetAudioVolume(EAudioMixerType type, float volume)
    {
        audioMixer.SetFloat(type.ToString(), Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    // 뮤트 토글
    public void SetAudioMute(EAudioMixerType audioMixerType)
    {
        int type = (int)audioMixerType;

        if (!isMute[type])
        {
            isMute[type] = true;
            audioMixer.GetFloat(audioMixerType.ToString(), out float curVolume);
            audioVolumes[type] = curVolume;
            SetAudioVolume(audioMixerType, 0.001f);
        }
        else
        {
            isMute[type] = false;
            SetAudioVolume(audioMixerType, audioVolumes[type]);
        }
    }

    // 음소거 토글
    public void ToggleMute(EAudioMixerType type)
    {
        int index = (int)type;

        if (!isMute[index])
        {
            isMute[index] = true;
            audioMixer.GetFloat(type.ToString(), out float currentVolume);
            audioVolumes[index] = currentVolume;
            SetAudioVolume(type, 0.0001f);
        }
        else
        {
            isMute[index] = false;
            SetAudioVolume(type, Mathf.Pow(10, audioVolumes[index] / 20f));
        }
    }

    // 물 진입 사운드
    public void PlayWaterEnterSound()
    {
        if (waterEnterSound != null)
            audioSource.PlayOneShot(waterEnterSound);
    }

}

using UnityEngine;
using UnityEngine.Audio;

public enum EAudioMixerType { Master, BGM, SFX }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Water Sounds")]
    public AudioClip waterEnterSound;

    private AudioSource audioSource;   // SFX 전용
    private AudioSource bgmSource;     // BGM 전용

    private bool[] isMute = new bool[3];
    private float[] audioVolumes = new float[3];

    private void Awake()
    {
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

        // BGM Source 생성
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = bgmGroup;   // BGM Mixer에 강제 연결

        //SFX Source 생성
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxGroup; // SFX Mixer에 강제 연결
    }

    // 볼륨 설정 (Master / BGM / SFX)
    public void SetAudioVolume(EAudioMixerType type, float volume)
    {
        audioMixer.SetFloat(
            type.ToString(),
            Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20
        );
    }

    public void SetAudioMute(EAudioMixerType type)
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

    // SFX 재생 (슬라이더 100% 반응)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    // BGM 재생 (슬라이더 100% 반응)
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    // 물 진입 효과음
    public void PlayWaterEnterSound()
    {
        if (waterEnterSound != null)
            PlaySFX(waterEnterSound);
    }
}

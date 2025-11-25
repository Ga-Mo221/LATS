using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    // --- UI Game ---
    [Header("Button sound")]
    [SerializeField] private AudioSource _ButtonUISound;
    [SerializeField] private AudioClip hoverSound;          // Âm thanh khi hover
    [SerializeField] private AudioClip clickSound;          // Âm thanh khi click

    // --- Logo ---
    [Header("Logo")]
    [SerializeField] private AudioSource _logoAudioSound;
    [SerializeField] private AudioClip _logoClip;

    // --- Background ---
    [Header("Music Game")]
    [SerializeField] private AudioSource _BgMusic;
    [SerializeField] private AudioSource _bgAudioSound_Chim;
    [SerializeField] private AudioSource _bgAudioSound_Ve;
    [SerializeField] private AudioSource _bgAudioSound_La;
    [SerializeField] private AudioSource _bgAudioSound_ConTrung;

    [SerializeField] private AudioClip _musicBGGame;
    [SerializeField] private AudioClip _musicBGMainMenu;
    [SerializeField] private AudioClip _tiengChim;
    [SerializeField] private AudioClip _tiengVe;
    [SerializeField] private AudioClip _tiengConTrung;
    [SerializeField] private AudioClip _tiengLaXaoSac;

    // --- Effect ---
    [SerializeField] private AudioSource _effectAudioSound;

    [Header("Attack")]
    [SerializeField] private AudioClip _attack1;
    [SerializeField] private AudioClip _attack2;
    [SerializeField] private AudioClip _attack3;
    [SerializeField] private AudioClip _attack4;

    [Header("Move")]
    [SerializeField] private AudioClip _walking;

    [Header("Jump")]
    [SerializeField] private AudioClip _jump1;
    [SerializeField] private AudioClip _jump2;
    [SerializeField] private AudioClip _tiepdat;

    [Header("Other FX")]
    [SerializeField] private AudioClip _dash;

    // Volume control
    [SerializeField] private float _allVolume = 1f;
    [SerializeField] private float _bgVolume = 1f;
    [SerializeField] private float _FSXVolume = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // load setting volume
        if (SettingManager.Instance == null) return;
        SetAllVolume(SettingManager.Instance.CurrentSettings.masterVolume);
        SetBGVolume(SettingManager.Instance.CurrentSettings.musicVolume);
        SetFSXVolume(SettingManager.Instance.CurrentSettings.sfxVolume);
    }

    // ================= BACKGROUND =================
    public void PlayBGAudioSound()
    {
        PlayLoop(_BgMusic, _musicBGGame);
        PlayLoop(_bgAudioSound_Chim, _tiengChim);
        PlayLoop(_bgAudioSound_Ve, _tiengVe);
        PlayLoop(_bgAudioSound_ConTrung, _tiengConTrung);
        PlayLoop(_bgAudioSound_La, _tiengLaXaoSac);
    }

    public void PlayMainMenuSound()
    {
        if (_BgMusic.isPlaying) stopBGAudioSound();
        PlayLoop(_BgMusic, _musicBGMainMenu);
    }

    public void stopBGAudioSound()
    {
        _BgMusic.Stop();
        _bgAudioSound_Chim.Stop();
        _bgAudioSound_Ve.Stop();
        _bgAudioSound_ConTrung.Stop();
        _bgAudioSound_La.Stop();
    }

    public void stopMainMenuSound()
    {
        _BgMusic.Stop();
    }

    private void PlayLoop(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null) return;
        source.clip = clip;
        source.loop = true;
        source.volume = _bgVolume * _allVolume;
        source.Play();
    }

    // ================= EFFECT =================
    public void PlayAttackAudio(int type)
    {
        switch (type)
        {
            case 1: PlayFX(_attack1); break;
            case 2: PlayFX(_attack2); break;
            case 3: PlayFX(_attack3); break;
            case 4: PlayFX(_attack4); break;
        }
    }

    public void PlayMoveAudio()
    {
        _effectAudioSound.clip = _walking;
        _effectAudioSound.volume = _FSXVolume * _allVolume;
        if (!_effectAudioSound.isPlaying)
            _effectAudioSound.Play();
    }

    public void PlayJumpAudio(int type)
    {
        switch (type)
        {
            case 1: PlayFX(_jump1); break;
            case 2: PlayFX(_jump2); break;
            case 3: PlayFX(_tiepdat); break;
        }
    }

    public void PlayDashAudio()
    {
        PlayFX(_dash);
    }

    private void PlayFX(AudioClip clip)
    {
        if (clip == null) return;
        _effectAudioSound.PlayOneShot(clip, _FSXVolume * _allVolume);
    }

    // ================= VOLUME =================
    public void SetAllVolume(float value)
    {
        _allVolume = Mathf.Clamp01(value);
        ApplyVolume();
    }

    public void SetBGVolume(float value)
    {
        _bgVolume = Mathf.Clamp01(value);
        ApplyVolume();
    }

    public void SetFSXVolume(float value)
    {
        _FSXVolume = Mathf.Clamp01(value);
        ApplyVolume();
    }

    private void ApplyVolume()
    {
        // BG
        _bgAudioSound_Chim.volume = _bgVolume * _allVolume;
        _bgAudioSound_Ve.volume = _bgVolume * _allVolume;
        _bgAudioSound_ConTrung.volume = _bgVolume * _allVolume;
        _bgAudioSound_La.volume = _bgVolume * _allVolume;

        // FX
        _effectAudioSound.volume = _FSXVolume * _allVolume;
    }

    // ================= EXTRA =================
    public void PauseAll()
    {
        AudioListener.pause = true;
    }

    public void ResumeAll()
    {
        AudioListener.pause = false;
    }

    public void playButtonHover()
    {
        if (hoverSound == null) return;
        _ButtonUISound.PlayOneShot(hoverSound, _FSXVolume * _allVolume);
    }

    public void playButtonPressed()
    {
        if (clickSound == null) return;
        _ButtonUISound.PlayOneShot(clickSound, _FSXVolume * _allVolume);
    }

    public void playLogoSound()
    {
        if (_logoClip == null) return;
        if (_logoAudioSound == null) return;
        _logoAudioSound.PlayOneShot(_logoClip, _bgVolume * _allVolume);
    }
}

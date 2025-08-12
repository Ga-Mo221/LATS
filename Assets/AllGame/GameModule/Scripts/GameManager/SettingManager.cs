using System.IO;
using UnityEngine;
using TMPro;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance { get; private set; }

    public SettingData CurrentSettings = new SettingData();
    public SettingDatatam _settingTam = new SettingDatatam();

    private string settingPath => Path.Combine(Application.persistentDataPath, "settings.json");

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // SetDefaultSettings();
        // SaveSettings();
        LoadSettings();
        CurrentSettingsTosettingtam();
        ApplyScreenMode();
    }


    public void CurrentSettingsTosettingtam()
    {
        _settingTam.language = CurrentSettings.language;
        _settingTam.isFullscreen = CurrentSettings.isFullscreen;
        _settingTam.resolutionWidth = CurrentSettings.resolutionWidth;
        _settingTam.resolutionHeight = CurrentSettings.resolutionHeight;
        _settingTam.targetFPS = CurrentSettings.targetFPS;
        _settingTam.masterVolume = CurrentSettings.masterVolume;
        _settingTam.sfxVolume = CurrentSettings.sfxVolume;
        _settingTam.musicVolume = CurrentSettings.musicVolume;
    }


    private void settingtamToCurrentSettings()
    {
        CurrentSettings.language = _settingTam.language;
        CurrentSettings.isFullscreen = _settingTam.isFullscreen;
        CurrentSettings.resolutionWidth = _settingTam.resolutionWidth;
        CurrentSettings.resolutionHeight = _settingTam.resolutionHeight;
        CurrentSettings.targetFPS = _settingTam.targetFPS;
        CurrentSettings.masterVolume = _settingTam.masterVolume;
        CurrentSettings.sfxVolume = _settingTam.sfxVolume;
        CurrentSettings.musicVolume = _settingTam.musicVolume;
    }


    public void SaveSettings()
    {
        settingtamToCurrentSettings();
        string json = JsonUtility.ToJson(CurrentSettings, true);
        File.WriteAllText(settingPath, json);
        Debug.Log("Settings saved to: " + settingPath);
        //Debug.Log(_settingTam.ToString());
        ApplyScreenMode();
        LoadSettings();
    }


    public void LoadSettings()
    {
        if (File.Exists(settingPath))
        {
            string json = File.ReadAllText(settingPath);
            CurrentSettings = JsonUtility.FromJson<SettingData>(json);
            //Debug.Log(json);
        }
        else
        {
            SetDefaultSettings();
        }
    }


    private void SetDefaultSettings()
    {
        Resolution defaultRes = Screen.currentResolution;
        CurrentSettings = new SettingData
        {
            language = Language.VI,
            isFullscreen = true,
            resolutionWidth = defaultRes.width,
            resolutionHeight = defaultRes.height,
            targetFPS = 60,
            masterVolume = 1f,
            sfxVolume = 1f,
            musicVolume = 1f
        };
    }


    public void ApplyScreenMode()
    {
        if (_settingTam.isFullscreen)
        {
            Screen.SetResolution(_settingTam.resolutionWidth, _settingTam.resolutionHeight, FullScreenMode.FullScreenWindow);
            Debug.Log($"Full Screen: {_settingTam.isFullscreen}\n width: {_settingTam.resolutionWidth} \n Height: {_settingTam.resolutionHeight}");
        }
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
            Debug.Log($"Full Screen: {_settingTam.isFullscreen}\n width: {1280} \n Height: {720}");
        }

        Application.targetFrameRate = _settingTam.targetFPS;
        Debug.Log($"Fps: {_settingTam.targetFPS}");
    }
}

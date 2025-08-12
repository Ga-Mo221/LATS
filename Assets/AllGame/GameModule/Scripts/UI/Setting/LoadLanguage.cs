using UnityEngine;
using TMPro;

public class LoadLanguage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _btSettingMaster;
    [SerializeField] private TextMeshProUGUI _btAudio;
    [SerializeField] private TextMeshProUGUI _language;
    [SerializeField] private TextMeshProUGUI _screenMode;
    [SerializeField] private TextMeshProUGUI _resolution;
    [SerializeField] private TextMeshProUGUI _fps;
    [SerializeField] private TextMeshProUGUI _btExit;
    [SerializeField] private TextMeshProUGUI _btSave;
    [SerializeField] private TextMeshProUGUI _audioVolume;
    [SerializeField] private TextMeshProUGUI _sfxVolume;
    [SerializeField] private TextMeshProUGUI _musicVolume;
    [SerializeField] private TextMeshProUGUI _btFullScreen;
    [SerializeField] private TextMeshProUGUI _btWindown;

    void Start()
    {
        loadLanguage();
    }

    private void loadLanguage()
    {
        if (!debug()) return;
        LocalizationManager loc = new LocalizationManager();
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "Setting");
        _title.text = loc.GetLocalizedValue("Setting_title");
        _btSettingMaster.text = loc.GetLocalizedValue("Setting_bt_settingmaster");
        _btAudio.text = loc.GetLocalizedValue("Setting_bt_audio");
        _language.text = loc.GetLocalizedValue("Setting_language");
        _screenMode.text = loc.GetLocalizedValue("Setting_screenmodel");
        _resolution.text = loc.GetLocalizedValue("Setting_phangiai");
        _fps.text = loc.GetLocalizedValue("Setting_fps");
        _btExit.text = loc.GetLocalizedValue("Setting_bt_exit");
        _btSave.text = loc.GetLocalizedValue("Setting_bt_save");
        _audioVolume.text = loc.GetLocalizedValue("Setting_audiovolume");
        _sfxVolume.text = loc.GetLocalizedValue("Setting_sfxvolume");
        _musicVolume.text = loc.GetLocalizedValue("Setting_musicvolume");
        _btFullScreen.text = loc.GetLocalizedValue("Setting_bt_fullscreen");
        _btWindown.text = loc.GetLocalizedValue("Setting_bt_windown");
    }

    private bool debug()
    {
        if (!_title)
        {
            Debug.LogError("chưa có TextMeshProGUI title");
            return false;
        }
        if (!_btSettingMaster)
        {
            Debug.LogError("chưa có TextMeshProGUI game setting");
            return false;
        }
        if (!_btAudio)
        {
            Debug.LogError("chưa có TextMeshProGUI audio");
            return false;
        }
        if (!_language)
        {
            Debug.LogError("chưa có TextMeshProGUI language");
            return false;
        }
        if (!_screenMode)
        {
            Debug.LogError("chưa có TextMeshProGUI screrenmode");
            return false;
        }
        if (!_resolution)
        {
            Debug.LogError("chưa có TextMeshProGUI resolution");
            return false;
        }
        if (!_fps)
        {
            Debug.LogError("chưa có TextMeshProGUI fps");
            return false;
        }
        if (!_btExit)
        {
            Debug.LogError("chưa có TextMeshProGUI exit");
            return false;
        }
        if (!_btSave)
        {
            Debug.LogError("chưa có TextMeshProGUI save");
            return false;
        }
        if (!_audioVolume)
        {
            Debug.LogError("chưa có TextMeshProGUI audio volume");
            return false;
        }
        if (!_sfxVolume)
        {
            Debug.LogError("chưa có TextMeshProGUI sfx volume");
            return false;
        }
        if (!_musicVolume)
        {
            Debug.LogError("chưa có TextMeshProGUI music volume");
            return false;
        }
        if (!_btFullScreen)
        {
            Debug.LogError("chưa có TextMeshProGUI fullscene");
            return false;
        }
        if (!_btWindown)
        {
            Debug.LogError("chưa có TextMeshProGUI windown");
            return false;
        }
        return true;
    }
}

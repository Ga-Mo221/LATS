using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadDataSetting : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _language;
    [SerializeField] private TextMeshProUGUI _resolution;
    [SerializeField] private TextMeshProUGUI _fps;
    [SerializeField] private Slider _audioVolume;
    [SerializeField] private Slider _sfxVolume;
    [SerializeField] private Slider _musicVolume;


    void Start()
    {
        loadData();
        loadDataSlider();
    }

    void Update()
    {
        loadData();
    }

    private void loadData()
    {
        switch (SettingManager.Instance._settingTam.language)
        {
            case Language.EN:
                _language.text = "English";
                break;
            case Language.VI:
                _language.text = "Tiếng Việt";
                break;
        }

        _resolution.text = $"{SettingManager.Instance._settingTam.resolutionWidth} x {SettingManager.Instance._settingTam.resolutionHeight}";
        _fps.text = $"{SettingManager.Instance._settingTam.targetFPS} FPS";
    }

    private void loadDataSlider()
    {
        _audioVolume.value = SettingManager.Instance._settingTam.masterVolume;
        _sfxVolume.value = SettingManager.Instance._settingTam.sfxVolume;
        _musicVolume.value = SettingManager.Instance._settingTam.musicVolume;
    }
}

using TMPro;
using UnityEngine;

public class GameOptionLoadLanguage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _setting;
    [SerializeField] private TextMeshProUGUI _menu;
    [SerializeField] private TextMeshProUGUI _exit;


    void Start()
    {
        loadLanguage();
    }

    public void loadLanguage()
    {
        if (!debug()) return;
        LocalizationManager loc = new LocalizationManager();
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "gameOption");

        _setting.text = loc.GetLocalizedValue("setting");
        _menu.text = loc.GetLocalizedValue("menu");
        _exit.text = loc.GetLocalizedValue("exit");
    }


    private bool debug()
    {
        if (!_setting)
        {
            Debug.LogError("[GameOptionLoadLanguage] Chưa gán 'text _setting'");
            return false;
        }
        if (!_menu)
        {
            Debug.LogError("[GameOptionLoadLanguage] Chưa gán 'text _menu'");
            return false;
        }
        if (!_exit)
        {
            Debug.LogError("[GameOptionLoadLanguage] Chưa gán 'text _exit'");
            return false;
        }
        return true;
    }
}

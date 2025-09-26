using TMPro;
using UnityEngine;

public class QuitGameLoadLanguage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _content;
    [SerializeField] private TextMeshProUGUI _exit;
    [SerializeField] private TextMeshProUGUI _cancel;


    void Start()
    {
        if (!_content)
            Debug.LogError("[QuitGameLoadLanguage] Chưa gán 'TextMeshProUGUI Content'");
        if (!_exit)
            Debug.LogError("[QuitGameLoadLanguage] Chưa gán 'TextMeshProUGUI Exit'");
        if (!_cancel)
            Debug.LogError("[QuitGameLoadLanguage] Chưa gán 'TextMeshProUGUI Cancel'");

        loadLanguage();
    }


    public void loadLanguage()
    {
        LocalizationManager loc = new LocalizationManager();
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "quitGame");
        _content.text = loc.GetLocalizedValue("content");
        _exit.text = loc.GetLocalizedValue("exit");
        _cancel.text = loc.GetLocalizedValue("cancel");
    }
}

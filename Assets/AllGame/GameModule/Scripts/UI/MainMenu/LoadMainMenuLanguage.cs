using UnityEngine;
using TMPro;

public class LoadMainMenuLanguage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _btContinue;
    [SerializeField] private TextMeshProUGUI _btNewGame;
    [SerializeField] private TextMeshProUGUI _btAbout;
    [SerializeField] private TextMeshProUGUI _btSetting;
    [SerializeField] private TextMeshProUGUI _btExit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadLanguage();
    }

    // Update is called once per frame
    public void loadLanguage()
    {
        LocalizationManager loc = new LocalizationManager();
        //Debug.Log(SettingManager.Instance.CurrentSettings.language.ToString());
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "MainMenu");
        // button continue
        if (_btContinue != null) _btContinue.text = loc.GetLocalizedValue("continue");
        
        // button new game
        if (_btNewGame != null) _btNewGame.text = loc.GetLocalizedValue("newgame");
        
        // button about
        if (_btAbout != null) _btAbout.text = loc.GetLocalizedValue("about");
        
        // button Setting
        if (_btSetting != null) _btSetting.text = loc.GetLocalizedValue("setting");
        
        // button Exit
        if (_btExit != null) _btExit.text = loc.GetLocalizedValue("exit");
    }
}

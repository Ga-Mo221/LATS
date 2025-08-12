using UnityEngine;
using TMPro;
using NaughtyAttributes;
using Unity.VisualScripting;

public class LoadMainMenuLanguage : MonoBehaviour
{
    public bool _isContinue = false;
    [ShowIf(nameof(_isContinue))]
    [SerializeField] private Transform _btContinue;
    public bool _isNewGame = false;
    [ShowIf(nameof(_isNewGame))]
    [SerializeField] private Transform _btNewGame;
    public bool _isAbout = false;
    [ShowIf(nameof(_isAbout))]
    [SerializeField] private Transform _btAbout;
    public bool _isSetting = false;
    [ShowIf(nameof(_isSetting))]
    [SerializeField] private Transform _btSetting;
    public bool _isExit = false;
    [ShowIf(nameof(_isExit))]
    [SerializeField] private Transform _btExit;
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
        if (_isContinue)
        {
            TextMeshProUGUI _conten = _btContinue.Find("Content").GetComponent<TextMeshProUGUI>();
            if (_conten == null)
            {
                Debug.LogError("Không tìm thấy TextMeshProUGUI trong button Continue!");
            }
            else _conten.text = loc.GetLocalizedValue("continue");
        }
        
        // button new game
        if (_isNewGame)
        {
            TextMeshProUGUI _conten = _btNewGame.Find("Content").GetComponent<TextMeshProUGUI>();
            if (_conten == null)
            {
                Debug.LogError("Không tìm thấy TextMeshProUGUI trong button New Game!");
            }
            else _conten.text = loc.GetLocalizedValue("newgame");
        }
        
        // button about
        if (_isAbout)
        {
            TextMeshProUGUI _conten = _btAbout.Find("Content").GetComponent<TextMeshProUGUI>();
            if (_conten == null)
            {
                Debug.LogError("Không tìm thấy TextMeshProUGUI trong button About!");
            }
            else _conten.text = loc.GetLocalizedValue("about");
        }
        
        // button setting
        if (_isSetting)
        {
            TextMeshProUGUI _conten = _btSetting.Find("Content").GetComponent<TextMeshProUGUI>();
            if (_conten == null)
            {
                Debug.LogError("Không tìm thấy TextMeshProUGUI trong button Setting!");
            }
            else _conten.text = loc.GetLocalizedValue("setting");
        }
        
        // button exit
        if (_isExit)
        {
            TextMeshProUGUI _conten = _btExit.Find("Content").GetComponent<TextMeshProUGUI>();
            if (_conten == null)
            {
                Debug.LogError("Không tìm thấy TextMeshProUGUI trong button Exit!");
            }
            else _conten.text = loc.GetLocalizedValue("exit");
        }
    }
}

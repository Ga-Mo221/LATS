using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingController : MonoBehaviour
{
    [SerializeField] private GameObject _settingMaster;
    [SerializeField] private GameObject _settingAudio;
    [SerializeField] private GameObject _tutorial;

    [SerializeField] private GameObject _buttonSettingMaster;
    [SerializeField] private GameObject _buttonAudio;
    [SerializeField] private GameObject _buttonTutorial;

    [SerializeField] private GameObject _fullScreen;
    [SerializeField] private GameObject _windown;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        setActiveGobj(_settingAudio, false);
    }

    void Start()
    {
        openSettingMaster();
        if (SettingManager.Instance._settingTam.isFullscreen)
        {
            selectFullScreen();
        }
        else
        {
            selectWindown();
        }
    }


    #region Open Setting Audio
    public void openSettingAudio()
    {
        setActiveGobj(_settingMaster, false);
        setActiveGobj(_settingAudio, true);
        setActiveGobj(_tutorial, false);
        selectButton(_buttonAudio, true);
        selectButton(_buttonSettingMaster, false);
        selectButton(_buttonTutorial, false);
    }
    #endregion


    #region Open Setting Master
    public void openSettingMaster()
    {
        setActiveGobj(_settingAudio, false);
        setActiveGobj(_settingMaster, true);
        setActiveGobj(_tutorial, false);
        selectButton(_buttonAudio, false);
        selectButton(_buttonSettingMaster, true);
        selectButton(_buttonTutorial, false);
    }
    #endregion


    #region Open Tutorial
    public void openTutorial()
    {
        setActiveGobj(_settingAudio, false);
        setActiveGobj(_settingMaster, false);
        setActiveGobj(_tutorial, true);
        selectButton(_buttonAudio, false);
        selectButton(_buttonSettingMaster, false);
        selectButton(_buttonTutorial, true);
    }
    #endregion


    #region Change Image Button Fullscreen
    public void selectFullScreen()
    {
        SettingManager.Instance._settingTam.isFullscreen = true;
        selectButton(_fullScreen, true);
        selectButton(_windown, false);
        //Debug.Log($"settingtam:{SettingManager.Instance._settingTam.isFullscreen} \ncurensetting:{SettingManager.Instance.CurrentSettings.isFullscreen}");
    }
    #endregion


    #region Change Image Button Windown
    public void selectWindown()
    {
        SettingManager.Instance._settingTam.isFullscreen = false;
        selectButton(_fullScreen, false);
        selectButton(_windown, true);
        //Debug.Log($"settingtam:{SettingManager.Instance._settingTam.isFullscreen} \ncurensetting:{SettingManager.Instance.CurrentSettings.isFullscreen}");
    }
    #endregion


    #region Left Change Language
    public void left_ChangeLanguage()
    {
        Language language = SettingManager.Instance._settingTam.language;

        if (language == Language.VI)
            SettingManager.Instance._settingTam.language = Language.EN;
        if (language == Language.EN)
            SettingManager.Instance._settingTam.language = Language.VI;
    }
    #endregion


    #region Right Change Language
    public void right_ChangeLanguage()
    {
        Language language = SettingManager.Instance._settingTam.language;

        if (language == Language.VI)
            SettingManager.Instance._settingTam.language = Language.EN;
        if (language == Language.EN)
            SettingManager.Instance._settingTam.language = Language.VI;
    }
    #endregion


    #region Left Change Fps
    public void left_ChangeFPS()
    {
        int _fps = SettingManager.Instance._settingTam.targetFPS;
        if (_fps == 60) SettingManager.Instance._settingTam.targetFPS = 144;
        else if (_fps == 120) SettingManager.Instance._settingTam.targetFPS = 60;
        else if (_fps == 144) SettingManager.Instance._settingTam.targetFPS = 120;
    }
    #endregion


    #region Right Change Fps
    public void right_ChangeFPS()
    {
        int _fps = SettingManager.Instance._settingTam.targetFPS;
        if (_fps == 60) SettingManager.Instance._settingTam.targetFPS = 120;
        else if (_fps == 120) SettingManager.Instance._settingTam.targetFPS = 144;
        else if (_fps == 144) SettingManager.Instance._settingTam.targetFPS = 60;
    }
    #endregion


    #region Left Change Resolution
    public void left_ChangeResolution()
    {
        int _width = SettingManager.Instance._settingTam.resolutionWidth;
        int _height = SettingManager.Instance._settingTam.resolutionHeight;

        if (_width == 1280 && _height == 720)
        {
            SettingManager.Instance._settingTam.resolutionWidth = 1920;
            SettingManager.Instance._settingTam.resolutionHeight = 1080;
        }
        else if (_width == 1920 && _height == 1080)
        {
            SettingManager.Instance._settingTam.resolutionWidth = 2560;
            SettingManager.Instance._settingTam.resolutionHeight = 1440;
        }
        else if (_width == 2560 && _height == 1440)
        {
            SettingManager.Instance._settingTam.resolutionWidth = 1280;
            SettingManager.Instance._settingTam.resolutionHeight = 720;
        }
    }
    #endregion


    #region  Right Change Resolution
    public void right_ChangeResolution()
    {
        int _width = SettingManager.Instance._settingTam.resolutionWidth;
        int _height = SettingManager.Instance._settingTam.resolutionHeight;

        if (_width == 2560 && _height == 1440)
        {
            SettingManager.Instance._settingTam.resolutionWidth = 1920;
            SettingManager.Instance._settingTam.resolutionHeight = 1080;
        }
        else if (_width == 1920 && _height == 1080)
        {
            SettingManager.Instance._settingTam.resolutionWidth = 1280;
            SettingManager.Instance._settingTam.resolutionHeight = 720;
        }
        else if (_width == 1280 && _height == 720)
        {
            SettingManager.Instance._settingTam.resolutionWidth = 2560;
            SettingManager.Instance._settingTam.resolutionHeight = 1440;
        }
    }
    #endregion


    #region Change Audio Volume
    public void changeAudioVolume(float value)
    {
        SettingManager.Instance._settingTam.masterVolume = value;
    }
    #endregion


    #region Change SFX Volume
    public void changeSFXVolume(float value)
    {
        SettingManager.Instance._settingTam.sfxVolume = value;
    }
    #endregion


    #region Change Music Volume
    public void changeMusicVolume(float value)
    {
        SettingManager.Instance._settingTam.musicVolume = value;
    }
    #endregion


    #region Close Setting
    public Action onClose;
    public void closeGobj()
    {
        GameManager.Instance._canOpenWindown = true;
        SettingManager.Instance.CurrentSettingsTosettingtam();
        onClose?.Invoke();
        gameObject.SetActive(false);
    }
    #endregion


    #region Save Setting
    public void saveSetting()
    {
        GameManager.Instance._canOpenWindown = true;
        SettingManager.Instance.SaveSettings();
        onClose?.Invoke();
        gameObject.SetActive(false);
    }
    #endregion


    #region Set Active Panel
    private void setActiveGobj(GameObject obj, bool ok)
    {
        if (!ok)
        {
            obj.GetComponent<CanvasGroup>().alpha = 0;
            obj.GetComponent<CanvasGroup>().interactable = false;
            obj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        else
        {
            obj.GetComponent<CanvasGroup>().alpha = 1;
            obj.GetComponent<CanvasGroup>().interactable = true;
            obj.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
    #endregion


    #region Change Image Button Select
    private void selectButton(GameObject _bt, bool ok)
    {
        var _content = _bt.transform.Find("Content");
        var _text = _content.GetComponent<TextMeshProUGUI>();
        var _img = _bt.GetComponent<Image>();
        var _button = _bt.GetComponent<Button>();
        var _imgPressed = _button.spriteState.pressedSprite;
        var _imgDis = _button.spriteState.disabledSprite;
        if (!_imgPressed) Debug.LogError("khong co _imgPressed");
        if (!_imgDis) Debug.LogError("khong co _imgDis");
        if (ok)
        {
            _img.sprite = _imgPressed;
            _text.color = Color.black;
        }
        else
        {
            _img.sprite = _imgDis;
            _text.color = Color.white;
        }
    }
    #endregion
}

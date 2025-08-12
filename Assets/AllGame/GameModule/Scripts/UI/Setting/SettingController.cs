using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using TMPro;

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

    public void openSettingAudio()
    {
        setActiveGobj(_settingMaster, false);
        setActiveGobj(_settingAudio, true);
        setActiveGobj(_tutorial, false);
        selectButton(_buttonAudio, true);
        selectButton(_buttonSettingMaster, false);
        selectButton(_buttonTutorial, false);
    }


    public void openSettingMaster()
    {
        setActiveGobj(_settingAudio, false);
        setActiveGobj(_settingMaster, true);
        setActiveGobj(_tutorial, false);
        selectButton(_buttonAudio, false);
        selectButton(_buttonSettingMaster, true);
        selectButton(_buttonTutorial, false);
    }

    public void openTutorial()
    {
        setActiveGobj(_settingAudio, false);
        setActiveGobj(_settingMaster, false);
        setActiveGobj(_tutorial, true);
        selectButton(_buttonAudio, false);
        selectButton(_buttonSettingMaster, false);
        selectButton(_buttonTutorial, true);
    }


    public void selectFullScreen()
    {
        SettingManager.Instance._settingTam.isFullscreen = true;
        selectButton(_fullScreen, true);
        selectButton(_windown, false);
        //Debug.Log($"settingtam:{SettingManager.Instance._settingTam.isFullscreen} \ncurensetting:{SettingManager.Instance.CurrentSettings.isFullscreen}");
    }


    public void selectWindown()
    {
        SettingManager.Instance._settingTam.isFullscreen = false;
        selectButton(_fullScreen, false);
        selectButton(_windown, true);
        //Debug.Log($"settingtam:{SettingManager.Instance._settingTam.isFullscreen} \ncurensetting:{SettingManager.Instance.CurrentSettings.isFullscreen}");
    }


    public void left_ChangeLanguage()
    {
        Language language = SettingManager.Instance._settingTam.language;

        if (language == Language.VI)
            SettingManager.Instance._settingTam.language = Language.EN;
        if (language == Language.EN)
            SettingManager.Instance._settingTam.language = Language.VI;
    }


    public void right_ChangeLanguage()
    {
        Language language = SettingManager.Instance._settingTam.language;

        if (language == Language.VI)
            SettingManager.Instance._settingTam.language = Language.EN;
        if (language == Language.EN)
            SettingManager.Instance._settingTam.language = Language.VI;
    }


    public void left_ChangeFPS()
    {
        int _fps = SettingManager.Instance._settingTam.targetFPS;
        if (_fps == 60) SettingManager.Instance._settingTam.targetFPS = 144;
        else if (_fps == 120) SettingManager.Instance._settingTam.targetFPS = 60;
        else if (_fps == 144) SettingManager.Instance._settingTam.targetFPS = 120;
    }


    public void right_ChangeFPS()
    {
        int _fps = SettingManager.Instance._settingTam.targetFPS;
        if (_fps == 60) SettingManager.Instance._settingTam.targetFPS = 120;
        else if (_fps == 120) SettingManager.Instance._settingTam.targetFPS = 144;
        else if (_fps == 144) SettingManager.Instance._settingTam.targetFPS = 60;
    }


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


    public void changeAudioVolume(float value)
    {
        SettingManager.Instance._settingTam.masterVolume = value;
    }


    public void changeSFXVolume(float value)
    {
        SettingManager.Instance._settingTam.sfxVolume = value;
    }


    public void changeMusicVolume(float value)
    {
        SettingManager.Instance._settingTam.musicVolume = value;
    }


    public Action onClose;
    public void closeGobj()
    {
        SettingManager.Instance.CurrentSettingsTosettingtam();
        onClose?.Invoke();
        Destroy(gameObject);
    }


    public void saveSetting()
    {
        SettingManager.Instance.SaveSettings();
        onClose?.Invoke();
        Destroy(gameObject);
    }


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
}

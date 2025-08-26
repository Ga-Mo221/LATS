using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuButtons;
    private LoadMainMenuLanguage _loadLanguage;

    void Awake()
    {
        if (!_mainMenuButtons)
            Debug.LogError("[MenuController] Chưa lấy được Button");
        _loadLanguage = GetComponent<LoadMainMenuLanguage>();
        if (!_loadLanguage)
            Debug.LogError("[MenuController] Chua lay duoc LoadMainMenuLanguage");
    }

    public void newGame()
    {
        GameManager.Instance.newGame();
    }

    public void openSetting()
    {
        _mainMenuButtons.SetActive(false);
        GameManager.Instance._loadlanguageSetting.loadLanguage();
        GameManager.Instance._getMainCameraSetting.setupCamera();
        GameManager.Instance._setting.SetActive(true);
        GameManager.Instance._canOpenWindown = false;
        SettingController _settingClose = GameManager.Instance._setting.GetComponent<SettingController>();
        _settingClose.onClose += () =>
        {
            if (SceneManager.GetActiveScene().name == "GameMenu")
            {
                if (!_mainMenuButtons)
                {
                    Debug.LogError("[MenuController] Chưa lấy được Button");
                    return;
                }
                _mainMenuButtons.SetActive(true); // Bật lại nút menu khi đóng
                _loadLanguage.loadLanguage();
            }
        }
            ;
    }

    public void exitGame()
    {
        GameManager.Instance.exitGame();
    }
}

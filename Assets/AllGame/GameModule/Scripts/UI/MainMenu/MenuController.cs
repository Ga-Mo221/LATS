using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using System;

public class MenuController : MonoBehaviour
{
    private GameObject _mainMenuButtons;
    private LoadMainMenuLanguage _loadLanguage;

    void Awake()
    {
        _mainMenuButtons = transform.Find("Buttons").gameObject;
        if (!_mainMenuButtons)
            Debug.LogError("Chưa lấy được Button");
        _loadLanguage = GetComponent<LoadMainMenuLanguage>();
        if (!_loadLanguage)
            Debug.LogError("Chua lay duoc LoadMainMenuLanguage");
    }

    public void newGame()
    {
        GameManager.Instance.newGame();
    }

    public void openSetting()
    {
        _mainMenuButtons.SetActive(false);
        var _settingClose = GameManager.Instance.openSetting();
        _settingClose.onClose += () =>
        {
            _mainMenuButtons.SetActive(true); // Bật lại nút menu khi đóng
            _loadLanguage.loadLanguage();
        };
    }

    public void exitGame()
    {
        GameManager.Instance.exitGame();
    }
}

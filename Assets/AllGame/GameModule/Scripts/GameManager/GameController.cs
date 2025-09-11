using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        escape();
        tapClick();
    }

    private void escape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "GameMenu")
            {
                escapeGameMenu();
            }
            else
            {
                escapeClikOne();
            }
        }
    }

    private void escapeGameMenu()
    {
        bool _isOpen = GameManager.Instance._quitGame.activeSelf;
        if (!GameManager.Instance._quitGame.activeSelf && GameManager.Instance._canOpenWindown)
        {
            if (_isOpen) return;
            GameManager.Instance._canOpenWindown = false;
            GameManager.Instance._quitGame.SetActive(true);
            GameManager.Instance._quiGameController.loadLanguage();
        }
        else
        {
            if (!_isOpen) return;
            GameManager.Instance._canOpenWindown = true;
            GameManager.Instance._quitGame.SetActive(false);
        }
    }

    private void escapeClikOne()
    {
        bool _isOpen = GameManager.Instance._gameOptions.activeSelf;
        if (GameManager.Instance._canOpenWindown)
        {
            if (_isOpen) return;
            GameManager.Instance._gameOptionController.loadLanguage();
            GameManager.Instance._canOpenWindown = false;
            GameManager.Instance._gameOptions.SetActive(true);
        }
        else
        {
            if (offAllWindown()) return;
            if (!_isOpen) return;
            GameManager.Instance._canOpenWindown = true;
            GameManager.Instance._gameOptions.SetActive(false);
        }
    }


    private bool offAllWindown()
    {
        // setting
        bool _isOpenSetting = GameManager.Instance._setting.activeSelf;
        if (_isOpenSetting)
        {
            GameManager.Instance._settingController.closeGobj();
            return true;
        }

        // inventory
        bool _isOpenInventory = InventoryManager.Instance._isOpen;
        if (_isOpenInventory)
        {
            InventoryManager.Instance.setActive(!_isOpenInventory);
            GameManager.Instance._canOpenWindown = _isOpenInventory;
            InventoryManager.Instance._itemStats_Overlay.gameObject.SetActive(false);
            InventoryManager.Instance._contextMenu_Overlay.gameObject.SetActive(false);
            return true;
        }
        return false;
    }


    private void tapClick()
    {
        if (SceneManager.GetActiveScene().name == "GameMenu")
            return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool _isOpen = InventoryManager.Instance._isOpen;
            if (GameManager.Instance._canOpenWindown)
            {
                if (_isOpen) return;
                InventoryManager.Instance.setActive(!_isOpen);
                GameManager.Instance._canOpenWindown = _isOpen;
                InventoryManager.Instance._itemStats_Overlay.gameObject.SetActive(false);
                InventoryManager.Instance._contextMenu_Overlay.gameObject.SetActive(false);
            }
            else
            {
                if (!_isOpen) return;
                InventoryManager.Instance.setActive(!_isOpen);
                GameManager.Instance._canOpenWindown = _isOpen;
                InventoryManager.Instance._itemStats_Overlay.gameObject.SetActive(false);
                InventoryManager.Instance._contextMenu_Overlay.gameObject.SetActive(false);
            }
        }
    }
}

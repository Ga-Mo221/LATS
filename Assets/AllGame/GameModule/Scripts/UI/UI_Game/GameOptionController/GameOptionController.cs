using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOptionController : MonoBehaviour
{
    [SerializeField] private GameObject _buttons;
    [SerializeField] private GameOptionLoadLanguage _loadLanguage;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        if (!_buttons)
            Debug.LogError("[GameOptionController] Chưa có 'Buttons Group GameObject'");

        if (!_loadLanguage)
            Debug.LogError("[GameOptionController] Chưa có 'GameOptionLoadLanguage'");
    }

    public void openSetting()
    {
        GameManager.Instance._canOpenWindown = false;
        GameManager.Instance._loadlanguageSetting.loadLanguage();
        GameManager.Instance._setting.SetActive(true);
        gameObject.SetActive(false);
    }


    public void openMenu()
    {
        GameManager.Instance._canOpenWindown = true;
        PlayerManager.Instance.destroyPlayer();
        SceneManager.LoadScene("GameMenu");
        gameObject.SetActive(false);
    }

    public void exitOption()
    {
        GameManager.Instance._canOpenWindown = true;
        gameObject.SetActive(false);
    }


    public void loadLanguage()
        => _loadLanguage.loadLanguage();
}

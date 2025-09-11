using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singer Ton
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion


    #region value
    public bool _canOpenWindown = true;
    // setting
    public GameObject _setting { get; set; }
    public LoadLanguage _loadlanguageSetting { get; set; }
    public SettingController _settingController { get; set; }

    // game option
    public GameObject _gameOptions { get; set; }
    public GameOptionController _gameOptionController { get; set; }

    // game over
    public GameObject _gameOver{ get; set; }

    // quit Game
    public GameObject _quitGame { get; set; }
    public QuitGameController _quiGameController { get; set; }
    #endregion


    void Start()
    {
        if (SceneManager.GetActiveScene().name == "GameMenu")
        {
            Instantiate(GameModule.Instance._IntroPrefab);
        }
        createGameUI();
    }

    void Update()
    {
        bool _isOpenGameOver = _gameOver.activeSelf;
        if (_isOpenGameOver)
        {
            _setting.SetActive(false);
            _gameOptions.SetActive(false);
            InventoryManager.Instance.setActive(false);
        }
    }


    #region Create UI
    private void createGameUI()
    {
        _setting = Instantiate(GameModule.Instance._settingPrefab, transform.position, Quaternion.identity);
        _settingController = _setting.GetComponent<SettingController>();
        _loadlanguageSetting = _setting.GetComponent<LoadLanguage>();
        Debug.Log("[GameManager] Đã khởi tạo 'UI-Setting'");
        _setting.SetActive(false);

        _gameOptions = Instantiate(GameModule.Instance._gameOptionPrefab, transform.position, Quaternion.identity);
        _gameOptionController = _gameOptions.GetComponent<GameOptionController>();
        Debug.Log("[GameManager] Đã khởi tạo 'UI-GameOption'");
        _gameOptions.SetActive(false);

        _gameOver = Instantiate(GameModule.Instance._gameOverPrefab, transform.position, Quaternion.identity);
        Debug.Log("[GameManager] Đã khởi tạo 'UI-GameOver'");
        _gameOver.SetActive(false);

        _quitGame = Instantiate(GameModule.Instance._quitGameUIPrefab, transform.position, Quaternion.identity);
        _quiGameController = _quitGame.GetComponent<QuitGameController>();
        Debug.Log("[GameManager] Đã khởi tạo 'UI-QuitGame'");
        _quitGame.SetActive(false);

        Instantiate(GameModule.Instance._InventoryPrefab, transform.position, Quaternion.identity);
        Debug.Log("[GameManager] Đã khởi tạo 'UI-Inventory'");
    }
    #endregion


    #region New Game
    public void newGame()
    {
        SceneManager.LoadScene(2);
    }
    #endregion


    #region Continue
    public void loadGame()
    {
    }
    #endregion


    #region Quit Game
    public void exitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion
}

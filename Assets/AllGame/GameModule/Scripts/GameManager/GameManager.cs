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


    void Start()
    {
        createGameUI();
    }


    #region value
    public bool _canOpenWindown = true;
    // setting
    public GameObject _setting { get; set; }
    public GetMainCam _getMainCameraSetting { get; set; }
    public LoadLanguage _loadlanguageSetting { get; set; }
    private SettingController _settingController { get; set; }

    // game option
    public GameObject _gameOptions { get; set; }
    public GetMainCam _getMainCameraGameOption { get; set; }
    private GameOptionController _gameOptionController;

    // game over
    public GameObject _gameOver{ get; set; }
    #endregion


    void Update()
    {
        openSettingController();

        bool _isOpenGameOver = _gameOver.activeSelf;
        if (_isOpenGameOver)
        {
            _setting.SetActive(false);
            _gameOptions.SetActive(false);
        }
    }


    #region Create UI
    private void createGameUI()
    {
        _setting = Instantiate(GameModule.Instance._settingPrefab, transform.position, Quaternion.identity);
        _settingController = _setting.GetComponent<SettingController>();
        _getMainCameraSetting = _setting.GetComponent<GetMainCam>();
        _loadlanguageSetting = _setting.GetComponent<LoadLanguage>();
        Debug.Log("[GameManager] Đã khởi tạo 'UI-Setting'");
        _setting.SetActive(false);

        _gameOptions = Instantiate(GameModule.Instance._gameOptionPrefab, transform.position, Quaternion.identity);
        _getMainCameraGameOption = _gameOptions.GetComponent<GetMainCam>();
        _gameOptionController = _gameOptions.GetComponent<GameOptionController>();
        Debug.Log("[GameManager] Đã khởi tạo 'UI-GameOption'");
        _gameOptions.SetActive(false);


        _gameOver = Instantiate(GameModule.Instance._gameOverPrefab, transform.position, Quaternion.identity);
        Debug.Log("[GameManager] Đã khởi tạo 'UI-GameOver'");
        _gameOver.SetActive(false);
    }
    #endregion


    #region Open Setting
    private void openSettingController()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "GameMenu")
            {
                bool _isOpen = _setting.activeSelf;
                if (_canOpenWindown)
                {
                    _canOpenWindown = false;
                    _loadlanguageSetting.loadLanguage();
                    _getMainCameraSetting.setupCamera();
                    _setting.SetActive(!_isOpen);
                }
                else
                {
                    _canOpenWindown = true;
                    _settingController.closeGobj();
                }
            }
            else
            {
                bool _isOpen = _gameOptions.activeSelf;
                if (_canOpenWindown)
                {
                    _gameOptionController.loadLanguage();
                    _canOpenWindown = false;
                    _getMainCameraGameOption.setupCamera();
                }
                else
                {
                    bool _isOpenSetting = _setting.activeSelf;
                    if (_isOpenSetting)
                    {
                        _settingController.closeGobj();
                        return;
                    }
                    _canOpenWindown = true;
                }
                _gameOptions.SetActive(!_isOpen);
            }
        }
    }
    #endregion


    #region New Game
    public void newGame()
    {
        SceneManager.LoadScene(1);
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

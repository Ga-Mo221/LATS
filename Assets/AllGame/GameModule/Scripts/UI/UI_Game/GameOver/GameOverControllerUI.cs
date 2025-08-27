using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverControllerUI : MonoBehaviour
{
    [SerializeField] private GameOverLoadLanguage _loadLanguage;
    [SerializeField] private Image _panelImg;
    [SerializeField] private TextMeshProUGUI _textGameOver;
    [SerializeField] private GameObject _buttons;

    private float _startAphal = 0;
    private float _timeElapsed = 0f;
    private float _timeToFace = 2f;
    private bool _isOpen = true;
    private bool _start = true;
    private bool _resetcolor = true;

    private bool _openMenu = false;
    private bool _continue = false;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        if (!_loadLanguage)
            Debug.LogError("[GameOverControllerUI] Chưa gán 'GameOverLoadLanguage'");

        if (!_panelImg)
            Debug.LogError("[GameOverControllerUI] Chưa gán '_panelImg'");

        if (!_textGameOver)
            Debug.LogError("[GameOverControllerUI] Chưa gán '_textGameOver'");

        if (!_buttons)
            Debug.LogError("[GameOverControllerUI] Chưa gán '_buttons'");
    }


    void Update()
    {
        if (_isOpen)
        {
            if (_start)
            {
                openGameOverUI();
            }
        }
        else
        {
            if (_start)
            {
                closeGameOverUI();
            }
        }
    }


    private void openGameOverUI()
    {
        GameManager.Instance._canOpenWindown = false;
        if (_resetcolor)
        {
            _resetcolor = false;
            _loadLanguage.loadLanguage();
            _buttons.SetActive(false);
            _panelImg.color = new Color(_panelImg.color.r, _panelImg.color.g, _panelImg.color.b, _startAphal);
            _textGameOver.color = new Color(_textGameOver.color.r, _textGameOver.color.g, _textGameOver.color.b, _startAphal);
        }
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed < _timeToFace)
        {
            float _faceAlpha = Mathf.Clamp01(_timeElapsed / _timeToFace); // từ 0 -> 1
            _panelImg.color = new Color(_panelImg.color.r, _panelImg.color.g, _panelImg.color.b, _faceAlpha);
            _textGameOver.color = new Color(_textGameOver.color.r, _textGameOver.color.g, _textGameOver.color.b, _faceAlpha);
        }
        else
        {
            _buttons.SetActive(true);
            _start = false;
        }
    }


    private void closeGameOverUI()
    {
        _buttons.SetActive(false);
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed < _timeToFace)
        {
            float _faceAlpha = 1f - Mathf.Clamp01(_timeElapsed / _timeToFace); // từ 1 -> 0
            _textGameOver.color = new Color(_textGameOver.color.r, _textGameOver.color.g, _textGameOver.color.b, _faceAlpha);
        }
        else
        {
            _start = false;
            _isOpen = true;
            if (_continue)
            {
                continueGame2();
            }
            if (_openMenu)
            {
                openMenu2();
            }
        }
    }


    public void openMenu()
    {
        _timeElapsed = 0f;
        _isOpen = false;
        _start = true;
        _resetcolor = true;
        _openMenu = true;
    }


    private void openMenu2()
    {
        GameManager.Instance._canOpenWindown = true;
        PlayerManager.Instance.destroyPlayer();
        SceneManager.LoadScene("GameMenu");
        _start = true;
        _timeElapsed = 0f;
        _openMenu = false;
        GameManager.Instance._canOpenWindown = true;
        gameObject.SetActive(false);
    }


    public void continueGame()
    {
        _timeElapsed = 0f;
        _isOpen = false;
        _start = true;
        _resetcolor = true;
        _continue = true;
    }


    private void continueGame2()
    {
        PlayerManager.Instance.loadPlayerStart();

        _continue = false;
        _start = true;
        _timeElapsed = 0f;
        GameManager.Instance._canOpenWindown = true;
        gameObject.SetActive(false);
    }
}

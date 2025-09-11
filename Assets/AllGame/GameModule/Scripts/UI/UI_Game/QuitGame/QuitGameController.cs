using UnityEngine;

public class QuitGameController : MonoBehaviour
{
    [SerializeField] private QuitGameLoadLanguage _loadLanguage;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!_loadLanguage)
            Debug.LogError("[QuitGameController] Chưa gán 'QuitGameLoadLanguage'");
    }

    public void quitGame()
    {
        GameManager.Instance.exitGame();
    }

    public void loadLanguage()
    {
        _loadLanguage.loadLanguage();
    }

    public void cancel()
    {
        GameManager.Instance._canOpenWindown = true;
    }
}

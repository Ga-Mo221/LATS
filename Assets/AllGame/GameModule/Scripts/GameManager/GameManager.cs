using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    public void newGame()
    {
        SceneManager.LoadScene(1);
    }

    public void loadGame()
    {

    }

    public SettingController openSetting()
    {
        GameObject _settingprefab = GameModule.Instance._settingPrefab;
        if (_settingprefab == null)
        {
            Debug.LogError("Chưa có setting prefab được gán!");
            return null;
        }
        GameObject _setting = Instantiate(_settingprefab, transform.position, Quaternion.identity);
        var _settingclose = _setting.GetComponent<SettingController>();
        return _settingclose;
    }

    public void exitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

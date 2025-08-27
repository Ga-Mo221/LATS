using UnityEngine;

public class GameModule : MonoBehaviour
{
    public static GameModule Instance { get; private set; }

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

    [Header("Prefab")]
    [SerializeField] public GameObject _settingPrefab;
    [SerializeField] public GameObject _playerPrefab;
    [SerializeField] public GameObject _damageTextPrefab;
    [SerializeField] public GameObject _gameOptionPrefab;
    [SerializeField] public GameObject _gameOverPrefab;
    [SerializeField] public GameObject _IntroPrefab;
}

using UnityEngine;

public class GameModule : MonoBehaviour
{
    public static GameModule Instance { get; private set; }

    [Header("Prefab")]
    [SerializeField] public GameObject _settingPrefab;
    [SerializeField] public GameObject _playerPrefab;
    [SerializeField] public GameObject _damageTextPrefab;
    [SerializeField] public GameObject _gameOptionPrefab;
    [SerializeField] public GameObject _gameOverPrefab;
    [SerializeField] public GameObject _IntroPrefab;
    [SerializeField] public GameObject _ItemPrefab;
    [SerializeField] public GameObject _InventoryPrefab;
    [SerializeField] public GameObject _contextMenuPrefab;
    [SerializeField] public GameObject _itemStatPrefab;
    [SerializeField] public GameObject _quitGameUIPrefab;


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

    void Start()
    {
        if (!_settingPrefab)
            Debug.LogError("[GameModule] Chưa gán 'SettingPrefab'");
        if (!_playerPrefab)
            Debug.LogError("[GameModule] Chưa gán 'PlayerPrefab'");
        if (!_damageTextPrefab)
            Debug.LogError("[GameModule] Chưa gán 'DamageTextPrefab'");
        if (!_gameOptionPrefab)
            Debug.LogError("[GameModule] Chưa gán 'GameOptionPrefab'");
        if (!_gameOverPrefab)
            Debug.LogError("[GameModule] Chưa gán 'GameOverPrefab'");
        if (!_IntroPrefab)
            Debug.LogError("[GameModule] Chưa gán 'IntroPrefab'");
        if (!_ItemPrefab)
            Debug.LogError("[GameModule] Chưa gán 'ItemPrefab'");
        if (!_InventoryPrefab)
            Debug.LogError("[GameModule] Chưa gán 'InventoryPrefab'");
        if (!_contextMenuPrefab)
            Debug.LogError("[GameModule] Chưa gán 'ContextMenuPrefab'");
        if (!_itemStatPrefab)
            Debug.LogError("[GameModule] Chưa gán 'ItemStatPrefab'");
        if (!_quitGameUIPrefab)
            Debug.LogError("[GameModule] Chưa gán 'QuitGameUIPrefab'");
    }

}

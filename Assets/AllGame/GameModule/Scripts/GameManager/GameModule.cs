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

    // Add methods to manage game state, such as starting a new game, loading a game, etc.
    [SerializeField] public GameObject _settingPrefab;
}

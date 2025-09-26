using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStats : MonoBehaviour
{
    // slider
    [SerializeField] private Image _health;
    [SerializeField] private Image _mana;
    [SerializeField] private Slider _stamina;
    [SerializeField] private Animator _lifecountAnim;

    // text
    [SerializeField] private TextMeshProUGUI _fps;
    private float _deltaTime = 0.0f;


    void Start()
    {
        if (!_health)
            Debug.LogError("[UIPlayerState] Chưa gán 'Image _health'");
        if (!_mana)
            Debug.LogError("[UIPlayerState] Chưa gán 'Image _mana'");
        if (!_stamina)
            Debug.LogError("[UIPlayerState] Chưa gán 'Slider _stamina'");
        if (!_fps)
            Debug.LogError("[UIPlayerState] Chưa gán 'TextMeshProUGUI _fps'");
        if (!_lifecountAnim)
            Debug.LogError("[UIPlayerState] Chưa gán 'Animator _lifecountAnim");
    }


    void Update()
    {
        updateFps();

        updatePlayerStats();
    }


    private void updatePlayerStats()
    {
        _stamina.value = PlayerManager.Instance.getStamina() / PlayerManager.Instance._start._stamina;
        float _hea = PlayerManager.Instance._start._currentHealth;
        float _maxHealth = PlayerManager.Instance._start._maxHealth;
        float _ma = PlayerManager.Instance._start._currentMana;
        float _maxMana = PlayerManager.Instance._start._maxMana;
        _health.fillAmount = _hea / _maxHealth;
        _mana.fillAmount = _ma / _maxMana;

        int _lifeCount = PlayerManager.Instance._start._currentLifeCount;
        _lifecountAnim.SetInteger(AnimationString._lifecount, _lifeCount);
    }

    private void updateFps()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        _fps.text = Mathf.Ceil(fps).ToString() + " FPS";
    }
}

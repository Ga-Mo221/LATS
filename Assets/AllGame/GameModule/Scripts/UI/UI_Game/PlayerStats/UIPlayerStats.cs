using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStats : MonoBehaviour
{
    // slider
    [SerializeField] private Slider _health;
    [SerializeField] private Slider _mana;
    [SerializeField] private Slider _stamina;

    // text
    [SerializeField] private TextMeshProUGUI _fps;
    private float _deltaTime = 0.0f;


    void Start()
    {
        if (!_health)
            Debug.LogError("[UIPlayerState] Chưa gán 'Slider _health'");
        if (!_mana)
            Debug.LogError("[UIPlayerState] Chưa gán 'Slider _mana'");
        if (!_stamina)
            Debug.LogError("[UIPlayerState] Chưa gán 'Slider _stamina'");
        if (!_fps)
            Debug.LogError("[UIPlayerState] Chưa gán 'TextMeshProUGUI _fps'");
    }


    void Update()
    {
        updateFps();
        setValueSlider();

        updatePlayerStats();
    }


    private void updatePlayerStats()
    {
        _health.value = PlayerManager.Instance._start._currentHealth;
        _mana.value = PlayerManager.Instance._start._currentMana;
        _stamina.value = PlayerManager.Instance.getStamina();
    }


    private void setValueSlider()
    {
        _health.maxValue = PlayerManager.Instance._start._maxHealth;
        _mana.maxValue = PlayerManager.Instance._start._maxMana;
        _stamina.maxValue = PlayerManager.Instance._start._stamina;
    }


    private void updateFps()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        _fps.text = Mathf.Ceil(fps).ToString() + " FPS";
    }
}

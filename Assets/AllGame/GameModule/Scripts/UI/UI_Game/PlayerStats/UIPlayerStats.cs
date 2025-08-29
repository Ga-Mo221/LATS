using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStats : MonoBehaviour
{
    [SerializeField] private Slider _health;
    [SerializeField] private Slider _mana;
    [SerializeField] private Slider _stamina;


    void Start()
    {
        if (!_health)
            Debug.LogError("[UIPlayerState] Chưa gán 'Slider _health'");
        if (!_mana)
            Debug.LogError("[UIPlayerState] Chưa gán 'Slider _mana'");
        if (!_stamina)
            Debug.LogError("[UIPlayerState] Chưa gán 'Slider _stamina'");
    }


    void Update()
    {
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
}

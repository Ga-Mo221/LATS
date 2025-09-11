using TMPro;
using UnityEngine;

public class LoadPlayerStatsToText : MonoBehaviour
{
    private PlayerStarts _stat;
    [SerializeField] private TextMeshProUGUI _text;

    void Start()
    {
        if (!_text)
            Debug.LogError("[LoadPlayerStatsToText] Chưa gán 'TextMeshProUGUI'");
        _stat = PlayerManager.Instance._start;

        display();
    }

    public void display()
    {
        LocalizationManager loc = new LocalizationManager();
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "playerStats");
        if (_stat == null) return;
        _text.text = loc.GetLocalizedValue("level") + ": " + _stat._level + "\n";
        _text.text += loc.GetLocalizedValue("mana") + ": " + _stat._maxMana + "\n";
        _text.text += loc.GetLocalizedValue("health") + ": " + _stat._maxHealth + "\n";
        _text.text += loc.GetLocalizedValue("stamina") + ": " + _stat._stamina + "\n";
        _text.text += loc.GetLocalizedValue("moveSpeed") + ": " + _stat._runSpeed + "\n";
        _text.text += loc.GetLocalizedValue("physicalDamage") + ": " +_stat._physicalDamage + "\n";
        _text.text += loc.GetLocalizedValue("magicDamage") + ": " + _stat._magicDamage + "\n";
        _text.text += loc.GetLocalizedValue("critPhysical") + ": " + _stat._critChancePhysical + "%\n";
        _text.text += loc.GetLocalizedValue("critMagic") + ": " + _stat._critChanceMagic + "%\n";
        _text.text += loc.GetLocalizedValue("physicRes") + ": " + _stat._physicalDefense + "%\n";
        _text.text += loc.GetLocalizedValue("armor") + ": " + _stat._armor + "\n";
        _text.text += loc.GetLocalizedValue("magicRes") + ": " + _stat._magicResist + "\n";
        _text.text += loc.GetLocalizedValue("attackSpeed") + ": " + _stat._attackSpeed + "%\n";
        _text.text += loc.GetLocalizedValue("cooldownReduction") + ": " + _stat._cooldownReduction + "%\n";
    }
}

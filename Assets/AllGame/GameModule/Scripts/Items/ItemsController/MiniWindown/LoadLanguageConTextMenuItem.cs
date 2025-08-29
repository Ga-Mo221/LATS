using TMPro;
using UnityEngine;

public class LoadLanguageConTextMenuItem : MonoBehaviour
{
    public RtItem _rtItem;

    [Header("Inventory")]
    [SerializeField] private TextMeshProUGUI _useText;
    [SerializeField] private TextMeshProUGUI _equieText;
    [SerializeField] private TextMeshProUGUI _repairText;
    [SerializeField] private TextMeshProUGUI _sellText;
    [SerializeField] private TextMeshProUGUI _deleteText;


    void Start()
    {
        debug();
        loadLanguage();
    }

    public void loadLanguage()
    {
        LocalizationManager loc = new LocalizationManager();
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "itemButton");
        _useText.text = loc.GetLocalizedValue("use");
        _equieText.text = loc.GetLocalizedValue("equie");
        _repairText.text = loc.GetLocalizedValue("reqair");
        _sellText.text = loc.GetLocalizedValue("sell");
        _deleteText.text = loc.GetLocalizedValue("delete");
    }

    private void debug()
    {
        // inventory
        if (!_useText)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'UseText'");
        if (!_equieText)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'EquieText'");
        if (!_repairText)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'ReqirText'");
        if (!_sellText)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'SellText'");
        if (!_deleteText)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'DeleteText'");
    }
}

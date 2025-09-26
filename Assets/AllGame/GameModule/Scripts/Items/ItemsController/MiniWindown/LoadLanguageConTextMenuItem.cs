using TMPro;
using UnityEngine;

public class LoadLanguageConTextMenuItem : MonoBehaviour
{
    public RtItem _rtItem;

    [Header("Inventory_R")]
    [SerializeField] private TextMeshProUGUI _equieText_inventory_R;
    [SerializeField] private TextMeshProUGUI _repairText_inventory_R;
    [SerializeField] private TextMeshProUGUI _deleteText_inventory_R;

    [Header("Inventory_L")]
    [SerializeField] private TextMeshProUGUI _equieText_inventory_L;
    [SerializeField] private TextMeshProUGUI _repairText_inventory_L;
    [SerializeField] private TextMeshProUGUI _deleteText_inventory_L;

    [Header("Inventory_R_Consumable")]
    [SerializeField] private TextMeshProUGUI _useText_inventory_R_Consumable;
    [SerializeField] private TextMeshProUGUI _deleteText_inventory_R_Consumable;

    [Header("Inventory_L_Consumable")]
    [SerializeField] private TextMeshProUGUI _useText_inventory_L_Consumable;
    [SerializeField] private TextMeshProUGUI _deleteText_inventory_L_Consumable;
    
    [Header("UnEquie_R")]
    [SerializeField] private TextMeshProUGUI _unEquie_R;
    [Header("UnEquie_L")]
    [SerializeField] private TextMeshProUGUI _unEquie_L;

    //[SerializeField] private TextMeshProUGUI _sellText;


    void Start()
    {
        debug();
        loadLanguage();
    }

    public void loadLanguage()
    {
        LocalizationManager loc = new LocalizationManager();
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "itemButton");

        // inventory_R
        _equieText_inventory_R.text = loc.GetLocalizedValue("equie");
        _repairText_inventory_R.text = loc.GetLocalizedValue("reqair");
        _deleteText_inventory_R.text = loc.GetLocalizedValue("delete");

        // inventory_L
        _equieText_inventory_L.text = loc.GetLocalizedValue("equie");
        _repairText_inventory_L.text = loc.GetLocalizedValue("reqair");
        _deleteText_inventory_L.text = loc.GetLocalizedValue("delete");

        // inventory_R_Consumable
        _useText_inventory_R_Consumable.text = loc.GetLocalizedValue("use");
        _deleteText_inventory_R_Consumable.text = loc.GetLocalizedValue("delete");

        // inventory_L_Consumable
        _useText_inventory_L_Consumable.text = loc.GetLocalizedValue("use");
        _deleteText_inventory_L_Consumable.text = loc.GetLocalizedValue("delete");

        // _unequip_R
        _unEquie_R.text = loc.GetLocalizedValue("unequie");

        // _unequip_L
        _unEquie_L.text = loc.GetLocalizedValue("unequie");
    }

    private void debug()
    {
        // inventory_R
        if (!_equieText_inventory_R)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'EquieText_inventory_R'");
        if (!_repairText_inventory_R)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'ReqirText_inventory_R'");
        if (!_deleteText_inventory_R)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'DeleteText_inventory_R'");

        // inventory_L
        if (!_equieText_inventory_L)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'EquieText_inventory_L'");
        if (!_repairText_inventory_L)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'ReqirText_inventory_L'");
        if (!_deleteText_inventory_L)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'DeleteText_inventory_L'");

        // inventory_R_Consumable
        if (!_useText_inventory_R_Consumable)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'UseText_inventory_R_Consumable'");
        if (!_deleteText_inventory_R_Consumable)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'DeleteText_inventory_R_Consumable'");

        // inventory_L_Consumable
        if (!_useText_inventory_L_Consumable)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'UseText_inventory_L_Consumable'");
        if (!_deleteText_inventory_L_Consumable)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'DeleteText_inventory_L_Consumable'");

        // unquie_R
        if (!_unEquie_R)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'UnEquie_R'");

        // unquie_L
        if (!_unEquie_L)
            Debug.LogError("[LoadLanguageConTextMenuItem] Chưa gán 'UnEquie_L'");
    }
}

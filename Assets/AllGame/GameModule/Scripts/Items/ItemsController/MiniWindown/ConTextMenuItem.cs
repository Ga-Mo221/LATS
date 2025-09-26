using UnityEngine;
using UnityEngine.UI;

public class ConTextMenuItem : MonoBehaviour
{
    public RtItem _rtItem;

    [SerializeField] private ContextMenuController _contextMenuController;
    [SerializeField] private LoadLanguageConTextMenuItem _loadLanguage;
    [SerializeField] private GameObject _inventory_R;
    [SerializeField] private GameObject _inventory_L;
    [SerializeField] private Button _butonRepair_R;
    [SerializeField] private Button _butonRepair_L;
    [SerializeField] private GameObject _inventory_R_Consumable;
    [SerializeField] private GameObject _inventory_L_Consumable;
    [SerializeField] private GameObject _unquie_R;
    [SerializeField] private GameObject _unquie_L;


    void Start()
    {
        if (!_contextMenuController)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'ContextMenuController'");
        if (!_loadLanguage)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'LoadLanguageConTextMenuItem'");
        if (!_inventory_R)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'Inventory_R'");
        if (!_inventory_L)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'Inventory_L'");
        if (!_inventory_R_Consumable)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'Inventory_R_Consumable'");
        if (!_inventory_L_Consumable)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'Inventory_L_Consumable'");
        if (!_unquie_R)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'Unquie_R'");
        if (!_unquie_L)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'Unquie_L'");
        if (!_butonRepair_R)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'Button Repair Right'");
        if (!_butonRepair_L)
            Debug.LogError("[ConTextMenuItem] Chưa gán 'Button Repair Left'");
    }

    public RtItem getRtItem() => _rtItem;

    public void setItem(RtItem rtItem, Vector3 pos, Vector3 centerPos)
    {
        _rtItem = rtItem;
        transform.position = pos;
        _loadLanguage.loadLanguage();
        _contextMenuController.setRtItem(_rtItem);

        if (_rtItem._itemStatus == ItemStatus.UnEquip)
        {
            if (_rtItem._baseItem._itemType == ItemType.Consumable)
            {
                if (pos.x > centerPos.x)
                {
                    _inventory_R.SetActive(false);
                    _inventory_L.SetActive(false);
                    _inventory_R_Consumable.SetActive(false);
                    _inventory_L_Consumable.SetActive(true);
                    _unquie_R.SetActive(false);
                    _unquie_L.SetActive(false);
                }
                else
                {
                    _inventory_R.SetActive(false);
                    _inventory_L.SetActive(false);
                    _inventory_R_Consumable.SetActive(true);
                    _inventory_L_Consumable.SetActive(false);
                    _unquie_R.SetActive(false);
                    _unquie_L.SetActive(false);
                }
                return;
            }
            if (pos.x > centerPos.x)
            {
                _inventory_R.SetActive(false);
                _inventory_L.SetActive(true);
                _inventory_R_Consumable.SetActive(false);
                _inventory_L_Consumable.SetActive(false);
                _unquie_R.SetActive(false);
                _unquie_L.SetActive(false);
                if (_rtItem._currentDurability < _rtItem._baseItem._maxDurability)
                {
                    _butonRepair_R.interactable = true;
                    _butonRepair_L.interactable = true;
                }
                else
                {
                    _butonRepair_R.interactable = false;
                    _butonRepair_L.interactable = false;
                }
            }
            else
            {
                _inventory_R.SetActive(true);
                _inventory_L.SetActive(false);
                _inventory_R_Consumable.SetActive(false);
                _inventory_L_Consumable.SetActive(false);
                _unquie_R.SetActive(false);
                _unquie_L.SetActive(false);
                if (_rtItem._currentDurability < _rtItem._baseItem._maxDurability)
                {
                    _butonRepair_R.interactable = true;
                    _butonRepair_L.interactable = true;
                }
                else
                {
                    _butonRepair_R.interactable = false;
                    _butonRepair_L.interactable = false;
                }
            }
        }
        else if (_rtItem._itemStatus == ItemStatus.Equip)
        {
            if (pos.x > centerPos.x)
            {
                _inventory_R.SetActive(false);
                _inventory_L.SetActive(false);
                _inventory_R_Consumable.SetActive(false);
                _inventory_L_Consumable.SetActive(false);
                _unquie_R.SetActive(false);
                _unquie_L.SetActive(true);
            }
            else
            {
                _inventory_R.SetActive(false);
                _inventory_L.SetActive(false);
                _inventory_R_Consumable.SetActive(false);
                _inventory_L_Consumable.SetActive(false);
                _unquie_R.SetActive(true);
                _unquie_L.SetActive(false);
            }
        }
    }
}

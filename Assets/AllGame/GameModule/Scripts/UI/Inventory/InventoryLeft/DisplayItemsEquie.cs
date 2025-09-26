using Unity.VisualScripting;
using UnityEngine;

public class DisplayItemsEquie : MonoBehaviour
{
    [SerializeField] private LoadPlayerStatsToText _loadLanguage;

    [SerializeField] private ItemUiController _meleen;
    [SerializeField] private ItemUiController _ranged;
    [SerializeField] private ItemUiController _helmet;
    [SerializeField] private ItemUiController _armor;
    [SerializeField] private ItemUiController _boots;
    [SerializeField] private ItemUiController _accessory;
    [SerializeField] private ItemUiController _consumable;


    void Start()
    {
        if (!_loadLanguage)
            Debug.LogError("[DisplayItemsEquie] Chưa gán 'LoadPlayerStatsToText'");
        if (!_meleen)
            Debug.LogError("[DisplayItemsEquie] Chưa gán 'Meleen'");
        if (!_ranged)
            Debug.LogError("[DisplayItemsEquie] Chưa gán 'Ranged'");
        if (!_helmet)
            Debug.LogError("[DisplayItemsEquie] Chưa gán 'Helmet'");
        if (!_armor)
            Debug.LogError("[DisplayItemsEquie] Chưa gán 'Armor'");
        if (!_boots)
            Debug.LogError("[DisplayItemsEquie] Chưa gán 'Boots'");
        if (!_accessory)
            Debug.LogError("[DisplayItemsEquie] Chưa gán 'Accessory'");
        if (!_consumable)
            Debug.LogError("[DisplayItemsEquie] Chưa gán 'Consumable'");

        display();
    }

    public void loadLanguage() => _loadLanguage.display();

    public void display()
    {
        EquipedItem _equie = InventoryManager.Instance._equipedItem;

        if (_equie._isMelleWeapon)
        {
            _meleen.setRtItem(_equie._MeleeWeapon);
        }
        else _meleen.setRtItem(null);

        if (_equie._isRangedWeapon)
        {
            _ranged.setRtItem(_equie._RangedWeapon);
        }
        else _ranged.setRtItem(null);

        if (_equie._isHelmet)
        {
            _helmet.setRtItem(_equie._helmet);
        }
        else _helmet.setRtItem(null);

        if (_equie._isArmor)
        {
            _armor.setRtItem(_equie._armor);
        }
        else _armor.setRtItem(null);

        if (_equie._isBoots)
        {
            _boots.setRtItem(_equie._boots);
        }
        else _boots.setRtItem(null);

        if (_equie._isAccesory)
        {
            _accessory.setRtItem(_equie._accessory);
        }
        else _accessory.setRtItem(null);

        if (_equie._isConsumahble)
        {
            _consumable.setRtItem(_equie._Consumahble);
        }
        else _consumable.setRtItem(null);
    }
}

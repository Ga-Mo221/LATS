using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUiController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RtItem _rtItem;
    [SerializeField] public Image _itemIcon;
    [SerializeField] private Transform _itemStatPos;
    public bool _pickUp = false;
    private Transform _contextMenu_Overlay;
    private Transform _itemStats_Overlay;
    private int _clickCount = 0;
    private Coroutine _resetClickCount;
    private Coroutine _showItemStat;    


    void Start()
    {
        _contextMenu_Overlay = InventoryManager.Instance._contextMenu_Overlay.transform;
        _itemStats_Overlay = InventoryManager.Instance._itemStats_Overlay.transform;
        if (!_itemStatPos)
            Debug.LogError("[ItemUiController] Chưa gán 'ItemStatPos'");
        if (!_itemIcon)
            Debug.LogError("[ItemUiController] Chưa gán 'ItemIcon'");
    }

    #region Check Mouse Enter
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_rtItem == null) return;
        if (_rtItem._itemID == "") return;
        if (_showItemStat != null)
            StopCoroutine(_showItemStat);
        _showItemStat = StartCoroutine(showItemStat());
    }
    #endregion


    #region Check Mouse Exit
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_rtItem == null) return;
        if (_rtItem._itemID == "") return;
        if (_showItemStat != null)
        {
            StopCoroutine(_showItemStat);
        }
        _itemStats_Overlay.gameObject.SetActive(false);
    }
    #endregion


    #region Show Item Stat (Mouse Hover)
    private IEnumerator showItemStat()
    {
        yield return new WaitForSeconds(1f);
        _itemStats_Overlay.gameObject.SetActive(true);
        var _itemStatsWindown = InventoryManager.Instance._itemStatsWindown;
        _itemStatsWindown.ShowItemStats(stringitemStats(),_rtItem, _itemStatPos.position);
    }
    #endregion


    #region Check Mouse Click
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    #endregion


    #region Set Runtime Item
    public void setRtItem(RtItem item)
    {
        if (item != null)
        {
            _rtItem = item;
            _itemIcon.sprite = item._baseItem._itemIcon;
            _itemIcon.enabled = true;
        }
        else
        {
            _rtItem = null;
            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
        }
    }
    #endregion


    #region Mouse Left Click
    private void OnLeftClick()
    {
        if (_rtItem == null) return;
        if (_rtItem._itemID == "") return;
        //add
        //Xử lý pickup nếu item đang ở trạng thái Drop
        if (_rtItem._itemStatus == ItemStatus.Drop)
        {
            pickUpItem();
        }
        if (_rtItem._itemStatus == ItemStatus.UnEquip || _rtItem._itemStatus == ItemStatus.Equip)
            EquipORUnEquip();
    }
    #endregion

    //add
    #region Pick Up Item from Chest
    public void pickUpItem()
    {
        // Check inventory còn chỗ trống
        bool added = false;
        switch (_rtItem._baseItem._itemType)
        {
            case ItemType.Weapon:
                added = InventoryManager.Instance._rtItemsWeapon.Count < InventoryConstants.MAX_WEAPON;
                break;
            case ItemType.Helmet:
                added = InventoryManager.Instance._rtItemsHelmet.Count < InventoryConstants.MAX_HELMET;
                break;
            case ItemType.Armor:
                added = InventoryManager.Instance._rtItemsArmor.Count < InventoryConstants.MAX_ARMOR;
                break;
            case ItemType.Boots:
                added = InventoryManager.Instance._rtItemsBoots.Count < InventoryConstants.MAX_BOOTS;
                break;
            case ItemType.Accessory:
                added = InventoryManager.Instance._rtItemsAccesory.Count < InventoryConstants.MAX_ACCESSORY;
                break;
            case ItemType.Consumable:
                added = InventoryManager.Instance._rtItemsConsumahble.Count < InventoryConstants.MAX_CONSUMABLE;                
                break;
        }
        if (added)
        {
            InventoryManager.Instance.addItemToList(_rtItem);
            _pickUp = true;
        }
    }
    #endregion


    #region Equip or UnEquip Item (Double Click)
    private void EquipORUnEquip()
    {
        _clickCount++;
        if (_resetClickCount != null)
            StopCoroutine(_resetClickCount);
        _resetClickCount = StartCoroutine(resetClickCount());
        if (_clickCount == 2)
        {
            if (_itemStats_Overlay.gameObject.activeSelf)
                _itemStats_Overlay.gameObject.SetActive(false);
            if (_rtItem._itemStatus == ItemStatus.UnEquip)
                {
                    InventoryManager.Instance.equip(_rtItem);
                }
                else if (_rtItem._itemStatus == ItemStatus.Equip)
                {
                    InventoryManager.Instance.unEquip(_rtItem);
                }
        }
    }
    #endregion


    #region Reset Click Count
    private IEnumerator resetClickCount()
    {
        yield return new WaitForSeconds(0.5f);
        _clickCount = 0;
    }
    #endregion


    #region Mouse Right Click
    private void OnRightClick()
    {
        if (_rtItem == null) return;
        if (_rtItem._itemID == "") return;
        _contextMenu_Overlay.gameObject.SetActive(true);
        Vector3 _pos = transform.position;
        var _itemContextMenuItem = InventoryManager.Instance._itemContextMenuItem;
        //add this
        _itemContextMenuItem.setItem(_rtItem, _pos, InventoryManager.Instance._inventoryRightPos.position,this);
    }
    #endregion


    #region Show Item Stat Button
    public void showItemStatButton()
    {
        TextMeshProUGUI _text = InventoryManager.Instance._itemStatsText;
        Slider _durabilitySlider = InventoryManager.Instance._durabilitySlider;
        if (_rtItem == null)
        {
            _text.text = "";
            _durabilitySlider.gameObject.SetActive(false);
            return;
        }
        if (string.IsNullOrEmpty(_rtItem._itemID)) return;

        if (_rtItem._itemStatus == ItemStatus.UnEquip || _rtItem._itemStatus == ItemStatus.Equip)
        {
            _durabilitySlider.gameObject.SetActive(true);
            _text.text = stringitemStats();
            float _intDurability = _rtItem._currentDurability;
            _durabilitySlider.maxValue = _rtItem._baseItem._maxDurability;
            _durabilitySlider.value = _intDurability;
        }
    }
    #endregion


    #region String Item Stats
    private string stringitemStats()
    {
        LocalizationManager loc = new LocalizationManager();
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "itemStats");
        string _text;
        _text = $"{loc.GetLocalizedValue("name")} : {_rtItem._baseItem._itemName}\n";

        // ===== RARITY =====
        switch (_rtItem._baseItem._itemRarity)
        {
            case ItemRarity.Common: _text += loc.GetLocalizedValue("rarityCommon") + "\n"; break;
            case ItemRarity.Rare: _text += $"{loc.GetLocalizedValue("rarityRare")}\n"; break;
            case ItemRarity.Legendary: _text += $"{loc.GetLocalizedValue("rarityLegendary")}\n"; break;
        }

        // ===== TYPE =====
        switch (_rtItem._baseItem._itemType)
        {
            case ItemType.Weapon: _text += $"{loc.GetLocalizedValue("weaponType")}\n"; break;
            case ItemType.Helmet: _text += $"{loc.GetLocalizedValue("helmetType")}\n"; break;
            case ItemType.Armor: _text += $"{loc.GetLocalizedValue("armorType")}\n"; break;
            case ItemType.Boots: _text += $"{loc.GetLocalizedValue("bootsType")}\n"; break;
            case ItemType.Accessory: _text += $"{loc.GetLocalizedValue("accessoryType")}\n"; break;
            case ItemType.Consumable: _text += $"{loc.GetLocalizedValue("consumableType")}\n"; break;
        }

        // ===== WEAPON TYPE =====
        if (_rtItem._baseItem._itemType == ItemType.Weapon)
        {
            switch (_rtItem._baseItem._weaponType)
            {
                case WeaponType.Melee: _text += $"{loc.GetLocalizedValue("weaponMelee")}\n"; break;
                case WeaponType.Ranged: _text += $"{loc.GetLocalizedValue("weaponRanged")}\n"; break;
            }
        }

        // ===== STATS =====
        void AppendStat(string label, float baseValue, float bonusValue)
        {
            if (baseValue != 0 || bonusValue != 0)
            {
                _text += $"{label}: {baseValue}";
                if (bonusValue != 0)
                    _text += $" + {bonusValue}";
                _text += "\n";
            }
        }

        AppendStat(loc.GetLocalizedValue("physicalDamage"), _rtItem._baseItem._itemDamagePhysical, _rtItem._bonusDamagePhysical);
        AppendStat(loc.GetLocalizedValue("magicDamage"), _rtItem._baseItem._itemDamgeMagic, _rtItem._bonusDamgeMagic);
        AppendStat(loc.GetLocalizedValue("attackSpeed"), _rtItem._baseItem._itemSpeedAttack, _rtItem._bonusSpeedAttack);
        AppendStat(loc.GetLocalizedValue("moveSpped"), _rtItem._baseItem._itemSpeedMove, _rtItem._bonusSpeedMove);
        AppendStat(loc.GetLocalizedValue("health"), _rtItem._baseItem._itemHealth, _rtItem._bonusHealth);
        AppendStat(loc.GetLocalizedValue("critPhysical"), _rtItem._baseItem._itemcritChancePhysical, _rtItem._bonuscritChancePhysical);
        AppendStat(loc.GetLocalizedValue("critMagic"), _rtItem._baseItem._itemCritChanceMagic, _rtItem._bonusCritChanceMagic);
        AppendStat(loc.GetLocalizedValue("armor"), _rtItem._baseItem._itemArmor, _rtItem._bonusArmor);
        AppendStat(loc.GetLocalizedValue("physicRes"), _rtItem._baseItem._itemPhysicRes, _rtItem._bonusPhysicRes);
        AppendStat(loc.GetLocalizedValue("magicRes"), _rtItem._baseItem._itemMagicResist, _rtItem._bonusMagicResist);
        AppendStat(loc.GetLocalizedValue("cooldownReduction"), _rtItem._baseItem._itemCooldownReduction, _rtItem._bonusCooldownReduction);

        // ===== EXTRA =====
        _text += loc.GetLocalizedValue("durability");
        return _text;
    }
    #endregion
}

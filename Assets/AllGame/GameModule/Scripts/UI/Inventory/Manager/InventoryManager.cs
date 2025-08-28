using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class InventoryManager : MonoBehaviour
{
    #region Singleton
    public static InventoryManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion


    #region Values
    [Header("All Items")]
    public List<Item> _allItems = new List<Item>();

    [Header("Inventory")]
    public List<RtItem> _rtItemsWeapon = new List<RtItem>(); // gioi hang 10
    public List<RtItem> _rtItemsHelmet = new List<RtItem>(); // gioi hang 15
    public List<RtItem> _rtItemsArmor = new List<RtItem>(); // gioi hang 15
    public List<RtItem> _rtItemsBoots = new List<RtItem>(); // gioi hang 15
    public List<RtItem> _rtItemsAccesory = new List<RtItem>(); // gioi hang 15
    public List<RtItem> _rtItemsConsumahble = new List<RtItem>(); // gioi hang 21

    [Header("Equiped Items")]
    public EquipedItem _equipedItem = new EquipedItem();

    public bool _weaponCreateItem { get; set; } = false;
    public bool _helmetCreateItem { get; set; } = false;
    public bool _armorCreateItem { get; set; } = false;
    public bool _bootsCreateItem { get; set; } = false;
    public bool _accessoryCreateItem { get; set; } = false;
    public bool _consumableCreateItem { get; set; } = false;


    [Header("GameObject")]
    public GameObject _itemStats;
    public ItemStatsWindown _itemStatsWindown;
    public GameObject _itemContextMenu;
    public ConTextMenuItem _itemContextMenuItem;


    [Header("References")]
    public bool _show = false;
    [ShowIf(nameof(_show))]
    [SerializeField] public InventoryWeapon _inventoryWeapon;
    [ShowIf(nameof(_show))]
    [SerializeField] private InventoryHelmet _inventoryHelmet;
    [ShowIf(nameof(_show))]
    [SerializeField] private InventoryArmor _inventoryArmor;
    [ShowIf(nameof(_show))]
    [SerializeField] private InventoryBoots _inventoryBoots;
    [ShowIf(nameof(_show))]
    [SerializeField] private InventoryAccessory _inventoryAccessory;
    [ShowIf(nameof(_show))]
    [SerializeField] private InventoryConsumable _inventoryConsumable;

    [ShowIf(nameof(_show))]
    [SerializeField] private SaveItem _saveItem;
    [ShowIf(nameof(_show))]
    [SerializeField] public GameObject _contextMenu_Overlay;
    [ShowIf(nameof(_show))]
    [SerializeField] public GameObject _itemStats_Overlay;
    [ShowIf(nameof(_show))]
    [SerializeField] public TextMeshProUGUI _itemStatsText;
    [ShowIf(nameof(_show))]
    [SerializeField] public Slider _durabilitySlider;

    #endregion


    void Start()
    {
        if (!_saveItem)
            Debug.LogError("[InventoryManager] Chưa gán 'SaveItem'");
        if (!_contextMenu_Overlay)
            Debug.LogError("[InventoryManager] Chưa gán 'GameObject _contextMenu_Overlay'");
        if (!_itemStats_Overlay)
            Debug.LogError("[InventoryManager] Chưa gán 'GameObject _itemStats_Overlay'");
        if (!_inventoryWeapon)
            Debug.LogError("[InventoryManager] Chưa gán 'InventoryWeapon'");
        if (!_inventoryHelmet)
            Debug.LogError("[InventoryManager] Chưa gán 'InventoryHelmet'");
        if (!_inventoryArmor)
            Debug.LogError("[InventoryManager] Chưa gán 'InventoryArmor'");
        if (!_inventoryBoots)
            Debug.LogError("[InventoryManager] Chưa gán 'InventoryBoots'");
        if (!_inventoryAccessory)
            Debug.LogError("[InventoryManager] Chưa gán 'InventoryAccessory'");
        if (!_inventoryConsumable)
            Debug.LogError("[InventoryManager] Chưa gán 'InventoryConsumable'");
        if (!_itemStatsText)
            Debug.LogError("[InventoryManager] Chưa gán 'TextMeshProUGUI _itemStatsText'");
        if (!_durabilitySlider)
            Debug.LogError("[InventoryManager] Chưa gán 'Slider _durabilitySlider'");

        foreach (var item in InventoryManager.Instance._allItems)
        {
            for (int i = 0; i < 10; i++)
                InventoryManager.Instance.addItemToList(item);
        }
        autoCreatePrefab();
    }


    #region Auto Create Prefab
    private void autoCreatePrefab()
    {
        _itemStats = Instantiate(GameModule.Instance._itemStatPrefab, _itemStats_Overlay.transform);
        _itemStatsWindown = _itemStats.GetComponent<ItemStatsWindown>();

        _itemContextMenu = Instantiate(GameModule.Instance._contextMenuPrefab, _contextMenu_Overlay.transform);
        _itemContextMenuItem = _itemContextMenu.GetComponent<ConTextMenuItem>();
    }
    #endregion


    #region Load Items on Folder Items
#if UNITY_EDITOR
    [ContextMenu("📦 Import All Items In 'Assets/AllGame/GameModule/Items'")]
    private void importAllItemsInFolderItems()
    {
        _allItems.Clear();

        // Tìm tất cả các asset loại Item trong thư mục Assets/AllGame/GameModule/Items
        string[] guids = AssetDatabase.FindAssets("t:Item", new[] { "Assets/AllGame/GameModule/Items" });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Item item = AssetDatabase.LoadAssetAtPath<Item>(path);
            if (item != null)
            {
                _allItems.Add(item);
            }
        }

        Debug.Log($"[InventoryManager] ✅ Đã import {_allItems.Count} item từ thư mục Assets/AllGame/GameModule/Items.");
    }
#endif
    #endregion


    #region Load Items On File Save
    public void loadItemsInFileSave()
    {
        //_saveItem.LoadFromJson(_rtItems);
    }
    #endregion


    #region Save Items
    public void saveItemsInFile()
    {
        //_saveItem.SaveToJson(_rtItems);
    }
    #endregion


    #region Reset Inventory
    public void resetInventory()
    {
    }
    #endregion


    #region Add Item To List
    public void addItemToList(Item item)
    {
        RtItem rtItem = new RtItem(item);
        switch (rtItem._baseItem._itemType)
        {
            case ItemType.Weapon:
                {
                    if (_rtItemsWeapon.Count < InventoryConstants.MAX_WEAPON)
                        _rtItemsWeapon.Add(rtItem);
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Helmet:
                {
                    if (_rtItemsHelmet.Count < InventoryConstants.MAX_HELMET)
                        _rtItemsHelmet.Add(rtItem);
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Armor:
                {
                    if (_rtItemsArmor.Count < InventoryConstants.MAX_ARMOR)
                        _rtItemsArmor.Add(rtItem);
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Boots:
                {
                    if (_rtItemsBoots.Count < InventoryConstants.MAX_BOOTS)
                        _rtItemsBoots.Add(rtItem);
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Accessory:
                {
                    if (_rtItemsAccesory.Count < InventoryConstants.MAX_ACCESSORY)
                        _rtItemsAccesory.Add(rtItem);
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Consumable:
                {
                    if (_rtItemsConsumahble.Count < InventoryConstants.MAX_CONSUMABLE)
                        _rtItemsConsumahble.Add(rtItem);
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
        }
    }
    public void addItemToList(RtItem rtItem)
    {
        switch (rtItem._baseItem._itemType)
        {
            case ItemType.Weapon:
                {
                    if (_rtItemsWeapon.Count < InventoryConstants.MAX_WEAPON)
                    {
                        rtItem._itemStatus = ItemStatus.UnEquip;
                        _rtItemsWeapon.Add(rtItem);
                    }
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Helmet:
                {
                    if (_rtItemsHelmet.Count < InventoryConstants.MAX_HELMET)
                    {
                        rtItem._itemStatus = ItemStatus.UnEquip;
                        _rtItemsHelmet.Add(rtItem);
                    }
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Armor:
                {
                    if (_rtItemsArmor.Count < InventoryConstants.MAX_ARMOR)
                    {
                        rtItem._itemStatus = ItemStatus.UnEquip;
                        _rtItemsArmor.Add(rtItem);
                    }
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Boots:
                {
                    if (_rtItemsBoots.Count < InventoryConstants.MAX_BOOTS)
                    {
                        rtItem._itemStatus = ItemStatus.UnEquip;
                        _rtItemsBoots.Add(rtItem);
                    }
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Accessory:
                {
                    if (_rtItemsAccesory.Count < InventoryConstants.MAX_ACCESSORY)
                    {
                        rtItem._itemStatus = ItemStatus.UnEquip;
                        _rtItemsAccesory.Add(rtItem);
                    }
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
            case ItemType.Consumable:
                {
                    if (_rtItemsConsumahble.Count < InventoryConstants.MAX_CONSUMABLE)
                    {
                        rtItem._itemStatus = ItemStatus.UnEquip;
                        _rtItemsConsumahble.Add(rtItem);
                    }
                    else
                        Debug.Log("[InventoryManager] Túi đã đầy");
                    break;
                }
        }
    }
    #endregion


    #region Equip Item
    public void equip(RtItem item)
    {
        _equipedItem.equip(item);
        displayInventory(item);
        // DisplayEquipedIetm();
        applyItem(item, true);
    }
    #endregion


    #region UnEquip Item
    public void unEquip(RtItem item)
    {
        _equipedItem.unEquip(item);
        displayInventory(item);
        // DisplayEquipedIetm();
        applyItem(item, false);
    }
    #endregion


    #region Display Inventory
    private void displayInventory(RtItem item)
    {
        switch (item._baseItem._itemType)
        {
            case ItemType.Weapon:
                {
                    _inventoryWeapon.displayItemInInventory();
                    break;
                }
            case ItemType.Helmet:
                {
                    _inventoryHelmet.displayItemInInventory();
                    break;
                }
            case ItemType.Armor:
                {
                    _inventoryArmor.displayItemInInventory();
                    break;
                }
            case ItemType.Boots:
                {
                    _inventoryBoots.displayItemInInventory();
                    break;
                }
            case ItemType.Accessory:
                {
                    _inventoryAccessory.displayItemInInventory();
                    break;
                }
            case ItemType.Consumable:
                {
                    _inventoryConsumable.displayItemInInventory();
                    break;
                }
        }
    }
    #endregion


    #region Apply Item
    private void applyItem(RtItem item, bool equip)
    {
        float _physic = item._baseItem._itemDamagePhysical + item._bonusDamagePhysical;
        float _magic = item._baseItem._itemDamgeMagic + item._bonusDamgeMagic;
        float _attackSpeed = item._baseItem._itemSpeedAttack + item._bonusSpeedAttack;
        float _moveSpeed = item._baseItem._itemSpeedMove + item._bonusSpeedMove;
        float _health = item._baseItem._itemHealth + item._bonusHealth;
        float _critPhysic = item._baseItem._itemCritChanceMagic + item._bonusCritChanceMagic;
        float _critMagic = item._baseItem._itemcritChancePhysical + item._bonuscritChancePhysical;
        float _armor = item._baseItem._itemArmor + item._bonusArmor;
        float _physicRes = item._baseItem._itemPhysicRes + item._bonusPhysicRes;
        float _magicRes = item._baseItem._itemMagicResist + item._bonusMagicResist;
        float _cooldown = item._baseItem._itemCooldownReduction + item._bonusCooldownReduction;
        if (equip)
        {
            PlayerManager.Instance._start._physicalDamage += _physic;
            PlayerManager.Instance._start._magicDamage += _magic;
            PlayerManager.Instance._start._runSpeed += _moveSpeed;
            PlayerManager.Instance._start._walkSpeed += _moveSpeed;
            PlayerManager.Instance._start._attackSpeed += _attackSpeed;
            PlayerManager.Instance._start._maxHealth += _health;
            PlayerManager.Instance._start._currentHealth += _health;
            PlayerManager.Instance._start._critChancePhysical += _critPhysic;
            PlayerManager.Instance._start._critChanceMagic += _critMagic;
            PlayerManager.Instance._start._armor += _armor;
            PlayerManager.Instance._start._physicalDefense += _physicRes;
            PlayerManager.Instance._start._magicResist += _magicRes;
            PlayerManager.Instance._start._cooldownReduction += _cooldown;
        }
        else
        {
            PlayerManager.Instance._start._physicalDamage -= _physic;
            PlayerManager.Instance._start._magicDamage -= _magic;
            PlayerManager.Instance._start._runSpeed -= _moveSpeed;
            PlayerManager.Instance._start._walkSpeed -= _moveSpeed;
            PlayerManager.Instance._start._attackSpeed -= _attackSpeed;
            PlayerManager.Instance._start._maxHealth -= _health;
            PlayerManager.Instance._start._currentHealth -= _health;
            PlayerManager.Instance._start._critChancePhysical -= _critPhysic;
            PlayerManager.Instance._start._critChanceMagic -= _critMagic;
            PlayerManager.Instance._start._armor -= _armor;
            PlayerManager.Instance._start._physicalDefense -= _physicRes;
            PlayerManager.Instance._start._magicResist -= _magicRes;
            PlayerManager.Instance._start._cooldownReduction -= _cooldown;
        }
        PlayerManager.Instance.setAttackSpeed();
    }
    #endregion


    #region Add Item In Inventory UI
    public void addItemInInventory(RtItem item, GameObject obj)
    {
        var _imgItem = obj.GetComponent<ItemUiController>()._itemIcon;
        _imgItem.sprite = item._baseItem._itemIcon;
        _imgItem.enabled = true;
        obj.GetComponent<ItemUiController>().setRtItem(item);
    }
    #endregion


    #region Remove Item In Inventory UI
    public void removeItemInInventory(RtItem item, GameObject obj)
    {
        var _imgItem = obj.GetComponent<ItemUiController>()._itemIcon;
        _imgItem.sprite = null;
        _imgItem.enabled = false;
        obj.GetComponent<ItemUiController>().setRtItem(null);
    }
    #endregion
}

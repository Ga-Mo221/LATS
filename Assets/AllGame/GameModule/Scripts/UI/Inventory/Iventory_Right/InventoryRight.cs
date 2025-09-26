using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryRight : MonoBehaviour
{
    [Header("Game Object")]
    [SerializeField] public GameObject _inventoryR;

    [Header("Scripts")]
    [SerializeField] private GetMainCam _camera;
    [SerializeField] public InventoryWeapon _inventoryWeapon;
    [SerializeField] private InventoryHelmet _inventoryHelmet;
    [SerializeField] private InventoryArmor _inventoryArmor;
    [SerializeField] private InventoryBoots _inventoryBoots;
    [SerializeField] private InventoryAccessory _inventoryAccessory;
    [SerializeField] private InventoryConsumable _inventoryConsumable;

    [Header("UI")]
    [SerializeField] public TextMeshProUGUI _itemStatsText;
    [SerializeField] public Slider _durabilitySlider;


    void Start()
    {
        if (!_inventoryR)
            Debug.LogError("[InventoryManager] Chưa gán 'GameObject Inventory'");
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
    }

    public void display(RtItem item)
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


    public void setActiveInventory(bool amount, Vector3 pos)
    {
        _inventoryR.transform.position = new Vector3(pos.x, pos.y, _inventoryR.transform.position.z);
        _inventoryR.SetActive(amount);
        _camera.setupCamera();
    }
    public bool getActiveInventory() => _inventoryR.activeSelf;
}

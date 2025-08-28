using UnityEngine;
using UnityEngine.UI;

public class InventoryRButtons : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private GameObject _weaponButton;
    [SerializeField] private GameObject _helmetButton;
    [SerializeField] private GameObject _armorButton;
    [SerializeField] private GameObject _bootsButton;
    [SerializeField] private GameObject _accessoryButton;
    [SerializeField] private GameObject _consumableButton;


    [Header("Panel")]
    [SerializeField] private GameObject _weaponPanel;
    [SerializeField] private GameObject _helmetPanel;
    [SerializeField] private GameObject _armorPanel;
    [SerializeField] private GameObject _bootsPanel;
    [SerializeField] private GameObject _accessoryPanel;
    [SerializeField] private GameObject _consumablePanel;

    void Start()
    {
        debug();
    }


    #region Select Weapon Button
    public void selectWeaponButton()
    {
        changeImageButton(_weaponButton, true);
        changeImageButton(_helmetButton, false);
        changeImageButton(_armorButton, false);
        changeImageButton(_bootsButton, false);
        changeImageButton(_accessoryButton, false);
        changeImageButton(_consumableButton, false);

        _weaponPanel.SetActive(true);
        _helmetPanel.SetActive(false);
        _armorPanel.SetActive(false);
        _bootsPanel.SetActive(false);
        _accessoryPanel.SetActive(false);
        _consumablePanel.SetActive(false);
        InventoryWeapon _script = _weaponPanel.GetComponent<InventoryWeapon>();
        if (!InventoryManager.Instance._weaponCreateItem)
            _script.autoAddItemGameObject();
        _script.displayItemInInventory();
    }
    #endregion


    #region Select Helmet Button
    public void selectHelmetButton()
    {
        changeImageButton(_weaponButton, false);
        changeImageButton(_helmetButton, true);
        changeImageButton(_armorButton, false);
        changeImageButton(_bootsButton, false);
        changeImageButton(_accessoryButton, false);
        changeImageButton(_consumableButton, false);

        _weaponPanel.SetActive(false);
        _helmetPanel.SetActive(true);
        _armorPanel.SetActive(false);
        _bootsPanel.SetActive(false);
        _accessoryPanel.SetActive(false);
        _consumablePanel.SetActive(false);
        InventoryHelmet _script = _helmetPanel.GetComponent<InventoryHelmet>();
        if (!InventoryManager.Instance._helmetCreateItem)
            _script.autoAddItemGameObject();
        _script.displayItemInInventory();
    }
    #endregion


    #region Select Armor Button
    public void selectArmorButton()
    {
        changeImageButton(_weaponButton, false);
        changeImageButton(_helmetButton, false);
        changeImageButton(_armorButton, true);
        changeImageButton(_bootsButton, false);
        changeImageButton(_accessoryButton, false);
        changeImageButton(_consumableButton, false);

        _weaponPanel.SetActive(false);
        _helmetPanel.SetActive(false);
        _armorPanel.SetActive(true);
        _bootsPanel.SetActive(false);
        _accessoryPanel.SetActive(false);
        _consumablePanel.SetActive(false);
        InventoryArmor _script = _armorPanel.GetComponent<InventoryArmor>();
        if (!InventoryManager.Instance._armorCreateItem)
            _script.autoAddItemGameObject();
        _script.displayItemInInventory();
    }
    #endregion


    #region Select Boots Button
    public void selectBootsButton()
    {
        changeImageButton(_weaponButton, false);
        changeImageButton(_helmetButton, false);
        changeImageButton(_armorButton, false);
        changeImageButton(_bootsButton, true);
        changeImageButton(_accessoryButton, false);
        changeImageButton(_consumableButton, false);

        _weaponPanel.SetActive(false);
        _helmetPanel.SetActive(false);
        _armorPanel.SetActive(false);
        _bootsPanel.SetActive(true);
        _accessoryPanel.SetActive(false);
        _consumablePanel.SetActive(false);
        InventoryBoots _script = _bootsPanel.GetComponent<InventoryBoots>();
        if (!InventoryManager.Instance._bootsCreateItem)
            _script.autoAddItemGameObject();
        _script.displayItemInInventory();
    }
    #endregion


    #region Select Accessory Button
    public void selectAccessoryButton()
    {
        changeImageButton(_weaponButton, false);
        changeImageButton(_helmetButton, false);
        changeImageButton(_armorButton, false);
        changeImageButton(_bootsButton, false);
        changeImageButton(_accessoryButton, true);
        changeImageButton(_consumableButton, false);

        _weaponPanel.SetActive(false);
        _helmetPanel.SetActive(false);
        _armorPanel.SetActive(false);
        _bootsPanel.SetActive(false);
        _accessoryPanel.SetActive(true);
        _consumablePanel.SetActive(false);
        InventoryAccessory _script = _accessoryPanel.GetComponent<InventoryAccessory>();
        if (!InventoryManager.Instance._accessoryCreateItem)
            _script.autoAddItemGameObject();
        _script.displayItemInInventory();
    }
    #endregion


    #region Select Consumable Button
    public void selectConsumableButton()
    {
        changeImageButton(_weaponButton, false);
        changeImageButton(_helmetButton, false);
        changeImageButton(_armorButton, false);
        changeImageButton(_bootsButton, false);
        changeImageButton(_accessoryButton, false);
        changeImageButton(_consumableButton, true);

        _weaponPanel.SetActive(false);
        _helmetPanel.SetActive(false);
        _armorPanel.SetActive(false);
        _bootsPanel.SetActive(false);
        _accessoryPanel.SetActive(false);
        _consumablePanel.SetActive(true);
        InventoryConsumable _script = _consumablePanel.GetComponent<InventoryConsumable>();
        if (!InventoryManager.Instance._consumableCreateItem)
            _script.autoAddItemGameObject();
        _script.displayItemInInventory();
    }
    #endregion



    private void changeImageButton(GameObject button, bool Amount)
    {
        var _imgbutton = button.GetComponent<Image>();
        var _button = button.GetComponent<Button>();
        var _imgselect = _button.spriteState.selectedSprite;
        var _imgPressed = _button.spriteState.pressedSprite;
        if (Amount)
            _imgbutton.sprite = _imgselect;
        else
            _imgbutton.sprite = _imgPressed;
    }



    private void debug()
    {
        if (!_weaponButton)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _weaponButton'");
        if (!_helmetButton)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _helmetButton'");
        if (!_armorButton)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _armorButton'");
        if (!_bootsButton)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _bootsButton'");
        if (!_accessoryButton)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _accessoryButton'");
        if (!_consumableButton)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _consumableButton'");

        if (!_weaponPanel)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _weaponPanel'");
        if (!_helmetPanel)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _helmetPanel'");
        if (!_armorPanel)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _armorPanel'");
        if (!_bootsPanel)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _bootsPanel'");
        if (!_accessoryPanel)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _accessoryPanel'");
        if (!_consumablePanel)
            Debug.LogError("[InventoryRButtons] Chưa gán 'GameObject _consumablePanel'");
    }
}

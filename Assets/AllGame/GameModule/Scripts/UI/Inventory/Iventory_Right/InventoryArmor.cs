using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class InventoryArmor : MonoBehaviour
{
    private GameObject[] _items = new GameObject[InventoryConstants.MAX_ARMOR];

    void Start()
    {
        if (!InventoryManager.Instance._armorCreateItem)
            autoAddItemGameObject();
        displayItemInInventory();
    }

    public void autoAddItemGameObject()
    {
        InventoryManager.Instance._armorCreateItem = true;
        for (int i = 0; i < InventoryConstants.MAX_ARMOR; i++)
        {
            GameObject obj = Instantiate(GameModule.Instance._ItemPrefab, transform);
            _items[i] = obj;
        }
    }

    public void displayItemInInventory()
    {
        cleanItem();
        List<RtItem> _rtItems = InventoryManager.Instance._rtItemsArmor;
        foreach (var item in _rtItems)
        {
            if (item._itemStatus != ItemStatus.UnEquip) continue;
            for (int i = 0; i < InventoryConstants.MAX_ARMOR; i++)
            {
                var _itemUiController = _items[i].GetComponent<ItemUiController>();
                var _imgItem = _itemUiController._itemIcon;
                if (_imgItem.enabled == true) continue;
                InventoryManager.Instance.addItemInInventory(item, _items[i]);
                _itemUiController.checkItemsRarity();
                break;
            }
        }
    }

    private void cleanItem()
    {
        foreach (var item in _items)
        {
            var _script = item.GetComponent<ItemUiController>();
            _script._rtItem = null;
            _script._itemIcon.enabled = false;
        }
    }
}

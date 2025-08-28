using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class InventoryConsumable : MonoBehaviour
{
    private GameObject[] _items = new GameObject[InventoryConstants.MAX_CONSUMABLE];

    void Start()
    {
        if (!InventoryManager.Instance._consumableCreateItem)
            autoAddItemGameObject();
        displayItemInInventory();
    }

    public void autoAddItemGameObject()
    {
        InventoryManager.Instance._consumableCreateItem = true;
        for (int i = 0; i < InventoryConstants.MAX_CONSUMABLE; i++)
        {
            GameObject obj = Instantiate(GameModule.Instance._ItemPrefab, transform);
            _items[i] = obj;
        }
    }

    public void displayItemInInventory()
    {
        cleanItem();
        List<RtItem> _rtItems = InventoryManager.Instance._rtItemsConsumahble;
        foreach (var item in _rtItems)
        {
            if (item._itemStatus != ItemStatus.UnEquip) continue;
            for (int i = 0; i < InventoryConstants.MAX_CONSUMABLE; i++)
            {
                var _imgItem = _items[i].GetComponent<ItemUiController>()._itemIcon;
                if (_imgItem.enabled == true) continue;
                InventoryManager.Instance.addItemInInventory(item, _items[i]);
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

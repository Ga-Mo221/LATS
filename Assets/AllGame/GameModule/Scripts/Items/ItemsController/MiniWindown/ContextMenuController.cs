using UnityEngine;

public class ContextMenuController : MonoBehaviour
{
    public RtItem _rtItem;
    private GameObject _ovelay;

    void Start()
    {
        _ovelay = InventoryManager.Instance._contextMenu_Overlay;
    }

    public void setRtItem(RtItem rtitem) => _rtItem = rtitem;

    public void equip()
    {
        InventoryManager.Instance.equip(_rtItem);
        _ovelay.SetActive(false);
    }

    public void unEquip()
    {
        InventoryManager.Instance.unEquip(_rtItem);
        _ovelay.SetActive(false);
    }

    public void repair()
    {
        Debug.Log("[ContextMenuController] Bạn đã sửa item " + _rtItem._baseItem.name);
        _ovelay.SetActive(false);
    }

    public void delete()
    {
        InventoryManager.Instance.removeItemInList(_rtItem);
        _ovelay.SetActive(false);
    }

    public void use()
    {
        Debug.Log("[ContextMenuController] Bạn đã sử dụng item " + _rtItem._baseItem.name);
        _ovelay.SetActive(false);
    }
}

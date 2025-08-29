using System.Collections.Generic;

[System.Serializable]
public class InventorySaveData
{
    public List<RtItem> rtItems = new List<RtItem>();
    public EquipedItem equipedItem = new EquipedItem();
}

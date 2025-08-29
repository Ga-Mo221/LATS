using UnityEngine;

public class ConTextMenuItem : MonoBehaviour
{
    private RtItem _rtItem;

    [SerializeField] private LoadLanguageConTextMenuItem _loadLanguage;
    [SerializeField] private GameObject _inventory_R;
    [SerializeField] private GameObject _inventory_L;

    public void setItem(RtItem rtItem, Vector3 pos, Vector3 centerPos)
    {
        _rtItem = rtItem;
        transform.position = pos;
        _loadLanguage.loadLanguage();
        if (_rtItem._itemStatus == ItemStatus.UnEquip)
        {
            if (pos.x > centerPos.x)
            {
                _inventory_R.SetActive(false);
                _inventory_L.SetActive(true);
            }
            else
            {
                _inventory_R.SetActive(true);
                _inventory_L.SetActive(false);
            }
        }
    }
}

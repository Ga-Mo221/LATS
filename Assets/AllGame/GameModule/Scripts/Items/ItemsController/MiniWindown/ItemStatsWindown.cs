using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemStatsWindown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemStatsText;
    [SerializeField] private Slider _durabilitySlider;

    void Start()
    {
        if (!_itemStatsText)
            Debug.LogError("[ItemStatsWindown] Chưa gán 'TextMeshProUGUI _itemStatsText'");
        if (!_durabilitySlider)
            Debug.LogError("[ItemStatsWindown] Chưa gán 'Slider _durabilitySlider'");
    }


    public void ShowItemStats(String itemStats, RtItem _rtItem, Vector3 pos)
    {
        transform.position = pos;
        _itemStatsText.text = itemStats;
        float _intDurability = _rtItem._currentDurability;
        _durabilitySlider.maxValue = _rtItem._baseItem._maxDurability;
        _durabilitySlider.value = _intDurability;
    }
}

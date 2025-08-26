using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;

public class ResetButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button myButton;
    [SerializeField] private bool _changeColorText = false;
    [ShowIf(nameof(_changeColorText))]
    [SerializeField] private TextMeshProUGUI _buttonText;
    [ShowIf(nameof(_changeColorText))]
    [SerializeField] private Color _normalColor = Color.white;
    [ShowIf(nameof(_changeColorText))]
    [SerializeField] private Color _hoverColor = Color.black;

    void Awake()
    {
        myButton = gameObject.GetComponent<Button>();
        if (!myButton)
            Debug.LogError($"[{gameObject.name}] [RessetButton] Không lấy được 'Component Button'");

        if (_changeColorText)
            if (!_buttonText)
                Debug.LogError($"[{gameObject.name}] [RessetButton] Không lấy được 'TextMeshProUGUI'");
    }


    public void ResetButtonState()
    {
        // bỏ chọn button → trở về Normal
        if (EventSystem.current.currentSelectedGameObject == myButton.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (_changeColorText)
            {
                _buttonText.color = _normalColor;
            }
        }
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_changeColorText)
            _buttonText.color = _hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_changeColorText)
            _buttonText.color = _normalColor;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using NaughtyAttributes;

[RequireComponent(typeof(Button))]
public class ButtonEffect : MonoBehaviour, IPointerEnterHandler,
                            IPointerExitHandler, IPointerDownHandler,
                            IPointerUpHandler
{
    /* ----------  PUBLIC VARIABLES (điều chỉnh trong Inspector) ---------- */
    [SerializeField] private bool _effect = true;

    [ShowIf(nameof(_effect))]
    public GameObject _imgSelect;
    [ShowIf(nameof(_effect))]
    [Header("Fade Settings")]
    public float fadeDuration = 0.2f;     // Thời gian fade (s)
    [ShowIf(nameof(_effect))]
    public float scaleSize = 5f;

    /* ----------  PRIVATE VARIABLES ---------- */
    private Button _button;
    private TextMeshProUGUI _text;

    private bool isHovering = false;
    private bool isPressed = false;

    private float startSizeText;

    void Awake()
    {
        // button không phải là hình ảnh mà là một textmeshProUI có component Button
        _button  = GetComponent<Button>();

        if (!_effect) return;
        // Tìm Text con
        _text = GetComponent<TextMeshProUGUI>();
        if (_text == null)
            Debug.LogError("ButtonEffect: Không tìm thấy Text trong button.");
        else    startSizeText = _text.fontSize;
    }

    /* ----------  INTERFACE IMPLEMENTATION ---------- */

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (SoundManager.Instance != null)
            SoundManager.Instance.playButtonHover();

        if (_imgSelect != null)
            _imgSelect.SetActive(true);

        if (!_effect) return;
        StartCoroutine(FadeTextSize(scaleSize, fadeDuration)); // Fade out
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;

        if (_imgSelect != null)
            _imgSelect.SetActive(false);

        if (!_effect) return;
        StartCoroutine(FadeTextSize(- scaleSize, fadeDuration)); // Fade in
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        isPressed = true;
        if (SoundManager.Instance != null)
            SoundManager.Instance.playButtonPressed();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPressed) return;
        isPressed = false;
    }

    /* ----------  HELPER METHODS ---------- */
    /// <summary>
    /// Mượt dần thay đổi font size của TextMeshProUGUI.
    /// </summary>
    /// <param name="plusSize">Số điểm cần tăng (dương) hoặc giảm (âm).</param>
    /// <param name="duration">Thời gian thực hiện chuyển động (giây).</param>
    IEnumerator FadeTextSize(float plusSize, float duration)
    {
        // 1. Lấy giá trị bắt đầu và kết thúc
        float startSize = 0f;
        if (plusSize > 0)
            startSize = startSizeText;
        else    startSize = startSizeText - plusSize;

        float targetSize = Mathf.Max(0f, startSize + plusSize);   // Đảm bảo không âm

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;          // Dùng unscaled để vẫn hoạt động khi pause
            float t = Mathf.Clamp01(elapsed / duration);

            // Lerp giữa kích thước ban đầu và mục tiêu
            _text.fontSize = Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        // Đảm bảo đặt chính xác vào giá trị cuối cùng (để tránh lỗi rounding)
        _text.fontSize = targetSize;
    }
}

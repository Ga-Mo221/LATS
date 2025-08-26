using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public bool ShowGameObject = false;
    [ShowIf(nameof(ShowGameObject))]
    [SerializeField] private Sprite _imagePhysicDamage;
    [ShowIf(nameof(ShowGameObject))]
    [SerializeField] private Sprite _imageMagicDamage;
    [ShowIf(nameof(ShowGameObject))]
    [SerializeField] private Image _img;
    [ShowIf(nameof(ShowGameObject))]
    [SerializeField] private RectTransform _textTransform;
    [ShowIf(nameof(ShowGameObject))]
    [SerializeField] private TextMeshProUGUI _damageText;


    [Header("Start")]
    public bool _start = false;
    

    private Vector3 _moveSpeed = new Vector3(0, 5, 0);
    private float _timeToFace = 1f;

    private float _timeElapsed = 0f;
    private Color _startColor;

    private float _damage;

    void Start()
    {
        if (!_imagePhysicDamage)
            Debug.LogError("[DamageText] Chưa gán '_imagePhysicDamage'");
        if (!_imageMagicDamage)
            Debug.LogError("[DamageText] Chưa gán '_imageMagicDamage'");
        if (!_img)
            Debug.LogError("[DamageText] Chưa gán '_img'");
        if (!_textTransform)
            Debug.LogError("[DamageText] Chưa gán '_textTransform'");
        if (!_damageText)
            Debug.LogError("[DamageText] Chưa gán '_damageText'");
    }

    void Update()
    {
        if (_start)
        {
            _damageText.color = _startColor;
            _damageText.text = _damage.ToString();
            animText();
        }
    }

    private void animText()
    {
        _textTransform.position += _moveSpeed * Time.deltaTime;
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed < _timeToFace)
        {
            float _faceAlpha = _startColor.a * (1 - (_timeElapsed / _timeToFace));
            _damageText.color = new Color(_startColor.r, _startColor.g, _startColor.b, _faceAlpha);
        }
        else
        {
            _start = false;
            gameObject.SetActive(false);
        }
    }

    public void setDamage(float damage, bool magic, Vector3 pos)
    {
        _textTransform.position = pos;
        _damage = damage;
        _timeElapsed = 0f;
        _timeToFace = 1f;
        if (!magic)
        {
            _startColor = new Color(217f / 255f, 7f / 255f, 7f / 255f, 255f / 255f); // đỏ
            _img.sprite = _imagePhysicDamage;
        }
        else
        {
            _startColor = new Color(197f / 255f, 8f / 255f, 231f / 255f, 255f / 255f); // tím
            _img.sprite = _imageMagicDamage;
        }
    }
}

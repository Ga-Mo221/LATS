using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] private Image _panel;
    [SerializeField] private Image _logo;
    [SerializeField] TextMeshProUGUI _teamName_R1;
    [SerializeField] TextMeshProUGUI _teamName_R2;
    [SerializeField] TextMeshProUGUI _teamName_L1;
    [SerializeField] TextMeshProUGUI _teamName_L2;
    [SerializeField] GameObject _load;


    private bool _ok = false;


    private float _timeElapsed = 0f;
    private float _timeToFace = 2f;
    private int _loop = 0;


    private bool _start = true;
    private bool _end = false;
    private bool _resetcolor = true;
    private bool _displaylogo = true;


    void Start()
    {
        if (!_panel)
            Debug.Log("[Intro] Chưa gán 'Image _panel'");

        if (!_load)
            Debug.Log("[Intro] Chưa gán 'Image _logo'");

        if (!_teamName_R1)
            Debug.Log("[Intro] Chưa gán 'TextMeshProUGUI _teamName_R1'");

        if (!_teamName_R2)
            Debug.Log("[Intro] Chưa gán 'TextMeshProUGUI _teamName_R2'");

        if (!_teamName_L1)
            Debug.Log("[Intro] Chưa gán 'TextMeshProUGUI _teamName_L1'");

        if (!_teamName_L2)
            Debug.Log("[Intro] Chưa gán 'TextMeshProUGUI _teamName_L2'");

        if (!_load)
            Debug.Log("[Intro] Chưa gán 'GameObject'");

        StartCoroutine(startIntro());
    }


    private IEnumerator startIntro()
    {
        yield return new WaitForSeconds(1.5f);
        _ok = true;
    }


    void Update()
    {
        if (_ok)
        {
            if (!_end)
                loadLogo();
            else
                resetColor();
        }

        if (_loop == 3)
        {
            _load.SetActive(false);
            _loop += 1;
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayMainMenuSound();
        }
        else if (_loop > 3)
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed < _timeToFace)
            {
                float _faceAlpha = 1f - Mathf.Clamp01(_timeElapsed / _timeToFace); // từ 1 -> 0
                _panel.color = new Color(_panel.color.r, _panel.color.g, _panel.color.b, _faceAlpha);
            }
            else
            {
                GameManager.Instance._canOpenWindown = true;
                gameObject.SetActive(false);
            }
        }
    }


    private void loadLogo()
    {
        GameManager.Instance._canOpenWindown = false;
        resetColor();
        if (_start)
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed < _timeToFace)
            {
                float _faceAlpha = Mathf.Clamp01(_timeElapsed / _timeToFace); // từ 0 -> 1
                _logo.color = new Color(_logo.color.r, _logo.color.g, _logo.color.b, _faceAlpha);
                _teamName_R1.color = new Color(_teamName_R1.color.r, _teamName_R1.color.g, _teamName_R1.color.b, _faceAlpha);
                _teamName_L1.color = new Color(_teamName_L1.color.r, _teamName_L1.color.g, _teamName_L1.color.b, _faceAlpha);
                _teamName_R2.color = new Color(_teamName_R2.color.r, _teamName_R2.color.g, _teamName_R2.color.b, _faceAlpha);
                _teamName_L2.color = new Color(_teamName_L2.color.r, _teamName_L2.color.g, _teamName_L2.color.b, _faceAlpha);
            }
            else
            {
                if (_displaylogo)
                    StartCoroutine(displayLogo());
            }
        }
        else
        {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed < _timeToFace)
            {
                float _faceAlpha = 1f - Mathf.Clamp01(_timeElapsed / _timeToFace); // từ 1 -> 0
                _logo.color = new Color(_logo.color.r, _logo.color.g, _logo.color.b, _faceAlpha);
                _teamName_R1.color = new Color(_teamName_R1.color.r, _teamName_R1.color.g, _teamName_R1.color.b, _faceAlpha);
                _teamName_L1.color = new Color(_teamName_L1.color.r, _teamName_L1.color.g, _teamName_L1.color.b, _faceAlpha);
                _teamName_R2.color = new Color(_teamName_R2.color.r, _teamName_R2.color.g, _teamName_R2.color.b, _faceAlpha);
                _teamName_L2.color = new Color(_teamName_L2.color.r, _teamName_L2.color.g, _teamName_L2.color.b, _faceAlpha);
            }
            else
            {
                _timeElapsed = 0f;
                _end = true;
                _resetcolor = true;
                _load.SetActive(true);
            }
        }
    }


    public void resetColor()
    {
        if (_resetcolor)
        {
            _resetcolor = false;
            _logo.color = new Color(_logo.color.r, _logo.color.g, _logo.color.b, 0);
            _teamName_R1.color = new Color(_teamName_R1.color.r, _teamName_R1.color.g, _teamName_R1.color.b, 0);
            _teamName_R2.color = new Color(_teamName_R2.color.r, _teamName_R2.color.g, _teamName_R2.color.b, 0);
            _teamName_L1.color = new Color(_teamName_L1.color.r, _teamName_L1.color.g, _teamName_L1.color.b, 0);
            _teamName_L2.color = new Color(_teamName_L2.color.r, _teamName_L2.color.g, _teamName_L2.color.b, 0);
        }
    }


    private IEnumerator displayLogo()
    {
        _displaylogo = false;
        if (SoundManager.Instance != null)
            SoundManager.Instance.playLogoSound();
        yield return new WaitForSeconds(2f);
        _start = false;
        _timeElapsed = 0f;
    }

    public void addLoop()
    {
        _loop += 1;
    }
}

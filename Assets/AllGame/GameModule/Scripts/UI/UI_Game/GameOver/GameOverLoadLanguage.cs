using TMPro;
using UnityEngine;

public class GameOverLoadLanguage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameOver;
    [SerializeField] private TextMeshProUGUI _continue;
    [SerializeField] private TextMeshProUGUI _quitMenu;


    void Start()
    {
        loadLanguage();
    }


    public void loadLanguage()
    {
        if (!debug()) return;
        LocalizationManager loc = new LocalizationManager();
        loc.LoadLocalization(SettingManager.Instance.CurrentSettings.language.ToString(), "gameOver");

        _gameOver.text = loc.GetLocalizedValue("gameover");
        _continue.text = loc.GetLocalizedValue("continue");
        _quitMenu.text = loc.GetLocalizedValue("exit");
    }


    private bool debug()
    {
        if (!_gameOver)
        {
            Debug.LogError("[GameOverLoadLanguage] Chưa gán '_gameOver'");
            return false;
        }

        if (!_continue)
        {
            Debug.LogError("[GameOverLoadLanguage] Chưa gán '_continue'");
            return false;
        }

        if (!_quitMenu)
        {
            Debug.LogError("[GameOverLoadLanguage] Chưa gán '_quitMenu'");
            return false;
        }

        return true;
    }
}

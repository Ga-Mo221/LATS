using UnityEngine;
using System.Collections.Generic;

public class LocalizationManager
{
    private Dictionary<string, string> localizedText;
    public string currentLanguage = "VI";

    public void LoadLocalization(string languageCode, string model)
    {
        localizedText = new Dictionary<string, string>();

        TextAsset jsonFile = Resources.Load<TextAsset>($"Localization/{languageCode}");
        if (jsonFile == null)
        {
            Debug.LogError($"Không tìm thấy file ngôn ngữ: {languageCode}.json");
            return;
        }

        LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonFile.text);

        switch (model)
        {
            case "Setting":
                foreach (var entry in data.setting)
                {
                    localizedText[entry.key] = entry.value;
                }
                break;
            case "MainMenu":
                foreach (var entry in data.menu)
                {
                    localizedText[entry.key] = entry.value;
                }
                break;
            case "gameOption":
                foreach (var entry in data.gameOption)
                {
                    localizedText[entry.key] = entry.value;
                }
                break;
            case "gameOver":
                foreach (var entry in data.gameOver)
                {
                    localizedText[entry.key] = entry.value;
                }
                break;
            case "itemButton":
                foreach (var entry in data.itemButton)
                {
                    localizedText[entry.key] = entry.value;
                }
                break;
            case "itemStats":
                foreach (var entry in data.itemStats)
                {
                    localizedText[entry.key] = entry.value;
                }
                break;
        }
        

        currentLanguage = languageCode;
        //Debug.Log($"🌐 Đã tải ngôn ngữ: {languageCode}, tổng số: {localizedText.Count}");
    }

    public string GetLocalizedValue(string key)
    {
        if (localizedText.ContainsKey(key))
            return localizedText[key];
        return $"#{key}";
    }
}

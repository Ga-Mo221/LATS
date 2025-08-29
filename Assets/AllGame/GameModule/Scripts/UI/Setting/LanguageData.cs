using System.Collections.Generic;

[System.Serializable]
public class LocalizationData
{
    public List<LocalizedText> setting;
    public List<LocalizedText> menu;
    public List<LocalizedText> gameOption;
    public List<LocalizedText> gameOver;
    public List<LocalizedText> itemButton;
    public List<LocalizedText> itemStats;
}

[System.Serializable]
public class LocalizedText
{
    public string key;
    public string value;
}

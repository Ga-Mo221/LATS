using System.Collections.Generic;

[System.Serializable]
public class LocalizationData
{
    public List<LocalizedText> setting;
    public List<LocalizedText> menu;
}

[System.Serializable]
public class LocalizedText
{
    public string key;
    public string value;
}

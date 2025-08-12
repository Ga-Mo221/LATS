[System.Serializable]
public enum Language
{
    EN,
    VI
}


[System.Serializable]
public class SettingData
{
    public Language language;
    public bool isFullscreen;
    public int resolutionWidth;
    public int resolutionHeight;
    public int targetFPS;

    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;

    public override string ToString()
    {
        return $"[SettingData]\n" +
               $"- Language: {language}\n" +
               $"- Fullscreen: {isFullscreen}\n" +
               $"- Resolution: {resolutionWidth}x{resolutionHeight}\n" +
               $"- Target FPS: {targetFPS}\n" +
               $"- Master Volume: {masterVolume:F2}\n" +
               $"- SFX Volume: {sfxVolume:F2}\n" +
               $"- Music Volume: {musicVolume:F2}";
    }
}

[System.Serializable]
public class SettingDatatam
{
    public Language language;
    public bool isFullscreen;
    public int resolutionWidth;
    public int resolutionHeight;
    public int targetFPS;

    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;

    public override string ToString()
    {
        return $"[SettingData]\n" +
               $"- Language: {language}\n" +
               $"- Fullscreen: {isFullscreen}\n" +
               $"- Resolution: {resolutionWidth}x{resolutionHeight}\n" +
               $"- Target FPS: {targetFPS}\n" +
               $"- Master Volume: {masterVolume:F2}\n" +
               $"- SFX Volume: {sfxVolume:F2}\n" +
               $"- Music Volume: {musicVolume:F2}";
    }
}
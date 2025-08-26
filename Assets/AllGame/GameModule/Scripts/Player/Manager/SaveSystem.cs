using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "player_save.json");

    public static void SavePlayer(PlayerStarts stats)
    {
        string json = JsonUtility.ToJson(stats, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("[SaveSystem] Đã lưu dữ liệu vào: " + SavePath);
    }

    public static PlayerStarts LoadPlayer()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("[SaveSystem] Không tìm thấy file save.");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        PlayerStarts stats = JsonUtility.FromJson<PlayerStarts>(json);
        return stats;
    }

    public static void DeleteSave()
    {
        Debug.Log("[SaveSystem] Đã Xóa File Cũ");
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }

    public static PlayerStarts createNewStats()
    {
        return new PlayerStarts
        {
            _StartPoint = new Vector3(0,0,0),

            // Level
            _level = 1,

            // Exp
            _currentExp = 0f,
            _requiredExp = 100f,

            // Mạng sống
            _lifeCount = 3,
            _currentLifeCount = 3,

            // Mana 
            _maxMana = 100,
            _currentMana = 0,

            // HP
            _maxHealth = 100f,
            _currentHealth = 100f,

            // Stamina
            _stamina = 200f,

            // Move speed
            _walkSpeed = 10f,
            _runSpeed = 20f,

            // Nhảy
            _jumpForce = 30f,

            // Damage
            _physicalDamage = 10f,
            _magicDamage = 5f,

            // Giáp
            _armor = 10f,
            _magicResist = 5f,

            // Dash
            _dashPower = 40f,
            _dashingCooldown = 1f,

            // Attack speed (%)
            _attackSpeed = 100f,

            // Delay (%)
            _delay = 0f,

            // Tỉ lệ chí mạng
            _critChancePhysical = 0f,
            _critChanceMagic = 0f,
            _critMultiplier = 1.5f,

            // Giảm hồi chiêu (%)
            _cooldownReduction = 0,

            // Tiền tệ
            _xeng = 0,
            _linhAn = 0,

            // Hướng dẫn
            _tutorialRun = true,
            _tutorialJump = true,
            _tutorialSit = true,
            _tutorialAttack = true,
            _tutorialDash = true,

            // Kỹ năng đã mở
            _doubleJump = true,
            _skillQ = false,
            _skillW = false,
            _skillE = false
        };
    }
}

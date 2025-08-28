using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerManager : MonoBehaviour
{
    #region Singler Ton
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        // Đảm bảo chỉ có một PlayerManager tồn tại trong game
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Huỷ đối tượng trùng lặp
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại object này khi chuyển scene

        loadPlayerStart();
    }
    #endregion


    #region Player Profile
    [Header("GameObject")]
    [SerializeField] public PlayerHealth _playerHealth;
    [SerializeField] public PlayerRada _rada;
    [SerializeField] public PlayerAnimation _anim;
    public GameObject _player { get; set; }

    [Header("Player Stats")]
    public PlayerStarts _start = new PlayerStarts();

    private PlayerState _currentState = PlayerState.Idle;

    // player state
    //private bool _canMoveAttack = false;
    private bool _isDetected = false;
    private bool _isAlive = true; // còn sống
    private bool _knocked = false; // bị trúng đòn
    // canattack sẽ chạy ở hàm update để kiểm tra trong AnimatorController có weapon type = 0 hay không?
    private bool _canAttack = true; // cho những trường hợp đặc biệt hoặc không có vũ khí
    private float _dashTime = 0f;
    private float _stamina = 0f;
    private bool _isAttacking = false;

    public int _enemyHitBoxDirection { get; set; }
    public Vector3 _respawnPoint { get; set; }
    #endregion


    void Start()
    {
        if (!_playerHealth)
            Debug.LogError("[PlayerManager] Chưa gán '_playerHealth'");

        if (!_rada)
            Debug.LogError("[PlayerManager] Chưa gán '_rada'");

        if (!_anim)
            Debug.LogError("[PlayerManager] Chưa gán 'PlayerAnimation'");
    }


    #region Save Player Profile
    public void savePlayer()
    {
        SaveSystem.SavePlayer(_start);
    }
    #endregion


    #region Load Player Start Or Create New
    public void loadPlayerStart()
    {
        PlayerStarts loaded = SaveSystem.LoadPlayer();
        if (loaded != null)
        {
            _start = loaded;
            Debug.Log("[PlayerManager] Đã load dữ liệu cũ");

            transform.position = _start._StartPoint;
            _isAlive = true;
            _knocked = false;
            _isDetected = false;
            _stamina = _start._stamina;
            _dashTime = _start._dashingCooldown;
        }
        else
        {
            createNewPlayerStats();
        }
    }
    #endregion


    #region Create New Player Start
    public void createNewPlayerStats()
    {
        _start = SaveSystem.createNewStats(); // Khi không có save file
        //SavePlayer();
        transform.position = _start._StartPoint;
        _isAlive = true;
        _knocked = false;
        _stamina = _start._stamina;
        _dashTime = _start._dashingCooldown;
        Debug.Log($"[PlayerManager] Tạo mới nhân vật tại vị trí : \n _start: {_start._StartPoint}\n transform: {transform.position}");
    }
    #endregion


    #region  ResetGame
    public void resetGame() // gọi khi chọn "Chơi mới" hoặc "Chơi Lại"
    {
        SaveSystem.DeleteSave();
        createNewPlayerStats();
        savePlayer();
    }
    #endregion


    #region Respwan Point
    // set vị trí trước khi chết để có thể quay lại vị trí đó
    public void setRespawnPoint(Vector3 pos)
    {
        _respawnPoint = pos;
    }
    #endregion


    #region  Set Mana
    // tăng mana hiện tại lên, nếu tăng thì add = true
    public void setMana(float mana, bool add)
    {
        if (add && _start._currentMana < _start._maxMana)
        {
            _start._currentMana += mana;
            if (_start._currentMana > _start._maxMana)
                _start._currentMana = _start._maxMana;
        }
        else if (!add && _start._currentMana > 0)
        {
            _start._currentMana -= mana;
            if (_start._currentMana < 0)
                _start._currentMana = 0;
        }
    }
    #endregion


    #region Can Dash
    public bool dash() // trừ thể lực mỗi khi dash
    {
        if (_stamina >= 40)
        {
            _stamina -= 40;
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion


    #region set Coin
    public bool setCoin(int value, bool add) // add = true là bán và nhặt được xèng, add = false là mua đồ
    {
        if (add)
        {
            _start._xeng += value;
            return false;
        }
        else
        {
            if (_start._xeng >= value)
            {
                _start._xeng -= value;
                return true;
            }
            return false;
        }
    }
    #endregion


    #region Current State
    public void setCurrentState(PlayerState _state) => _currentState = _state;

    public PlayerState getCurrentState() => _currentState;
    #endregion


    #region IsAlive
    public void setIsAlive(bool islive) => _isAlive = islive;

    public bool getIsAlive() => _isAlive;
    #endregion


    #region Knocked
    public void setKnoked(bool knocked) => _knocked = knocked;

    public bool getKnocked() => _knocked;
    #endregion


    #region Stamina
    public void setStamina(float amount) => _stamina = amount;

    // add = true là cộng, add = false là trừ
    public void updateStamina(float amount, bool add)
    {
        _stamina = add ? _stamina + amount : _stamina - amount;
    }

    public float getStamina() => _stamina;
    #endregion


    #region Dash Time
    public void setCanDashTime(float amount) => _dashTime = amount;

    public float getDashTime() => _dashTime;
    #endregion


    #region CanAttack
    public void setCanAttack(bool amount) => _canAttack = amount;

    public bool getCanAttack() => _canAttack;
    #endregion


    #region Attacking
    public void setAttacking(bool amount) => _isAttacking = amount;

    public bool getAttacking() => _isAttacking;
    #endregion


    #region Set Attack Speed
    public void setAttackSpeed()
    {
        float _attackSpeed = _start.getAttackSpeed();
        _anim.setFloadAttackSpeed(_attackSpeed);
    }
    #endregion


    #region Player Detected
    public void setPlayerDetected(bool amount) => _isDetected = amount;

    public bool getPlayerDetected() => _isDetected; 
    #endregion


    #region Destroy Player
    public void destroyPlayer()
        => Destroy(gameObject);
    #endregion
}

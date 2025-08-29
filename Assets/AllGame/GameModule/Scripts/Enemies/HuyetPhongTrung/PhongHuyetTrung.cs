using UnityEngine;
using System.Collections;

public class PhongHuyetTrung : EnemyBase
{
    #region Variable
    [Header("Explosion Settings")]
    [SerializeField] private float _countdownDuration = 8f;
    [SerializeField] private float _maxMoveSpeedDuringCountdown = 10f;
    [SerializeField] private float _speedMultiplierCurve = 2f;

    // Explosion logic
    private bool _isExploding = false;
    private bool _isPoisoning = false;
    private bool _isDead = false; //track trạng thái chết
    private float _countdownTimer = 0f;
    private float _originalMoveSpeed;
    private float _originalRunSpeed;
    private GameObject _rememberedPlayer; // Player được nhớ để tiếp tục chase dù ra khỏi tầm
    private bool _hasStartedCountdown = false;
    
    // Thêm reference đến Poison component để control
    private Poison _poisonComponent;
    #endregion

    #region Enemy Start
    protected override void Awake()
    {
        base.Awake();
        _canFly = true;

        // Lưu tốc độ gốc
        _originalMoveSpeed = _enemyMoveSpd;
        _originalRunSpeed = _enemyRunSpd;
        
        // Lấy reference đến Poison component
        _poisonComponent = GetComponent<Poison>();
    }

    protected override void Start()
    {
        base.Start();
        _physicalRes = 0;
        _magicRes = 0;
    }

    protected override void Update()
    {
        if (_isExploding || _isPoisoning) 
            return;

        base.Update();
        
        // Xử lý remembered player logic
        handleRememberedPlayer();
        
        if (_isChasing && (_player != null || _rememberedPlayer != null))
        {
            handleCountdown();
        }
        
        updateAnimator();
    }

    protected override void FixedUpdate()
    {
        // Nếu đang exploding hoặc poisoning thì dừng movement
        if (_isExploding || _isPoisoning)
        {
            if (_rb != null)
            {
                _rb.linearVelocity = Vector2.zero;
                _isMoving = false;
            }
            return;
        }
        base.FixedUpdate();
    }
    #endregion

    #region Remembered Player Logic
    /// <summary>
    /// Xử lý logic nhớ player - khi phát hiện player lần đầu sẽ nhớ và chase dù ra khỏi tầm
    /// </summary>
    private void handleRememberedPlayer()
    {
        // Nếu detect được player mà chưa có remembered player -> bắt đầu nhớ
        if (_player != null && _rememberedPlayer == null)
        {
            _rememberedPlayer = _player;
        }
        
        // Nếu có remembered player nhưng không detect player hiện tại -> dùng remembered player
        if (_rememberedPlayer != null && _player == null)
        {
            _player = _rememberedPlayer; // Temporary assign để chase logic hoạt động
        }
    }
    #endregion

    #region Countdown and Explosion Logic
    private void handleCountdown()
    {
        // Bắt đầu countdown khi lần đầu detect player
        if (!_hasStartedCountdown)
        {
            _hasStartedCountdown = true;
            _countdownTimer = _countdownDuration;
            // Đảm bảo remembered player được set
            if (_rememberedPlayer == null && _player != null)
                _rememberedPlayer = _player;
        }

        // Cập nhật countdown
        _countdownTimer = Mathf.Max(0f, _countdownTimer - Time.deltaTime);
        updateChaseSpeed();

        // Kiểm tra khoảng cách để nổ
        if (_rememberedPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, _rememberedPlayer.transform.position);
            if (distance <= _enemyAttackRange)
            {
                triggerExplosion();
            }
        }
    }

    private void updateChaseSpeed()
    {
        // Tốc độ tăng dần theo thời gian
        float timeProgress = Mathf.Clamp01(1f - (_countdownTimer / _countdownDuration));
        float curvedProgress = Mathf.Pow(timeProgress, _speedMultiplierCurve);
        float newSpeed = Mathf.Lerp(_originalRunSpeed, _maxMoveSpeedDuringCountdown, curvedProgress);
        _enemyRunSpd = newSpeed;
    }

    private void triggerExplosion()
    {
        if (_isExploding) return;
        
        _isExploding = true;
        
        // Dừng movement
        if (_rb != null)
            _rb.linearVelocity = Vector2.zero;

        // Clear path để dừng hoàn toàn
        clearPathCompletely();

        // Trigger explosion animation
        if (_animator != null)
        {
            _animator.SetBool("isPatrol", false);
            _animator.SetBool("isStay", false);
            _animator.SetTrigger("isExplode");
        }
    }

    #endregion

    #region Override Movement để handle explosion state

    protected override bool ShouldStopMovement()
    {
        return _isExploding || _isPoisoning || base.ShouldStopMovement();
    }

    protected override void moveToPlayer()
    {
        if (_isExploding || _isPoisoning) return;
        
        // Sử dụng remembered player nếu có để tiếp tục chase ngay cả khi mất detection
        GameObject targetPlayer = _rememberedPlayer != null ? _rememberedPlayer : _player;
        if (targetPlayer != null)
        {
            GameObject originalPlayer = _player;
            _player = targetPlayer;
            base.moveToPlayer();
            _player = originalPlayer; // Restore lại sau khi xử lý
        }
        else
        {
            base.moveToPlayer();
        }
    }

    protected override void Patrol()
    {
        if (_isExploding || _isPoisoning) return;
        
        // Reset về tốc độ patrol bình thường
        _enemyMoveSpd = _originalMoveSpeed;
        _enemyRunSpd = _originalRunSpeed;
        base.Patrol();
    }

    #endregion

    #region Animation Events

    // Animation Event được gọi trong animation Explode để gây damage
    public void onExplodeDamageEvent()
    {
        if (!_isExploding) return;

        if (_rememberedPlayer != null)
        {
            float distance = Vector3.Distance(transform.position, _rememberedPlayer.transform.position);
            if (distance <= _enemyAttackRange)
            {
                Attack();
            }
        }
    }

    // Animation Event được gọi khi animation Explode kết thúc
    public void onExplodeAnimationComplete()
    {
        // Kiểm tra xem enemy có bị chết không
        if (!_isExploding || _isDead) 
        {
            return;
        }
        
        _isPoisoning = true;
        
        // Chuyển sang animation Poison
        if (_animator != null)
            _animator.SetBool("isDead", true);
        
        // Kích hoạt poison effect
        if (_poisonComponent != null)
        {
            _poisonComponent.startPoisonEffect();
        }
        else
        {
            Die();
        }
    }
    
    public void onPoisonAnimationComplete()
    { }
    #endregion

    #region Attack and TakeDamage

    protected override void Attack()
    {
        if (_rememberedPlayer == null) return;

        PlayerHealth health = _rememberedPlayer.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.takeDamage(0, _enemyPhysicalDamage, false);
            Debug.Log($"Explosion dealt {_enemyPhysicalDamage} physical damage");
        }
    }

    public override void takeDamage()
    {
        // Cho phép nhận damage trong lúc exploding để có thể chết sớm
        if (_isPoisoning)
        {
            // Không nhận damage khi đang poison (đã chết rồi)
            return;
        }
        
        // Nếu đang exploding thì force Die luôn để cleanup
        if (_isExploding)
        {
            forceCleanupAndDie();
            return;
        }
        
        base.takeDamage();
    }

    /// <summary>
    /// Method mới để force cleanup khi bị giết trong lúc exploding
    /// </summary>
    private void forceCleanupAndDie()
    {
        // Đánh dấu đã chết để ngăn animation events
        _isDead = true;
        
        // Dừng poison nếu đang active
        if (_poisonComponent != null)
        {
            _poisonComponent.forceStop();
        }
        
        // Reset states
        _isExploding = false;
        _isPoisoning = false;
        
        // Force reset animator về trạng thái normal trước khi Die
        if (_animator != null)
        {
            _animator.SetBool("isDead", false);
            _animator.SetBool("isPatrol", false);
            _animator.SetBool("isStay", false);
            _animator.ResetTrigger("isExplode");
        }
        
        // Gọi Die từ base class
        base.takeDamage(); // Sẽ gọi knockback và có thể trigger Die nếu hết HP
    }
    #endregion

    #region Die and Respawn

    public override void Die()
    {
        // Đánh dấu đã chết
        _isDead = true;
        
        // Force stop poison effect khi chết
        if (_poisonComponent != null)
        {
            _poisonComponent.forceStop();
        }
        
        // Reset tất cả states
        _isExploding = false;
        _isPoisoning = false;

        base.Die();
    }

    public override void respawn()
    {
        // Reset flag chết
        _isDead = false;
        
        // Force stop poison effect trước khi respawn
        if (_poisonComponent != null)
        {
            _poisonComponent.forceStop();
        }
        
        // Reset tất cả explosion states
        _isExploding = false;
        _isPoisoning = false;
        _hasStartedCountdown = false;
        _countdownTimer = 0f;
        _rememberedPlayer = null;

        // Restore tốc độ gốc
        _enemyMoveSpd = _originalMoveSpeed;
        _enemyRunSpd = _originalRunSpeed;
        
        // Đảm bảo rigidbody không bị freeze
        if (_rb != null)
            _rb.constraints = RigidbodyConstraints2D.None;

        base.respawn();

        // Force reset animator về idle state
        if (_animator != null)
        {
            _animator.SetBool("isDead", false);
            _animator.SetBool("isPatrol", true);
            _animator.SetBool("isStay", false);
            // Reset trigger states
            _animator.ResetTrigger("isExplode");
            
            // Force play idle animation
            _animator.Play("Idel", -1, 0f);
        }
    }

    #endregion

    #region Animation

    private void updateAnimator()
    {
        if (_animator == null || _isExploding || _isPoisoning)
            return;

        // Sử dụng các biến từ base class
        _animator.SetBool("isPatrol", _isPatrol && !_isChasing);
        _animator.SetBool("isStay", _isStay);
    }

    #endregion
}
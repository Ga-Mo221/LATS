using System.Collections;
using UnityEngine;

public class LeThuyNhan : EnemyBase
{
    #region Variable
    [Header("LeThuyNhan Settings")]
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _attackAnimationDuration = 0.8f; // Thời gian animation attack
    [SerializeField] private float _hurtAnimationDuration = 0.5f;   // Thời gian animation hurt

    // State management
    private bool _isAttacking = false;
    private bool _isHurt = false;
    private bool _canAttack = true;
    private bool _isLive = true;
    private bool _isInAttackCooldown = false;
    
    // Timers
    private float _stateTimer = 0f;
    private Coroutine _currentStateCoroutine;
    
    // Animation Manager
    private LeThuyNhan_AnimationManager _animationManager;
    #endregion

    #region Enemy Start
    protected override void Awake()
    {
        base.Awake();
        _canFly = false;
        
        // Lấy reference đến Animation Manager
        _animationManager = GetComponent<LeThuyNhan_AnimationManager>();
        if (_animationManager == null)
            Debug.LogError("[LeThuyNhan] Missing LeThuyNhan_AnimationManager!");
    }

    protected override void Update()
    {
        if (!_isLive) return;
        
        // GỌI base.Update() TRƯỚC để đảm bảo detectPlayer() chạy
        base.Update();
        
        UpdateStateTimer();
        CheckForAttack();
        UpdateAnimations();
    }

    protected override void FixedUpdate()
    {
        if (!_isLive) return;

        // Nếu đang trong trạng thái cố định (attack/hurt), dừng movement
        if (ShouldStopMovement())
        {
            StopMovement();
            return;
        }

        // GỌI base.FixedUpdate() để xử lý movement
        base.FixedUpdate();
    }
    #endregion

    #region State Management
    protected override bool ShouldStopMovement()
    {
        return _isAttacking || _isHurt || _isKnockedBack;
    }
    
    private void StopMovement()
    {
        if (_rb != null)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            _isMoving = false;
        }
    }
        
    private void UpdateStateTimer()
    {
        if (_stateTimer > 0f)
        {
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0f)
            {
                if (_isAttacking) EndAttack();
                else if (_isHurt) EndHurt();
            }
        }
    }
    #endregion

    #region Attack Logic
    private void CheckForAttack()
    {
        if (!CanInitiateAttack()) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
        if (distanceToPlayer <= _enemyAttackRange * 1.1f)
        {
            InitiateAttack();
        }
    }
    
    private bool CanInitiateAttack()
    {
        return _player != null && 
               _isLive && 
               _canAttack && 
               !_isAttacking && 
               !_isHurt && 
               !_isKnockedBack &&
               !_isInAttackCooldown;
    }
    
    private void InitiateAttack()
    {
        Debug.Log("[LeThuyNhan] Initiating attack");
        
        _isAttacking = true;
        _canAttack = false;
        _stateTimer = _attackAnimationDuration;
        
        if (_rb != null)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            _isMoving = false;
        }
        
        FacePlayer();
        
        // Sử dụng Animation Manager thay vì truy cập animator trực tiếp
        if (_animationManager != null)
            _animationManager.triggerAttackAnimation();

        if (_currentStateCoroutine != null)
            StopCoroutine(_currentStateCoroutine);
    }
    
    private void EndAttack()
    {
        Debug.Log("[LeThuyNhan] Ending attack");
        _isAttacking = false;
        _stateTimer = 0f;
        
        // Start attack cooldown
        StartAttackCooldown();
    }
    
    private void StartAttackCooldown()
    {
        _isInAttackCooldown = true;
        _currentStateCoroutine = StartCoroutine(AttackCooldownCoroutine());
    }
    
    private IEnumerator AttackCooldownCoroutine()
    {
        yield return new WaitForSeconds(_attackCooldown);
        if (_isLive)
        {
            _canAttack = true;
            _isInAttackCooldown = false;
            Debug.Log("[LeThuyNhan] Attack cooldown finished");
        }
        _currentStateCoroutine = null;
    }
    
    protected override void Attack()
    {
        if (!_isLive || _player == null) return;

        float distance = Vector2.Distance(transform.position, _player.transform.position);
        if (distance <= _enemyAttackRange * 1.2f)
        {
            PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.takeDamage(1, _enemyPhysicalDamage, false);
                Debug.Log($"LeThuyNhan deals {_enemyPhysicalDamage} damage to player (distance: {distance:F2})");
            }
        }
    }
    #endregion

    #region Movement Override
    protected override void moveToPlayer()
    {
        if (!_isLive || _player == null || ShouldStopMovement())
        {
            if (_rb != null)
            {
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
                _isMoving = false;
            }
            return;
        }

        // Sử dụng logic từ base class
        base.moveToPlayer();
    }

    protected override void Patrol()
    {
        if (!_isLive || ShouldStopMovement())
        {
            if (_rb != null)
            {
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
                _isMoving = false;
            }
            return;
        }

        // Sử dụng logic từ base class
        base.Patrol();
    }
    #endregion

    #region Take Damage
    public override void takeDamage()
    {
        if (!_isLive) return;
        
        Debug.Log("[LeThuyNhan] Taking damage");
        
        base.takeDamage();
        
        _isHurt = true;
        _stateTimer = _hurtAnimationDuration;
        
        // Nếu đang attack thì hủy attack
        if (_isAttacking)
        {
            _isAttacking = false;
        }
        
        if (_rb != null)
        {
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            _isMoving = false;
        }
        
        // Sử dụng Animation Manager
        if (_animationManager != null)
            _animationManager.triggerHurtAnimation();
        
        // Chỉ stop coroutine attack, không stop cooldown
        if (_currentStateCoroutine != null && !_isInAttackCooldown)
        {
            StopCoroutine(_currentStateCoroutine);
            _currentStateCoroutine = null;
        }
    }
    
    private void EndHurt()
    {
        Debug.Log("[LeThuyNhan] Ending hurt state");
        _isHurt = false;
        _stateTimer = 0f;
        if (!_isInAttackCooldown)
            _canAttack = true;
    }
    #endregion

    #region Die and Respawn
    public override void Die()
    {
        if (!_isLive) return;

        Debug.Log("[LeThuyNhan] Dying");

        _isLive = false;
        _isAttacking = false;
        _isHurt = false;
        _canAttack = false;
        _isInAttackCooldown = false;
        _stateTimer = 0f;

        // Chỉ stop coroutine tấn công, không stop toàn bộ
        if (_currentStateCoroutine != null)
        {
            StopCoroutine(_currentStateCoroutine);
            _currentStateCoroutine = null;
        }

        // Sử dụng Animation Manager
        if (_animationManager != null)
            _animationManager.setDeadAnimation(true);

        base.Die();

        // Chest logic nếu có
        // Chest chest = gameObject.GetComponent<Chest>();
        // if (chest != null)
        // {
        //     chest.enemyDie(60f);
        // }
    }

    public override void respawn()
    {
        Debug.Log("[LeThuyNhan] Respawning");
        
        // Reset biến AI riêng của LeThuyNhan TRƯỚC KHI gọi base
        _isLive = true;
        _canAttack = true;
        _isAttacking = false;
        _isHurt = false;
        _isInAttackCooldown = false;
        _stateTimer = 0f;
        
        // Dọn dẹp coroutine cũ
        if (_currentStateCoroutine != null)
        {
            StopCoroutine(_currentStateCoroutine);
            _currentStateCoroutine = null;
        }
        
        // Gọi base respawn
        base.respawn();
        
        // Reset animations thông qua Animation Manager
        if (_animationManager != null)
        {
            _animationManager.resetAllAnimations();
        }
    }
    #endregion

    #region Animation and Visual
    private void UpdateAnimations()
    {
        if (_animationManager == null || !_isLive) return;
        
        // Cập nhật walking animation thông qua Animation Manager
        _animationManager.updateWalkingAnimation(_isMoving, _isAttacking, _isHurt, _isKnockedBack);
    }
    
    private void FacePlayer()
    {
        if (_player == null) return;
        float direction = _player.transform.position.x - transform.position.x;
        if (Mathf.Abs(direction) > 0.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction > 0 ? 1f : -1f;
            transform.localScale = scale;
        }
    }
    
    #endregion

    #region Animation Events (called from Animation Manager)
    
    /// <summary>
    /// Được gọi từ Animation Manager khi attack animation đến frame gây damage
    /// </summary>
    public void onAttackHit()
    {
        Attack();
    }
    
    /// <summary>
    /// Được gọi từ Animation Manager khi attack animation kết thúc
    /// </summary>
    public void onAttackFinished()
    {
        if (_isLive && _isAttacking)
        {
            EndAttack();
        }
    }
    
    /// <summary>
    /// Được gọi từ Animation Manager khi hurt animation kết thúc
    /// </summary>
    public void onHurtFinished()
    {
        if (_isLive && _isHurt)
        {
            EndHurt();
        }
    }
    
    #endregion
}
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool ShowGameObject = false;
    [ShowIf(nameof(ShowGameObject))]
    [SerializeField] private Rigidbody2D _rb;
    [ShowIf(nameof(ShowGameObject))]
    [SerializeField] private PlayerInput _playerInput;
    [ShowIf(nameof(ShowGameObject))]
    [SerializeField] private PlayerAnimation _anim;

    // check gounded
    private bool _isGround;
    private bool _canCheckGrounded = true;
    public bool ShowCheckGroundGameObject = false;
    [ShowIf(nameof(ShowCheckGroundGameObject))]
    [SerializeField] private LayerMask _groundLayer;
    [ShowIf(nameof(ShowCheckGroundGameObject))]
    [SerializeField] private Transform _groundCheck;
    [ShowIf(nameof(ShowCheckGroundGameObject))]
    [SerializeField] private Vector2 _boxSize = new Vector2(0.5f, 0.1f);


    // move
    private bool _canMove => _anim.getBoolCanMove();


    // Jump
    private int _jumpCount = 0;


    // Dash
    private bool _canDash = true;
    public bool _isDashing;
    private float _originalGravity;


    // Player Detected
    private Coroutine _resetDetected;

    void Start()
    {
        if (!_rb)
            Debug.LogError("[PlayerController] Chưa gán 'Rigibody2D'");
        if (!_playerInput)
            Debug.LogError("[PlayerController] Chưa gán 'PlayerInput'");
        if (!_groundCheck)
            Debug.LogError("[PlayerController] Chưa gán 'GroundCheck'");
        if (!_anim)
            Debug.LogError("[PlayerController] Chưa gán 'PlayerAnimation'");

        _originalGravity = _rb.gravityScale;
    }


    void Update()
    {
        updatePlayerLive();
        updateKnocked();
        updatePlayerStates();
        //debug();
        if (_canMove)
        {
            checkGrounded();
            if (_isDashing) return;
            handleJump();
            handleMove();
            handleSiting();
            handleDash();
        }
        handleAttack();

        if (PlayerManager.Instance.getIsAlive())
        {
            _anim.setBoolPlayerDetected(PlayerManager.Instance.getPlayerDetected());
            if (PlayerManager.Instance.getPlayerDetected())
            {
                if (_resetDetected != null)
                    StopCoroutine(_resetDetected);
                _resetDetected = StartCoroutine(resetDetectedState());
            }
        }
    }


    #region Reset Player Detected
    private IEnumerator resetDetectedState()
    {
        yield return new WaitForSeconds(3f);
        PlayerManager.Instance.setPlayerDetected(false);
    }
    #endregion


    #region Update Is A Live
    private void updatePlayerLive()
        => _anim.updateBoolIsALive(PlayerManager.Instance.getIsAlive());
    #endregion


    #region Update Knocked
    private void updateKnocked()
        => _anim.updateBoolKnocked(PlayerManager.Instance.getKnocked());
    #endregion


    #region Update Y Velocity
    void FixedUpdate()
        => _anim.setFloatYVelocity(_rb.linearVelocity.y);
    #endregion


    #region Check Grounded
    private void checkGrounded()
    {
        if (_canCheckGrounded)
            _isGround = Physics2D.OverlapBox(_groundCheck.position, _boxSize, 0f, _groundLayer);
        else _isGround = false;

        _anim.setBoolGround(_isGround);
        if (_isGround)
            _jumpCount = 0;
        _playerInput.setJumping(_isGround);

        if (PlayerManager.Instance.getKnocked()) return;
        if (_isGround && !_playerInput._isMoving && !_isDashing && !_playerInput._isJumping)
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }
    private IEnumerator resetCanCheckGrounded()
    {
        yield return new WaitForSeconds(0.8f);
        _canCheckGrounded = true;
    }
    void OnDrawGizmos()
    {
        if (_groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheck.position, _boxSize);
    }
    #endregion


    #region Handle Movevement
    private void handleMove()
    {
        float _moveSpeed = PlayerManager.Instance._start.GetMoveSpeed(_playerInput._isRunning);
        if (_playerInput._isDash) return;
        if (_playerInput._isSiting) _moveSpeed -= 0.5f;
        if (_playerInput._isJumping) _moveSpeed += 1;
        if (_playerInput._isRunning && PlayerManager.Instance.getStamina() > 0)
        {
            // Giảm statmina khi chạy
            PlayerManager.Instance.updateStamina(Time.deltaTime * 25, false);
        }
        if (_playerInput._isMoving)
        {
            _rb.linearVelocity = new Vector2(_playerInput._moveInput * _moveSpeed, _rb.linearVelocity.y);
        }
        _anim.setBoolIsMove(_playerInput._isMoving, _playerInput._isRunning);
        flip(_playerInput._moveInput);
    }
    #endregion


    #region Player Direction
    private void flip(float moveinput)
    {
        if (moveinput > 0.1) transform.localScale = new Vector3(1, 1, 1);
        else if (moveinput < -0.1) transform.localScale = new Vector3(-1, 1, 1);
    }
    #endregion


    #region Handle Jump
    private void handleJump()
    {
        bool _doubleJump = PlayerManager.Instance._start._doubleJump;

        if ((_playerInput._isJump && _isGround) || (_playerInput._isJump && _jumpCount < 1 && _doubleJump))
        {
            if (!_isGround) _jumpCount++;
            if (_isGround)
            {
                _canCheckGrounded = false;
                StartCoroutine(resetCanCheckGrounded());
            }
            _anim.setTriggerJumping();
        }
    }
    public void Jump()
    {
        // reset Velocity
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0);

        float _jumpForce = PlayerManager.Instance._start._jumpForce;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);

    }
    #endregion


    #region HendleSiting
    private void handleSiting()
        => _anim.setBoolSitting(_playerInput._isSiting);
    #endregion


    #region Handle Dash
    private void handleDash()
    {
        if (_playerInput._isDash && _canDash && _isGround && _canMove)
        {
            if (PlayerManager.Instance.dash())
                startDash();
        }
    }
    private void startDash()
    {
        _canDash = false;
        _isDashing = true;
        _rb.gravityScale = 0;
        _rb.linearVelocity = new Vector2(PlayerManager.Instance._start.getDashPower() * transform.localScale.x, 0f);
        _anim.setTriggerDashing();
    }
    public void resetDash()
    {
        _playerInput.resetDash();
        _rb.linearVelocity = new Vector2(0f, 0f);
        _rb.gravityScale = _originalGravity;
        _isDashing = false;
        float _time = PlayerManager.Instance._start.getDashingCooldown();

        StartCoroutine(resetCanDash(_time));
    }
    private IEnumerator resetCanDash(float amount)
    {
        float resetTime = 0;
        while (resetTime < amount)
        {
            resetTime += Time.deltaTime;
            float time = Mathf.Lerp(1f,0f,resetTime / amount);
            if (PlayerManager.Instance != null)
                PlayerManager.Instance.setDashTimeReset(time);
            yield return null;
        }
        if (PlayerManager.Instance != null)
            PlayerManager.Instance.setDashTimeReset(0);
        _canDash = true;
    }
    #endregion


    #region Handle Attack
    private void handleAttack()
    {
        int _weaponType = _anim.getIntegerWeaponType();
        if (_playerInput._isAttack && _weaponType != 0)
        {
            _anim.setTriggerAttack();
            if (_playerInput._isSiting)
                _playerInput.setIsSitting(false);
        }
    }
    #endregion


    #region Update Can Move
    public void canMoveTrue() => _anim.setBoolCanMove(true);
    public void canMoveFalse()
    {
        _rb.linearVelocity = Vector2.zero;
        _anim.setBoolCanMove(false);
    }
    #endregion


    #region Debug Player State
    private void debug()
    {
        if (_playerInput._isMoving)
        {
            if (_playerInput._isRunning)
                Debug.Log("[PlayerController] Đang chạy");
            else
                Debug.Log("[PlayerController] Đang đi");
        }
        if (_playerInput._isSiting) Debug.Log("[PlayerController] Đang ngồi");
        if (_isDashing) Debug.Log("[PlayerController] đang Dash");
        if (!_canMove) Debug.Log("[PlayerController] Đang Đánh");
        if (_isGround) Debug.Log("[PlayerController] Đang đứng trên mặt đất");
        else if (_playerInput._isJumping) Debug.Log("[PlayerController] Đang nhảy");
    }
    #endregion


    #region Update Player Sates
    private void updatePlayerStates()
    {
        if (PlayerManager.Instance.getIsAlive())
        {
            if (PlayerManager.Instance.getKnocked())
            {
                PlayerManager.Instance.setCurrentState(PlayerState.Knocked);
            }
            else
            {
                bool _isAttacking = PlayerManager.Instance.getAttacking();
                if (_isGround && _isAttacking)
                    PlayerManager.Instance.setCurrentState(PlayerState.Ground_Attack);
                else if (!_isGround && _isAttacking)
                    PlayerManager.Instance.setCurrentState(PlayerState.Air_Attack);
                else if (!_isGround)
                    PlayerManager.Instance.setCurrentState(PlayerState.Jump);
                else if (_isDashing)
                    PlayerManager.Instance.setCurrentState(PlayerState.Dash);
                else if (_playerInput._isRunning)
                    PlayerManager.Instance.setCurrentState(PlayerState.Run);
                else if (_playerInput._isMoving && _playerInput._isSiting)
                    PlayerManager.Instance.setCurrentState(PlayerState.Sit_Walk);
                else if (_playerInput._isMoving)
                    PlayerManager.Instance.setCurrentState(PlayerState.Walk);
                else if (_playerInput._isSiting)
                            PlayerManager.Instance.setCurrentState(PlayerState.Sit);
                        else
                            PlayerManager.Instance.setCurrentState(PlayerState.Idle);
            }
        }
        else
            PlayerManager.Instance.setCurrentState(PlayerState.Die);
    }
    #endregion
}

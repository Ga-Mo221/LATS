using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float _moveInput { get; private set; }
    public bool _isMoving { get; private set; }
    public bool _isRunning { get; private set; }
    public bool _isDash { get; private set; }
    public bool _isSiting { get; private set; }
    public bool _isJumping { get; private set; }
    public bool _isJump { get; private set; }
    public bool _isAttack { get; private set; }

    public float _owari_Combo_Attack_cooldown = 0.5f;

    private bool _isAlive;
    private bool _knocked;

    void Update()
    {
        _isAlive = PlayerManager.Instance.getIsAlive();
        _knocked = PlayerManager.Instance.getKnocked();

        if (_isAlive && !_knocked && GameManager.Instance._canOpenWindown)
        {
            if (PlayerManager.Instance._start._tutorialJump)
                handleJump();

            if (PlayerManager.Instance._start._tutorialSit)
                handleSitAndDash();

            if (PlayerManager.Instance._start._tutorialAttack)
                handleAttack();

            handleMove();
        }
        else
        {
            _isMoving = false;
            _isRunning = false;
            _isDash = false;
            _isSiting = false;
            _isJump = false;
            _isAttack = false;
        }
    }

    #region Move
    private float _lastDirection = 0;
    private bool _isrun = false;
    private Coroutine _resetIsRun;
    private bool _recoverStamina = false;
    private bool _startResetIsRun = true;

    private void handleMove()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
        _isMoving = _moveInput != 0;
        if (PlayerManager.Instance.getStamina() < 0f)
        {
            PlayerManager.Instance.setStamina(0f);
            _isRunning = false;
        }
        if (_isMoving && _isrun && _lastDirection != _moveInput && !_isSiting && !_isJumping)
        {
            if (_resetIsRun != null)
            {
                StopCoroutine(_resetIsRun);
                _resetIsRun = null;
            }
            if (PlayerManager.Instance.getStamina() > 0)
            {
                _isRunning = true;
            }
            _recoverStamina = false;
            _lastDirection = _moveInput;
            _startResetIsRun = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isSiting && _isMoving && !_isRunning && PlayerManager.Instance.getStamina() > 0 && PlayerManager.Instance._start._tutorialRun && !_isJumping)
        {
            _lastDirection = _moveInput;
            _isRunning = !_isRunning;
            _isrun = _isRunning;
        }
        if (!_isMoving && _startResetIsRun)
        {
            _isRunning = _isMoving;
            if (_resetIsRun != null)
            {
                StopCoroutine(_resetIsRun);
                _resetIsRun = null;
            }
            _startResetIsRun = false;
            _resetIsRun = StartCoroutine(resetisrun());
        }
        recoverStamina();
    }
    private IEnumerator resetisrun()
    {
        yield return new WaitForSeconds(0.2f);
        _isrun = false;
        _startResetIsRun = true;
        _recoverStamina = true;
    }
    private void recoverStamina()
    {
        if (!_isrun && _recoverStamina && PlayerManager.Instance.getStamina() < PlayerManager.Instance._start._stamina)
        {
            PlayerManager.Instance.updateStamina(Time.deltaTime * 20, true);
        }
    }
    #endregion


    #region Jump
    private void handleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (_isSiting) _isSiting = false;
            else
            {
                if (!_isDash)
                {
                    _isJump = true;
                    StartCoroutine(resetJump());
                }
            }
        }
    }
    private IEnumerator resetJump()
    {
        yield return new WaitForEndOfFrame();
        // #if UNITY_EDITOR
        //         yield return new WaitForEndOfFrame();
        // #endif
        _isJump = false;
    }
    public void setJumping(bool _isGrounded) => _isJumping = !_isGrounded;
    #endregion


    #region Handle Sit And Dash
    private Coroutine _resetDash;
    private void handleSitAndDash()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_isRunning && PlayerManager.Instance._start._tutorialDash)
            {
                _isDash = true;
                _isMoving = false;
                _isRunning = false;
                if (_resetDash != null)
                    StopCoroutine(_resetDash);
                _resetDash = StartCoroutine(resetDash());
            }
            else
            {
                if (!_isRunning)
                    _isSiting = !_isSiting;
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            _isSiting = false;
        }
    }
    public IEnumerator resetDash()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _isDash = false;
    }
    #endregion


    #region Attack
    public void handleAttack()
    {
        bool _canAttack = PlayerManager.Instance.getCanAttack();
        if (_canAttack && Input.GetMouseButtonDown(0))
        {
            _isAttack = true;
            StartCoroutine(resetAttack());
        }
    }
    private IEnumerator resetAttack()
    {
        yield return new WaitForEndOfFrame();
        //yield return new WaitForEndOfFrame();
        _isAttack = false;
    }
    public void setIsSitting(bool amount) => _isSiting = amount;
    public void setCanAttackTrue() => PlayerManager.Instance.setCanAttack(true);
    public void resetCanAttack() => StartCoroutine(resetCanAttackIE());
    private IEnumerator resetCanAttackIE()
    {
        yield return new WaitForSeconds(_owari_Combo_Attack_cooldown);
        PlayerManager.Instance.setCanAttack(true);
    }
    #endregion
}

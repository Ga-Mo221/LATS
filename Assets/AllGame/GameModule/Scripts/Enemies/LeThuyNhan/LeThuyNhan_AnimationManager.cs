using UnityEngine;

/// <summary>
/// Animation Manager cho LeThuyNhan enemy
/// Tách biệt logic animation để dễ quản lý và maintain
/// </summary>
public class LeThuyNhan_AnimationManager : MonoBehaviour
{
    [Header("Animation Manager")]
    private Animator _animator;
    
    // Cache animation states để tránh set liên tục
    private bool _lastWalkingState = false;

    #region Initialization
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
            Debug.LogError("[LeThuyNhan_AnimationManager] Animator component not found!");
    }
    #endregion

    #region Public Animation Control Methods
    
    /// <summary>
    /// Cập nhật animation walking dựa trên movement state
    /// Chỉ update khi state thực sự thay đổi để tối ưu performance
    /// </summary>
    /// <param name="isMoving">Enemy có đang di chuyển không</param>
    /// <param name="isAttacking">Enemy có đang tấn công không</param>
    /// <param name="isHurt">Enemy có đang bị hurt không</param>
    /// <param name="isKnockedBack">Enemy có đang bị knockback không</param>
    public void updateWalkingAnimation(bool isMoving, bool isAttacking, bool isHurt, bool isKnockedBack)
    {
        if (_animator == null) return;
        
        // Chỉ walking khi thực sự đang di chuyển và không trong special states
        bool shouldWalk = isMoving && !isAttacking && !isHurt && !isKnockedBack;
        
        // Chỉ update khi state thay đổi
        if (shouldWalk != _lastWalkingState)
        {
            _animator.SetBool(AnimationString._enemyIsWalking, shouldWalk); 
            _lastWalkingState = shouldWalk;
        }
    }
    
    /// <summary>
    /// Trigger animation tấn công
    /// </summary>
    public void triggerAttackAnimation()
    {
        if (_animator == null) return;
        _animator.SetTrigger(AnimationString._enemyIsAttack); 
    }
    
    /// <summary>
    /// Trigger animation hurt
    /// </summary>
    public void triggerHurtAnimation()
    {
        if (_animator == null) return;
        _animator.SetTrigger(AnimationString._enemyIsHurt);
    }
    
    /// <summary>
    /// Set dead animation state
    /// </summary>
    /// <param name="isDead">Enemy có chết không</param>
    public void setDeadAnimation()
    {
        if (_animator == null) return;
            _animator.SetTrigger(AnimationString._enemyIsDead);
            Debug.Log("[LeThuyNhan_AnimationManager] Triggering isDead animation");
    }

    /// <summary>
    /// Reset tất cả animation states về mặc định
    /// Dùng khi respawn hoặc init
    /// </summary>
    public void resetAllAnimations()
    {
        if (_animator == null) return;

        _animator.SetBool(AnimationString._enemyIsWalking, false);

        // Reset cache states
        _lastWalkingState = false;

        // Reset trigger để tránh bị stuck anim
        _animator.ResetTrigger(AnimationString._enemyIsAttack);
        _animator.ResetTrigger(AnimationString._enemyIsHurt);
        _animator.ResetTrigger(AnimationString._enemyIsDead);
    }
    
    /// <summary>
    /// Kiểm tra xem animation nào đang chạy
    /// </summary>
    public bool isAnimationPlaying(string stateName)
    {
        if (_animator == null) return false;
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
    }
    
    /// <summary>
    /// Lấy thời gian normalized của animation hiện tại (0-1)
    /// </summary>
    public float getCurrentAnimationTime()
    {
        if (_animator == null) return 0f;
        
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }
    
    /// <summary>
    /// Kiểm tra xem animation hiện tại đã hoàn thành chưa
    /// </summary>
    public bool isCurrentAnimationComplete()
    {
        return getCurrentAnimationTime() >= 1.0f;
    }
    
    #endregion
    
    #region Animation Event Callbacks
    // Các method này sẽ được gọi từ Animation Events trong Unity Animator
    
    /// <summary>
    /// Animation Event: Được gọi khi attack animation đến frame gây damage
    /// </summary>
    public void OnAttackHitFrame()
    {
        LeThuyNhan enemy = GetComponent<LeThuyNhan>();
        if (enemy != null)
        {
            enemy.onAttackHit();
        }
    }
    
    /// <summary>
    /// Animation Event: Được gọi khi attack animation kết thúc
    /// </summary>
    public void OnAttackAnimationComplete()
    {
        LeThuyNhan enemy = GetComponent<LeThuyNhan>();
        if (enemy != null)
        {
            enemy.onAttackFinished();
        }
    }
    
    /// <summary>
    /// Animation Event: Được gọi khi hurt animation kết thúc
    /// </summary>
    public void OnHurtAnimationComplete()
    {
        LeThuyNhan enemy = GetComponent<LeThuyNhan>();
        if (enemy != null)
        {
            enemy.onHurtFinished();
        }
    }
    
    #endregion
}

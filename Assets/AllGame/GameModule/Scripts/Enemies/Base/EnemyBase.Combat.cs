using UnityEngine;
using System.Collections;

public abstract partial class EnemyBase
{
    #region Attack
    /// <summary>
    /// Method attack cơ bản - phải được override ở class con
    /// Mỗi loại enemy sẽ có cách tấn công khác nhau
    /// </summary>
    protected virtual void Attack() { }
    #endregion

    #region Logic Take Damage and Knockback
    /// <summary>
    /// Thiết lập HP mới sau khi nhận damage
    /// Tính toán damage sau khi trừ resistance
    /// </summary>
    /// <param name="damage">Lượng damage gốc</param>
    /// <param name="magic">True = magic damage, False = physical damage</param>
    public void setEnemyHP(float damage, bool magic)
    {
        // Tính damage thực tế sau khi trừ resistance
        float calcDamage = magic ? damage - _magicRes : damage - _physicalRes;
        
        // Damage tối thiểu là 1
        _enemyHP -= Mathf.Max(calcDamage, 1);

        displayDamager(calcDamage, magic); // Hiển thị damage text
        
        // Chết nếu hết máu
        if (_enemyHP <= 0) Die();
    }

    /// <summary>
    /// Phương thức được gọi khi enemy nhận damage
    /// Mặc định sẽ gây knockback, có thể override để thêm logic khác
    /// </summary>
    public virtual void takeDamage() => knockbackFromPlayer();

    /// <summary>
    /// Virtual method để thêm effects khi take damage
    /// </summary>
    protected virtual void OnTakeDamageEffects()
    {
    }

    /// <summary>
    /// Xử lý knockback khi bị tấn công bởi player
    /// Đẩy enemy ra xa player và tạm thời vô hiệu hóa movement
    /// </summary>
    protected void knockbackFromPlayer()
    {
        if (_player == null || _rb == null)
        {
            Debug.LogWarning("[Knockback] Missing player or rigidbody reference");
            return;
        }

        // Kiểm tra xem có đang trong knockback không
        if (_isKnockedBack)
        {
            // Extend knockback duration nếu đang knockback
            if (_knockbackRoutine != null) StopCoroutine(_knockbackRoutine);
            _knockbackRoutine = StartCoroutine(knockbackCoroutine());
            return;
        }

        // Tính hướng knockback (từ player đẩy ra xa)
        Vector2 playerPos = _player.transform.position;
        Vector2 enemyPos = transform.position;
        Vector2 knockbackDir = (enemyPos - playerPos).normalized;

        // Đảm bảo có hướng knockback hợp lệ
        if (knockbackDir.magnitude < 0.1f)
        {
            // Nếu player và enemy quá gần, dùng hướng mặt hiện tại của enemy
            knockbackDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        // Force stop tất cả movement hiện tại
        StopAllMovement();

        // Apply knockback force
        ApplyKnockbackForce(knockbackDir);

        // Bắt đầu knockback coroutine
        if (_knockbackRoutine != null) StopCoroutine(_knockbackRoutine);
        _knockbackRoutine = StartCoroutine(knockbackCoroutine());
    }

    /// <summary>
    /// Dừng tất cả movement và path finding
    /// </summary>
    private void StopAllMovement()
    {
        // Reset velocity về 0
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        // Clear path data
        _path = null;
        _currentWaypoint = 0;

        // Set movement flags
        _isMoving = false;

    }

    private void ApplyKnockbackForce(Vector2 direction)
    {
        // AddForce với Impulse
        _rb.AddForce(direction * _knockbackForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Coroutine xử lý thời gian knockback
    /// Trong thời gian này enemy không thể di chuyển bình thường
    /// </summary>
    protected IEnumerator knockbackCoroutine()
    {
        _isKnockedBack = true;
        float elapsedTime = 0f;

        Debug.Log($"[Knockback] Started coroutine - Duration: {_knockbackDuration}s");

        // Log velocity mỗi frame để debug
        while (elapsedTime < _knockbackDuration)
        {
            elapsedTime += Time.deltaTime;

            // Debug velocity mỗi 0.1 giây
            if (elapsedTime % 0.1f < Time.deltaTime)
            {
                //Debug.Log($"[Knockback] Time: {elapsedTime:F2}s, Velocity: {_rb.linearVelocity}");
            }

            yield return null;
        }

        // Dừng hoàn toàn sau knockback
        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y); // Giữ gravity cho ground enemies

        _isKnockedBack = false;
        _knockbackRoutine = null;

    }
    /// <summary> 
    /// Dừng ngay lập tức knockback nếu cần (ví dụ khi die hoặc respawn)
    /// </summary>
    public void ForceStopKnockback()
    {
        if (_knockbackRoutine != null)
        {
            StopCoroutine(_knockbackRoutine);
            _knockbackRoutine = null;
        }

        _isKnockedBack = false;
    }

    #endregion

}

using UnityEngine;

public abstract partial class EnemyBase
{
       #region Handle Backup    
    /// <summary>
    /// Backup movement system khi pathfinding thất bại
    /// Kích hoạt khi enemy bị "kẹt" không di chuyển được trong thời gian dài
    /// Sẽ di chuyển trực tiếp về phía player để tránh bị stuck
    /// </summary>
    protected void HandleBackupMovementBase()
    {
        // Không cần backup nếu không có player hoặc đang knockback
        if (_player == null|| ShouldStopMovement())
        {
            _stuckTimer = 0f;
            return;
        }

        if (!_isMoving)
        {
            // Tích lũy thời gian đứng im
            _stuckTimer += Time.fixedDeltaTime;
            
            // Nếu đứng im quá lâu và player vẫn trong tầm
            if (_stuckTimer >= _stuckThreshold && _player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, _player.transform.position);
                
                // Chỉ kích hoạt backup khi player trong tầm detection nhưng xa hơn attack range
                if (distanceToPlayer <= _detectionRange && distanceToPlayer > _enemyAttackRange * 1.2f)
                {
                    // Di chuyển trực tiếp về phía player (không qua pathfinding)
                    Vector2 direction = (_player.transform.position - transform.position).normalized;
                    _rb.linearVelocity = new Vector2(direction.x * (_enemyRunSpd * 0.75f), _rb.linearVelocity.y);
                    _isMoving = true;
                    flipToFacePlayer();
                    Debug.Log("[EnemyBase] Backup move towards player (stuck fix)");
                }
            }
        }
        else 
        {
            // Reset timer khi đang di chuyển bình thường
            _stuckTimer = 0f;
        }
    }
    
    /// <summary>
    /// Virtual method để child classes override
    /// Xác định khi nào enemy nên dừng movement (ví dụ: đang attack, exploding, v.v.)
    /// </summary>
    /// <returns>True nếu nên dừng movement</returns>
    protected virtual bool ShouldStopMovement() => false;
    #endregion

}

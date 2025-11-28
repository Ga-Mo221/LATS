using UnityEngine;

public abstract partial class EnemyBase
{
    #region Logic Detect Player
    /// <summary>
    /// Logic chính để phát hiện player
    /// Flying enemy: detect trực tiếp qua EnemyRada
    /// Ground enemy: phải check góc nhìn (trước/sau) và trạng thái ngồi của player
    /// </summary>
    private void detectPlayer()
    {
        // Lấy player từ EnemyRada (có thể null nếu không có player trong tầm)
        GameObject potentialPlayer = (_enemyRada != null && _enemyRada._player != null) ? _enemyRada._player : null;

        if (_canFly) 
        {
            _player = potentialPlayer;
            if (_player != null) 
            {
                if (!_wasPlayerDetected)
                {
                    _wasPlayerDetected = true;
                    PlayerManager.Instance.setPlayerDetected(true); // Gọi khi phát hiện player
                }
            }
            else if (_wasPlayerDetected)
            {
                _wasPlayerDetected = false;
                PlayerManager.Instance.setPlayerDetected(false); // Gọi khi mất player
            }
        }
        else
        {
            // Ground enemy: phải check góc nhìn và logic directional detection
            bool currentlyDetected = isPlayerInDirectionalRange(potentialPlayer);
            
            if (currentlyDetected && !_wasPlayerDetected)
            {
                _player = potentialPlayer;
                _wasPlayerDetected = true;
                PlayerManager.Instance.setPlayerDetected(true);
            }
            else if (!currentlyDetected && _wasPlayerDetected)
            {
                _player = null;
                _wasPlayerDetected = false;
                PlayerManager.Instance.setPlayerDetected(false);
            }
            else if (_isChasing && !currentlyDetected)
            {
                _player = null;
                _wasPlayerDetected = false;
                PlayerManager.Instance.setPlayerDetected(false);
            }
        }
    }

    /// <summary>
    /// Kiểm tra xem player có nằm trong góc nhìn của ground enemy không
    /// </summary>
    /// <param name="potentialPlayer">Player object cần kiểm tra</param>
    /// <returns>True nếu player trong tầm nhìn</returns>
    private bool isPlayerInDirectionalRange(GameObject potentialPlayer)
    {
        if (potentialPlayer == null) return false;

        Vector3 toPlayer = potentialPlayer.transform.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;

        // Nếu quá xa thì không detect
        if (distanceToPlayer > _detectionRange) return false;

        // Xác định hướng mặt của enemy (dựa vào localScale.x)
        Vector2 enemyFacing = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        float angle = Vector2.Angle(enemyFacing, toPlayer.normalized);

        // Kiểm tra góc nhìn phía trước
        if (angle <= _frontDetectionAngle * 0.5f) return true;

        // Kiểm tra detection phía sau
        if (angle > 90f && distanceToPlayer <= _backDetectionRange)
        {
            // Lấy trạng thái player từ PlayerManager
            PlayerState state = PlayerManager.Instance.getCurrentState();

            // Nếu player đang ngồi hoặc đột kích thì KHÔNG phát hiện
            if (state == PlayerState.Sit || 
                state == PlayerState.Sit_Walk || 
                state == PlayerState.Ground_Attack)
            {
                return false;
            }

            // Các trạng thái khác thì bị phát hiện
            return true;
        }
        
        return false;
    }
    #endregion

}

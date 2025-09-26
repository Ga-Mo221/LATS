using UnityEngine;
using System.Collections;

public abstract partial class EnemyBase
{
       #region Handle Movement
    /// <summary>
    /// Chuyển sang chase mode khi detect được player
    /// Reset path cũ và bắt đầu tính path tới player
    /// </summary>
    private void handleChaseMode()
    {
        if (!_isChasing)
        {
            _isChasing = true;
            _path = null; // Reset path cũ
            _currentWaypoint = 0;
            Debug.Log("[EnemyBase] Switch → CHASE");
        }
        moveToPlayer();
    }

    /// <summary>
    /// Chuyển sang patrol mode khi mất player
    /// Reset path cũ và quay về patrol
    /// </summary>
    private void handlePatrolMode()
    {
        if (_isChasing)
        {
            _isChasing = false;
            _path = null; // Reset path cũ
            _currentWaypoint = 0;
            Debug.Log("[EnemyBase] Switch → PATROL");
        }
        Patrol();
    }

    /// <summary>
    /// Logic di chuyển chase player
    /// Tính path tới player và di chuyển theo path
    /// </summary>
    protected virtual void moveToPlayer()
    {
        if (_isKnockedBack) return;
        if (_player == null)
        {
            // Không có player -> dừng movement
            _rb.linearVelocity = Vector2.zero;
            _isMoving = false;
            return;
        }

        // Nếu không có path hoặc đã hết waypoint
        if (_path == null || _currentWaypoint >= _path.vectorPath.Count)
        {
            // Yêu cầu tính path mới nếu seeker rảnh
            if (_seeker.IsDone())
            {
                _seeker.StartPath(_rb.position, _player.transform.position, onPathComplete);
            }

            // Trong lúc chờ path, vẫn quay mặt về player và dừng di chuyển
            flipToFacePlayer();
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            _isMoving = false;
            return;
        }

        // Di chuyển theo path hiện tại
        Vector2 dir = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        
        // Flying enemy di chuyển cả X và Y, ground enemy chỉ di chuyển X
        _rb.linearVelocity = _canFly ? 
            dir * _enemyRunSpd : 
            new Vector2(dir.x * _enemyRunSpd, _rb.linearVelocity.y);

        _isMoving = _rb.linearVelocity.sqrMagnitude > 0.01f;

        // Chuyển sang waypoint tiếp theo khi đến gần
        if (Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]) < _nextWaypointDistance)
            _currentWaypoint++;

        flipToFacePlayer(); // Luôn quay mặt về player
    }

    /// <summary>
    /// Logic patrol qua lại quanh spawn point
    /// Di chuyển từ trái sang phải và ngược lại trong phạm vi _patrolRange
    /// </summary>
    protected virtual void Patrol()
    {
        if (_isKnockedBack) return;
        // Không patrol nếu đang đứng im hoặc component chưa sẵn sàng
        if (!_isPatrol || _isStay || _rb == null || !_seeker.IsDone())
        {
            _isMoving = false;
            return;
        }

        // Nếu không có path hoặc đã hết waypoint
        if (_path == null || _currentWaypoint >= _path.vectorPath.Count)
        {
            // Tính target position dựa vào hướng patrol hiện tại
            Vector3 target = _movingToRight ? 
                _patrolStartPos + Vector3.right * _patrolRange : 
                _patrolStartPos + Vector3.left * _patrolRange;
                
            _seeker.StartPath(_rb.position, target, onPathComplete);
            _isMoving = false;
            return;
        }

        // Di chuyển theo path patrol
        Vector2 dir = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        
        // Flying enemy di chuyển cả X và Y, ground enemy chỉ di chuyển X
        _rb.linearVelocity = _canFly ? 
            dir * _enemyMoveSpd : 
            new Vector2(dir.x * _enemyMoveSpd, _rb.linearVelocity.y);

        _isMoving = _rb.linearVelocity.sqrMagnitude > 0.01f;

        // Chuyển sang waypoint tiếp theo
        if (Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]) < _nextWaypointDistance)
            _currentWaypoint++;

        // Khi hoàn thành path, dừng lại và đổi hướng
        if (_currentWaypoint >= _path.vectorPath.Count)
            StartCoroutine(stopAndTurnAround());

        Flip(); // Flip sprite theo hướng di chuyển
    }

    /// <summary>
    /// Coroutine xử lý việc dừng lại và đổi hướng patrol
    /// Enemy sẽ đứng im một lúc trước khi patrol ngược lại
    /// </summary>
    protected virtual IEnumerator stopAndTurnAround()
    {
        // Chuyển sang trạng thái đứng im
        _isPatrol = false;
        _isStay = true;
        _rb.linearVelocity = Vector2.zero;
        _isMoving = false;
        
        // Đợi một khoảng thời gian
        yield return new WaitForSeconds(_patrolWaitDuration);
        
        // Đổi hướng và tiếp tục patrol
        _movingToRight = !_movingToRight;
        _isStay = false;
        _isPatrol = true;
        _path = null; // Reset path để tính lại
        _currentWaypoint = 0;
    }
    #endregion

    #region Handle Flip
    /// <summary>
    /// Lật mặt enemy để nhìn về phía player
    /// Thay đổi localScale.x để flip sprite
    /// </summary>
    protected void flipToFacePlayer()
    {
        if (_player == null) return;
        
        float dir = _player.transform.position.x - transform.position.x;
        if (Mathf.Abs(dir) < 0.2f) return; // Ignore nếu khoảng cách quá nhỏ
        
        Vector3 scale = transform.localScale;
        scale.x = dir > 0 ? 1 : -1; // Dương = nhìn phải, âm = nhìn trái
        transform.localScale = scale;
    }

    /// <summary>
    /// Flip sprite theo hướng di chuyển (cho patrol)
    /// </summary>
    private void Flip()
    {
        if (_rb.linearVelocity.x > 0.05f) 
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        else if (_rb.linearVelocity.x < -0.05f) 
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
    }
    #endregion

}

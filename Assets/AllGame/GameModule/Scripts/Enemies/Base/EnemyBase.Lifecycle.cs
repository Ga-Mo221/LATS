using UnityEngine;
using System.Collections;

public abstract partial class EnemyBase
{
    #region Die and Respawn
    /// <summary>
    /// Xử lý khi enemy chết
    /// Kiểm tra số lần chết để quyết định respawn hay destroy vĩnh viễn
    /// </summary>
    public virtual void Die()
    {
        _currentDeathCount++;

        if (_currentDeathCount == 1)
        {
            GameObject tui = Instantiate(_itemDropPrefab, transform.position, Quaternion.identity);
            var chest = tui.GetComponent<Chest>();
            foreach (var Items in _possibleDropItems)
            {
                chest.addItems(Items);
            }
        }

        // Xóa sạch pathfinding data
        clearPathCompletely();

        if (_currentDeathCount > _maxDeathCount)
        {
            // Chết vĩnh viễn - destroy object
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.simulated = false;
            this.enabled = false;
            Destroy(gameObject);
            Debug.Log("[EnemyBase] Permanent dead");
        }
        else
        {
            // Chết tạm thời - sẽ respawn
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic; // Không tương tác physics
            _rb.simulated = false;

            // Vô hiệu hóa collider
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            Debug.Log($"[EnemyBase]: {gameObject.name} died and will respawn in {_delaySpawn}");
            StartCoroutine(respawnCoroutine());
        }
    }

    /// <summary>
    /// Coroutine chờ trước khi respawn
    /// </summary>
    private IEnumerator respawnCoroutine()
    {
        yield return new WaitForSeconds(_delaySpawn);
        respawn();
    }

    /// <summary>
    /// Respawn enemy về trạng thái ban đầu
    /// Khôi phục tất cả stats, position và AI state
    /// </summary>
    public virtual void respawn()
    {
        // Xóa sạch pathfinding data
        clearPathCompletely();

        // Khôi phục tất cả stats về giá trị gốc
        _enemyMoveSpd = _defaultStats.moveSpd;
        _enemyRunSpd = _defaultStats.runSpd;
        _enemyHP = _defaultStats.hp;
        _enemyPhysicalDamage = _defaultStats.physicalDamage;
        _enemyMagicDamage = _defaultStats.magicDamage;
        _physicalRes = _defaultStats.physicalRes;
        _magicRes = _defaultStats.magicRes;
        _enemyAttackRange = _defaultStats.attackRange;
        _enemyAttackCooldown = _defaultStats.attackCooldown;
        _detectionRange = _defaultStats.detectionRange;
        _patrolRange = _defaultStats.patrolRange;
        _frontDetectionAngle = _defaultStats.frontDetectionAngle;
        _backDetectionRange = _defaultStats.backDetectionRange;
        _nextWaypointDistance = _defaultStats.nextWaypointDistance;
        _knockbackForce = _defaultStats.knockbackForce;
        _knockbackDuration = _defaultStats.knockbackDuration;
        _patrolWaitDuration = _defaultStats.patrolWaitDuration;

        // Reset tất cả AI state về trạng thái ban đầu
        _isChasing = false;
        _player = null;
        _wasPlayerDetected = false;
        _isMoving = false;
        _isPatrol = true;
        _isStay = false;
        _isKnockedBack = false;
        
        // Khôi phục visual components
        GetComponent<SpriteRenderer>().enabled = true;
        this.enabled = true;

        // Khôi phục physics
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.simulated = true;

        // Bật lại collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        // Về lại vị trí patrol ban đầu
        transform.position = _patrolStartPos;
        this.enabled = true;

        // Khởi động lại path update coroutine
        _updatePathCoroutine = StartCoroutine(updatePathCoroutine());

        Debug.Log($"[EnemyBase]: {gameObject.name} respawn at {transform.position} (deathCount={_currentDeathCount}/{_maxDeathCount})");
    }
    #endregion

}

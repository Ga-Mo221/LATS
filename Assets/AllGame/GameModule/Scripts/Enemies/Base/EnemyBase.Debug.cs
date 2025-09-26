using UnityEngine;

public abstract partial class EnemyBase
{
    #region Debug Gizmo
    /// <summary>
    /// Vẽ gizmos debug trong Scene view
    /// Hiển thị các tầm: attack (đỏ), detection (vàng cho flying, xanh lá cho góc nhìn trước, xanh dương cho phía sau)
    /// </summary>
    public void OnDrawGizmosSelected()
    {
        // Vẽ tầm tấn công (màu đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyAttackRange);

        if (_canFly)
        {
            // Flying enemy: vẽ tầm detection hình tròn đơn giản
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
        }
        else
        {
            // Ground enemy: vẽ góc nhìn directional detection
            Vector3 enemyPos = transform.position;
            Vector2 facing = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            // Vẽ góc nhìn phía trước (màu xanh lá)
            Gizmos.color = Color.green;
            float halfAngle = _frontDetectionAngle * 0.5f;

            // Vẽ cung tròn cho góc nhìn trước
            for (int i = 0; i <= 20; i++)
            {
                float angle = Mathf.Lerp(-halfAngle, halfAngle, i / 20f);
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.forward) * facing;
                Vector3 point = enemyPos + direction * _detectionRange;
                if (i > 0)
                {
                    Vector3 prevDirection = Quaternion.AngleAxis(Mathf.Lerp(-halfAngle, halfAngle, (i - 1) / 20f), Vector3.forward) * facing;
                    Vector3 prevPoint = enemyPos + prevDirection * _detectionRange;
                    Gizmos.DrawLine(prevPoint, point);
                }
                Gizmos.DrawLine(enemyPos, point);
            }

            // Vẽ tầm detection phía sau (màu xanh dương)
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _backDetectionRange);
        }
    }
    #endregion


}

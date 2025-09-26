using UnityEngine;
using System.Collections;
using Pathfinding;

public abstract partial class EnemyBase
{
    #region A* Path
    /// <summary>
    /// Xóa sạch tất cả dữ liệu pathfinding
    /// Dùng khi cần reset hoàn toàn AI movement (ví dụ khi die)
    /// </summary>
    protected void clearPathCompletely()
    {
        _path = null;
        _currentWaypoint = 0;
        _reachedEndOfPath = false;
        
        // Hủy tất cả path request đang pending
        if (_seeker != null) _seeker.CancelCurrentPathRequest();
        
        // Dừng coroutine cập nhật path
        if (_updatePathCoroutine != null)
        {
            StopCoroutine(_updatePathCoroutine);
            _updatePathCoroutine = null;
        }
        
        // Dừng movement ngay lập tức
        if (_rb != null) _rb.linearVelocity = Vector2.zero;

        Debug.Log("[EnemyBase] clearPathCompletely()");
    }
    
    /// <summary>
    /// Callback được gọi khi A* pathfinding tính toán xong path
    /// </summary>
    /// <param name="p">Path được tính toán</param>
    protected virtual void onPathComplete(Path p)
    {
        if (!p.error) // Nếu tính path thành công
        {
            _path = p;
            _currentWaypoint = 0; // Bắt đầu từ waypoint đầu tiên
            _reachedEndOfPath = false;
        }
    }
    
    /// <summary>
    /// Coroutine cập nhật path định kỳ khi chase player
    /// Chạy liên tục và tính path mới mỗi 0.75 giây
    /// </summary>
    private IEnumerator updatePathCoroutine()
    {
        while (true)
        {
            // Chỉ tính path mới khi không bận và có player target
            if (_seeker.IsDone() && _player != null)
            {
                _seeker.StartPath(_rb.position, _player.transform.position, onPathComplete);
            }
            yield return new WaitForSeconds(0.75f); // Cập nhật path mỗi 0.75 giây
        }
    }
    #endregion
}

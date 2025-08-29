using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    [Header("Poison Settings")]
    [SerializeField] private float _poisonDuration = 2f;
    [SerializeField] private float _poisonDamage = 1f;
    [SerializeField] private float _poisonTickInterval = 1f; // Damage mỗi giây
    [SerializeField] private CircleCollider2D _triggerCollider;

    private bool _isActive = false;
    private List<GameObject> _playersInArea = new List<GameObject>();
    private Coroutine _poisonCoroutine; // Lưu reference đến coroutine để có thể stop

    private void Awake()
    {
        // Đảm bảo collider tắt ban đầu
        if (_triggerCollider != null)
            _triggerCollider.enabled = false;
    }

    public void startPoisonEffect()
    {
        // Dừng poison cũ nếu đang chạy
        if (_poisonCoroutine != null)
        {
            StopCoroutine(_poisonCoroutine);
        }
        
        _poisonCoroutine = StartCoroutine(poisonEffectCoroutine());
    }

    /// <summary>
    /// Method mới để force stop poison effect
    /// </summary>
    public void forceStop()
    {
        // Dừng coroutine nếu đang chạy
        if (_poisonCoroutine != null)
        {
            StopCoroutine(_poisonCoroutine);
            _poisonCoroutine = null;
        }
        
        // Cleanup ngay lập tức
        cleanup();
    }

    private IEnumerator poisonEffectCoroutine()
    {
        // Khóa vị trí enemy
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        yield return new WaitForSeconds(0.2f);

        // Kiểm tra xem có bị force stop trong lúc delay không
        if (_poisonCoroutine == null) yield break;

        // Kích hoạt collider và bắt đầu poison effect
        if (_triggerCollider != null)
        {
            _triggerCollider.enabled = true;
        }

        _isActive = true;

        // Kiểm tra player có sẵn trong vùng không
        checkInitialPlayersInArea();

        float elapsed = 0f;
        while (elapsed < _poisonDuration && _poisonCoroutine != null) // Thêm check _poisonCoroutine
        {
            // Kiểm tra trước khi apply damage
            if (_poisonCoroutine == null || !_isActive) yield break;
            
            // Apply damage cho tất cả player trong vùng
            applyPoisonDamageToPlayersInArea();
            
            yield return new WaitForSeconds(_poisonTickInterval);
            elapsed += _poisonTickInterval;
        }

        // Kết thúc poison effect (chỉ khi không bị force stop)
        if (_poisonCoroutine != null)
        {
            cleanup();
            cleanupAndDestroy();
        }
    }

    /// <summary>
    /// Tách cleanup logic thành method riêng
    /// </summary>
    private void cleanup()
    {
        // Tắt poison effect
        _isActive = false;
        
        if (_triggerCollider != null)
            _triggerCollider.enabled = false;
            
        // Clear danh sách player
        _playersInArea.Clear();
    }

    private void checkInitialPlayersInArea()
    {
        if (_triggerCollider == null) return;

        // Tìm tất cả collider trong vùng poison
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position, 
            _triggerCollider.radius
        );

        foreach (Collider2D col in colliders)
        {
            GameObject player = getPlayerFromCollider(col);
            if (player != null && !_playersInArea.Contains(player))
            {
                _playersInArea.Add(player);
            }
        }
    }

    private void applyPoisonDamageToPlayersInArea()
    {
        // Kiểm tra kỹ trước khi apply damage
        if (!_isActive || _poisonCoroutine == null) return;

        // Tạo copy của list để tránh modification during iteration
        List<GameObject> playersToRemove = new List<GameObject>();

        foreach (GameObject player in _playersInArea)
        {
            // Thêm check lần nữa trong loop
            if (!_isActive || _poisonCoroutine == null) break;
            
            if (player == null)
            {
                playersToRemove.Add(player);
                continue;
            }

            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                // Check cuối cùng trước khi gây damage
                if (_isActive && _poisonCoroutine != null)
                {
                    health.takeDamage(0, _poisonDamage, true); // Magic damage = true cho poison
                    Debug.Log($"[Poison] Applied {_poisonDamage} poison damage to {player.name}");
                }
            }
            else
            {
                playersToRemove.Add(player);
            }
        }

        // Loại bỏ player không hợp lệ
        foreach (GameObject player in playersToRemove)
        {
            _playersInArea.Remove(player);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive) return;

        GameObject player = getPlayerFromCollider(collision);
        if (player != null && !_playersInArea.Contains(player))
        {
            _playersInArea.Add(player);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isActive) return;

        GameObject player = getPlayerFromCollider(collision);
        if (player != null && _playersInArea.Contains(player))
        {
            _playersInArea.Remove(player);
        }
    }

    private GameObject getPlayerFromCollider(Collider2D collision)
    {
        // Bỏ qua các object không phải player
        if (collision.name.Contains("Rada") || collision.name.Contains("Enemy"))
            return null;

        // Tìm player object trong hierarchy
        Transform current = collision.transform;
        while (current != null)
        {
            if (current.name == "Player" || current.CompareTag("Player"))
            {
                return current.gameObject;
            }
            current = current.parent;
        }

        return null;
    }

    private void cleanupAndDestroy()
    {        
        // Reset coroutine reference
        _poisonCoroutine = null;

        // Gọi Die() từ PhongHuyetTrung
        PhongHuyetTrung enemy = GetComponent<PhongHuyetTrung>();
        if (enemy != null)
        {
            enemy.Die();
        }
        else
        {
            // Fallback nếu không tìm thấy component
            Destroy(gameObject);
        }
    }
}
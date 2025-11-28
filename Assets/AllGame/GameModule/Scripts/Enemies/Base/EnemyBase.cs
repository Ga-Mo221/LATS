using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

/// <summary>
/// Lớp cơ sở cho tất cả enemy trong game
/// Cung cấp các chức năng: AI movement, pathfinding, attack, take damage, die/respawn
/// </summary>
public abstract partial class EnemyBase : MonoBehaviour
{
    #region Variables
    [Header("Enemy Stats")]
    [SerializeField] protected float _enemyMoveSpd = 1f;        // Tốc độ di chuyển khi patrol
    [SerializeField] protected float _enemyRunSpd;              // Tốc độ di chuyển khi chase player
    [SerializeField] protected float _enemyHP = 10f;            // Máu hiện tại của enemy
    [SerializeField] protected float _enemyPhysicalDamage = 1f; // Sát thương vật lý gây ra
    [SerializeField] protected float _enemyMagicDamage = 1f;    // Sát thương phép thuật gây ra
    [SerializeField] protected float _physicalRes = 0f;         // Kháng sát thương vật lý
    [SerializeField] protected float _magicRes = 0f;            // Kháng sát thương phép thuật
    [SerializeField] protected float _enemyAttackRange = 1.5f;  // Tầm đánh của enemy
    [SerializeField] protected float _enemyAttackCooldown = 1f; // Thời gian hồi chiêu tấn công
    [SerializeField] protected float _detectionRange = 5f;      // Tầm phát hiện player
    [SerializeField] protected float _patrolRange = 3f;         // Phạm vi patrol qua lại
    [SerializeField] protected float _patrolWaitDuration = 2f;  // Thời gian đợi khi đổi hướng patrol
    [SerializeField] protected GameObject _enemyRadaGOB;        // GameObject chứa EnemyRada script
    [SerializeField] protected bool _canFly = false;            // Enemy có thể bay không (ảnh hưởng detection và movement)

    [Header("Respawn System")]
    [SerializeField] protected float _maxDeathCount = 3;        // Số lần chết tối đa trước khi destroy vĩnh viễn
    [SerializeField] protected float _delaySpawn = 3;           // Thời gian delay trước khi respawn
    protected float _currentDeathCount;                         // Số lần đã chết hiện tại

    private EnemyStatsBackup _defaultStats;                     // Backup stats gốc để restore khi respawn

    /// <summary>
    /// Struct lưu trữ tất cả stats gốc của enemy
    /// Dùng để khôi phục lại trạng thái ban đầu khi respawn
    /// </summary>
    private struct EnemyStatsBackup
    {
        public float moveSpd, runSpd, hp, physicalDamage, magicDamage;
        public float physicalRes, magicRes, attackRange, attackCooldown;
        public float detectionRange, patrolRange, frontDetectionAngle, backDetectionRange;
        public float nextWaypointDistance, knockbackForce, knockbackDuration, patrolWaitDuration;
    }
    protected Coroutine _updatePathCoroutine;                   // Coroutine cập nhật path định kỳ

    [Header("Damage Text")]
    [SerializeField] private Transform _canvasDamageText;           // Canvas hiển thị DamageText
    [SerializeField] private Transform _startDamageTextPos;         // Vị trí spawn DamageText
    public GameObject[] _damageTexts = new GameObject[5];           // Bộ nhớ cache DamageText (5 cái)

    [Header("Drop Items")]
    [SerializeField] private GameObject _itemDropPrefab;            // Prefab item drop khi enemy chết
    [SerializeField] private List<Item> _possibleDropItems;         // Danh sách item có thể drop

    [Header("Directional Detection (Ground enemies only)")]
    [SerializeField] protected float _frontDetectionAngle = 90f;    // Góc nhìn phía trước (degrees)
    [SerializeField] protected float _backDetectionRange = 2f;      // Tầm phát hiện phía sau (chỉ khi player không ngồi)

    [Header("Pathfinding")]
    [SerializeField] protected float _nextWaypointDistance = 2f;    // Khoảng cách để chuyển sang waypoint tiếp theo
    protected Path _path;                                           // Path hiện tại từ A* pathfinding
    protected int _currentWaypoint = 0;                             // Index của waypoint hiện tại trong path
    protected bool _reachedEndOfPath = false;                      // Đã đến cuối path chưa

    // Component references
    protected Seeker _seeker;                                       // A* Pathfinding Seeker component
    protected Rigidbody2D _rb;                                      // Rigidbody2D component
    protected Animator _animator;                                   // Animator component
    protected EnemyRada _enemyRada;                                 // EnemyRada script để detect player

    // AI State variables
    protected GameObject _player;                                   // Reference tới player hiện tại được detect
    protected Vector3 _patrolStartPos;                              // Vị trí bắt đầu patrol (spawn point)

    protected bool _isChasing = false;                              // Đang chase player không
    public bool _isMoving = false;                                  // Đang di chuyển không (cho animation)
    public bool _isPatrol = true;                                   // Có được phép patrol không
    public bool _isStay = false;                                    // Đang đứng im không (khi đổi hướng patrol)
    protected bool _isKnockedBack = false;                          // Đang bị knockback không
    protected bool _movingToRight = true;                           // Patrol sang phải hay trái

    protected bool _wasPlayerDetected = false;                      // Player có được detect ở frame trước không (cho ground enemy)

    // Anti-stuck system
    protected float _stuckTimer = 0f;                               // Timer để detect khi enemy bị kẹt
    protected const float _stuckThreshold = 0.3f;                  // Thời gian tối đa đứng im trước khi kích hoạt backup movement

    [Header("Knockback")]
    [SerializeField] protected float _knockbackForce;        // Lực knockback khi bị tấn công
    [SerializeField] protected float _knockbackDuration;     // Thời gian knockback
    protected Coroutine _knockbackRoutine;                          // Coroutine xử lý knockback
    #endregion

    #region Starter
    /// <summary>
    /// Khởi tạo các component cơ bản
    /// Chạy trước Start(), dùng để get component references
    /// </summary>
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _seeker = GetComponent<Seeker>();
        _enemyRada = _enemyRadaGOB.GetComponent<EnemyRada>();
        
        // Cài đặt bán kính detection cho EnemyRada collider
        _enemyRadaGOB.GetComponent<CircleCollider2D>().radius = _detectionRange;
    }

    /// <summary>
    /// Khởi tạo AI state và backup stats gốc
    /// Bắt đầu coroutine cập nhật path
    /// </summary>
    protected virtual void Start()
    {
        // Lưu vị trí spawn làm điểm patrol trung tâm
        _patrolStartPos = transform.position;
        
        // Backup tất cả stats gốc để restore khi respawn
        _defaultStats = new EnemyStatsBackup
        {
            moveSpd = _enemyMoveSpd,
            runSpd = _enemyRunSpd,
            hp = _enemyHP,
            physicalDamage = _enemyPhysicalDamage,
            magicDamage = _enemyMagicDamage,
            physicalRes = _physicalRes,
            magicRes = _magicRes,
            attackRange = _enemyAttackRange,
            attackCooldown = _enemyAttackCooldown,
            detectionRange = _detectionRange,
            patrolRange = _patrolRange,
            frontDetectionAngle = _frontDetectionAngle,
            backDetectionRange = _backDetectionRange,
            nextWaypointDistance = _nextWaypointDistance,
            knockbackForce = _knockbackForce,
            knockbackDuration = _knockbackDuration,
            patrolWaitDuration = _patrolWaitDuration
        };
        
        // Bắt đầu coroutine cập nhật path định kỳ cho chase mode
        _updatePathCoroutine = StartCoroutine(updatePathCoroutine());
    }

    /// <summary>
    /// Update logic mỗi frame
    /// Chủ yếu xử lý AI detection
    /// </summary>
    protected virtual void Update()
    {
        detectPlayer(); // Kiểm tra xem có detect player không
    }

    /// <summary>
    /// FixedUpdate cho physics movement
    /// Xử lý chase/patrol mode và backup movement
    /// </summary>
    protected virtual void FixedUpdate()
    {
        // Chuyển đổi giữa chase và patrol mode
        if (_player != null) handleChaseMode();
        else handlePatrolMode();

        // Kích hoạt backup movement khi bị kẹt (chỉ khi có player)
        if (_player != null && !_isKnockedBack)
            HandleBackupMovementBase();
    }
    #endregion
}
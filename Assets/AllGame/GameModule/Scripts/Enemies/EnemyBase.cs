using UnityEngine;
using System.Collections;
using Pathfinding;
using NaughtyAttributes;
using Unity.Mathematics;

/// <summary>
/// Lớp cơ sở cho tất cả enemy trong game
/// Cung cấp các chức năng: AI movement, pathfinding, attack, take damage, die/respawn
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    #region Variables
    [Header("Enemy Stats")]
    [SerializeField] protected float _enemyMoveSpd = 1f;        // Tốc độ di chuyển khi patrol
    [SerializeField] protected float _enemyRunSpd;              // Tốc độ di chuyển khi chase player
    [SerializeField] protected float _enemyHP = 10f;            // Máu hiện tại của enemy
    [SerializeField] protected float _enemyPhysicalDamage = 1f; // Sát thương vật lý gây ra
    [SerializeField] protected float _enemyMagicDamage = 1f;    // Sát thương phép thuật gây ra
    [SerializeField] protected float _enemyAttackRange = 1.5f;  // Tầm đánh của enemy
    [SerializeField] protected float _enemyAttackCooldown = 1f; // Thời gian hồi chiêu tấn công
    [SerializeField] protected float _detectionRange = 5f;      // Tầm phát hiện player
    [SerializeField] protected float _patrolRange = 3f;         // Phạm vi patrol qua lại
    [SerializeField] protected float _physicalRes = 0f;         // Kháng sát thương vật lý
    [SerializeField] protected float _magicRes = 0f;            // Kháng sát thương phép thuật
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

    [SerializeField] protected float _patrolWaitDuration = 2f;      // Thời gian đợi khi đổi hướng patrol
    [SerializeField] protected float _knockbackForce = 300f;        // Lực knockback khi bị tấn công
    [SerializeField] protected float _knockbackDuration = 0.2f;     // Thời gian knockback
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

     #region Damage Text
    /// <summary>
    /// Tạo mới DamageText instance
    /// </summary>
    private void createDamageText(int index)
    {
        GameObject dmg = Instantiate(GameModule.Instance._damageTextPrefab, _startDamageTextPos.position, Quaternion.identity, _canvasDamageText);
        dmg.SetActive(false);
        _damageTexts[index] = dmg;
    }

    /// <summary>
    /// Hiển thị DamageText khi enemy nhận damage
    /// </summary>
    protected void displayDamager(float damage, bool magic)
    {
        for (int i = 0; i < _damageTexts.Length; i++)
        {
            if (_damageTexts[i] == null)
                createDamageText(i);

            DamageText script = _damageTexts[i].GetComponent<DamageText>();
            if (!script._start)
            {
                _damageTexts[i].SetActive(true);
                script.setDamage(damage, magic, _startDamageTextPos.position);
                script._start = true;
                return;
            }
        }
    }
    #endregion


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
            if (state == PlayerState.Sit || state == PlayerState.Sit_Walk || state == PlayerState.DotKich)
            {
                return false;
            }

            // Các trạng thái khác thì bị phát hiện
            return true;
        }
        
        return false;
    }
    #endregion

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
    /// Xử lý knockback khi bị tấn công bởi player
    /// Đẩy enemy ra xa player và tạm thời vô hiệu hóa movement
    /// </summary>
    protected void knockbackFromPlayer()
    {
        if (_player == null || _rb == null || _isKnockedBack) return;
        
        // Tính hướng knockback (từ player ra xa)
        Vector2 dir = ((Vector2)transform.position - (Vector2)_player.transform.position).normalized;
        
        // Apply knockback force
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(dir * _knockbackForce, ForceMode2D.Impulse);
        
        // Bắt đầu knockback duration
        if (_knockbackRoutine != null) StopCoroutine(_knockbackRoutine);
        _knockbackRoutine = StartCoroutine(knockbackCoroutine());
    }

    /// <summary>
    /// Coroutine xử lý thời gian knockback
    /// Trong thời gian này enemy không thể di chuyển bình thường
    /// </summary>
    protected IEnumerator knockbackCoroutine()
    {
        _isKnockedBack = true;
        yield return new WaitForSeconds(_knockbackDuration);
        _rb.linearVelocity = Vector2.zero; // Dừng hoàn toàn sau knockback
        _isKnockedBack = false;
    }
    #endregion

    #region Die and Respawn
    /// <summary>
    /// Xử lý khi enemy chết
    /// Kiểm tra số lần chết để quyết định respawn hay destroy vĩnh viễn
    /// </summary>
    public virtual void Die()
    {
        _currentDeathCount++;

        // Xóa sạch pathfinding data
        clearPathCompletely();

        if (_currentDeathCount > _maxDeathCount)
        {
            // Chết vĩnh viễn - destroy object
            _animator.SetBool("isDead", true);
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
            _animator.SetBool("isDead", true);
            _rb.linearVelocity = Vector2.zero;
            _rb.bodyType = RigidbodyType2D.Kinematic; // Không tương tác physics
            _rb.simulated = false;
            GetComponent<SpriteRenderer>().enabled = false; // Ẩn sprite

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

        // Reset animator
        _animator.SetBool("isDead", false);

        // Về lại vị trí patrol ban đầu
        transform.position = _patrolStartPos;
        this.enabled = true;

        // Khởi động lại path update coroutine
        _updatePathCoroutine = StartCoroutine(updatePathCoroutine());

        Debug.Log($"[EnemyBase]: {gameObject.name} respawn at {transform.position} (deathCount={_currentDeathCount}/{_maxDeathCount})");
    }
    #endregion

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

    #region Handle Backup    
    /// <summary>
    /// Backup movement system khi pathfinding thất bại
    /// Kích hoạt khi enemy bị "kẹt" không di chuyển được trong thời gian dài
    /// Sẽ di chuyển trực tiếp về phía player để tránh bị stuck
    /// </summary>
    protected void HandleBackupMovementBase()
    {
        // Không cần backup nếu không có player hoặc đang knockback
        if (_player == null || _isKnockedBack || ShouldStopMovement())
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
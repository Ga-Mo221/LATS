using UnityEngine;

public class PlayerColliderState : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D _collider;
    [SerializeField] private CapsuleCollider2D _colliderMaterial;

    void Start()
    {
        if (!_collider)
            Debug.LogError("[PlayerColiderState] Chưa có 'CapsuleCollider2D'");

        if (!_colliderMaterial)
            Debug.LogError("[PlayerColiderState] Chưa có 'CapsuleCollider2D Material'");
    }

    void Update()
    {
        PlayerState _state = PlayerManager.Instance.getCurrentState();
        bool _isGrounded = PlayerManager.Instance._anim.getBoolGround();
        if (!_isGrounded)
        {
            // _colidderMaterial
            _colliderMaterial.offset = new Vector2(_colliderMaterial.offset.x, -4.589088f);
            _colliderMaterial.size = new Vector2(_colliderMaterial.size.x, 4.394943f);

            // _collider
            _collider.offset = new Vector2(-0.112596f, -4.524124f);
            _collider.size = new Vector2(2.162621f, 4.617755f);
        }
        else
        {
            if (_state == PlayerState.Sit || _state == PlayerState.Sit_Walk)
            {
                // _colidderMaterial
                _colliderMaterial.offset = new Vector2(_colliderMaterial.offset.x, -4.589088f);
                _colliderMaterial.size = new Vector2(_colliderMaterial.size.x, 4.394943f);

                // _collider
                _collider.offset = new Vector2(-0.112596f, -4.524124f);
                _collider.size = new Vector2(2.162621f, 4.617755f);
            }
            else if (_state == PlayerState.Dash)
            {
                // _colidderMaterial
                _colliderMaterial.offset = new Vector2(_colliderMaterial.offset.x, -5.016832f);
                _colliderMaterial.size = new Vector2(_colliderMaterial.size.x, 3.539455f);

                // _collider
                _collider.offset = new Vector2(-0.112596f, -4.877737f);
                _collider.size = new Vector2(2.162621f, 3.910528f);
            }
            else
            {
                // _colidderMaterial
                _colliderMaterial.offset = new Vector2(_colliderMaterial.offset.x, -2.156269f);
                _colliderMaterial.size = new Vector2(_colliderMaterial.size.x, 9.260582f);

                // _collider
                _collider.offset = new Vector2(-0.112596f, -2.057978f);
                _collider.size = new Vector2(2.162621f, 9.550046f);
            }
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class AttackMovement : StateMachineBehaviour
{
    public bool _moveX = true;
    public float _movePowerX = 3f;
    public bool _moveY = false;
    public float _movePowerY = 0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Lấy Rigidbody từ GameObject nhân vật
        Rigidbody2D rb = animator.GetComponent<Rigidbody2D>();

        if (animator.gameObject.CompareTag("Player"))
            PlayerManager.Instance.setCanAttack(false);

        if (rb != null)
        {
            // Xác định hướng mặt nhân vật
            float direction = animator.transform.localScale.x > 0 ? 1 : -1;
            if (_moveX)
                rb.linearVelocity = new Vector2(_movePowerX * direction, rb.linearVelocity.y);
            if (_moveY)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, _movePowerY);
        }
    }
}

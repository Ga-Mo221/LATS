using UnityEngine;

public class PlayerRada : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerManager.Instance._enemyHitBoxDirection = (int)Mathf.Sign(collision.transform.position.x - transform.position.x);
    }
}

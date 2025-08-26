using UnityEngine;

public class TestTakeDamage : MonoBehaviour
{
    public int _id;
    public int _damage;
    public bool _magic;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance._playerHealth.takeDamage(_id, _damage, _magic);
        }
    }
}

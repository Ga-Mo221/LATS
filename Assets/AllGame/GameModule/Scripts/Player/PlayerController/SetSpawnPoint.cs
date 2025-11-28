using UnityEngine;

public class SetSpawnPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.CompareTag("Player"))
        {
            PlayerManager.Instance._respawnPoint = transform.position;
        }
    }
}

using Unity.VisualScripting;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [Header("Knockback")]
    public int AttackID;

    [Header("Bonus Damage %")]
    public float BonusDamagePercent = 0f;

    private float _damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        float baseDamage = PlayerManager.Instance._start._physicalDamage;

        _damage = baseDamage * (1 + BonusDamagePercent / 100f);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"[{gameObject.name}] [BasicAttack] Damage = {_damage}");
            // EnemyHealth _enemyHealth = collision.GetComponent<EnemyHealth>();
            // if (_enemyHealth != null)
            // {
            //     _enemyHealth.takeDamage(PlayerManager.Instance.Stats.getPhysicDamage(), false);
            //     PlayerManager.Instance.setMana(10, true);
            // }
        }
    }
}

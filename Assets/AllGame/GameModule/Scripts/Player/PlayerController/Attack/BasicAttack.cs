using Unity.VisualScripting;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    [Header("Knockback")]
    public int AttackID;

    [Header("Bonus Damage %")]
    public float BonusDamagePercent = 0f;

    [Header("Bounus Mana")]
    public float _bounusmana = 10f;

    [Header("Đột Kích")]
    public bool _dotKich = false;

    private float _damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        float baseDamage = PlayerManager.Instance._start.getPhysicDamage();
        _damage = baseDamage * (1 + BonusDamagePercent / 100f);

        if (_dotKich && PlayerManager.Instance.getPlayerDetected())
        {
            _damage = baseDamage;
        }

        if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log($"[{gameObject.name}] [BasicAttack] Damage = {_damage}");
                EnemyHealth _enemyHealth = collision.GetComponent<EnemyHealth>();
                if (_enemyHealth != null)
                {
                    _enemyHealth.takeDamage(PlayerManager.Instance._start.getPhysicDamage(), false);
                    PlayerManager.Instance.setMana(_bounusmana, true);
                }
            }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private PlayerAnimation _anim;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Transform _canvasDamageText;
    [SerializeField] private Transform _startDamagetTextPos;

    public GameObject[] _damageTexts = new GameObject[10];

    private float _flyPower;

    void Start()
    {
        if (!_rb)
            Debug.LogError("[PlayerHealth] Chưa có 'Rigibody2D'");
        if (!_anim)
            Debug.LogError("[PlayerHealth] Chưa có 'PlayerAnimation'");
        if (!_playerController)
            Debug.LogError("[PlayerHealth] Chưa có 'PlayerController'");
        if (!_canvasDamageText)
            Debug.LogError("[PlayerHealth] Chưa có '_canvasDamageText'");
        if (!_startDamagetTextPos)
            Debug.LogError("[PlayerHealth] Chưa có '_startDamagetTextPos'");
    }


    #region Take Damage
    public void takeDamage(int _id, float _damage, bool _magic)
    {
        switch (_id)
        {
            case 1:
                _flyPower = 8f;
                _anim.setIntegerKnockBackID(1);
                break;
            case 2:
                _flyPower = 20f;
                _anim.setIntegerKnockBackID(2);
                break;
            case 3:
                _flyPower = 30f;
                _anim.setIntegerKnockBackID(2);
                break;
            default:
                _flyPower = 0f;
                _anim.setIntegerKnockBackID(0);
                break;
        }
        float damage = PlayerManager.Instance._start.takeDamage(_damage, _magic);
        displayDamager(damage, _magic);
        if (checkIsALive())
        {
            if (_flyPower == 0f) return;
            handleKnockback();
        }
    }
    #endregion


    #region Create Damage Text
    private void createDamageText(int index)
    {
        GameObject _damage = Instantiate(GameModule.Instance._damageTextPrefab, _startDamagetTextPos.position, Quaternion.identity, _canvasDamageText);
        _damage.SetActive(false);
        _damageTexts[index] = _damage;
    }
    #endregion


    #region DisplayDamager
    private void displayDamager(float damage, bool magic)
    {
        for (int i = 0; i < 10; i++)
        {
            if (_damageTexts[i] == null)
                createDamageText(i);

            DamageText _script = _damageTexts[i].GetComponent<DamageText>();
            if (!_script._start)
            {
                _damageTexts[i].SetActive(true);
                _script.setDamage(damage, magic, _startDamagetTextPos.position);
                _script._start = true;
                return;
            }
        }
    }
    #endregion


    #region Check Is Alive
    private bool checkIsALive()
    {
        if (PlayerManager.Instance._start._currentHealth <= 0)
        {
            PlayerManager.Instance._start._currentHealth = 0;
            PlayerManager.Instance.setIsAlive(false);
            PlayerManager.Instance.setCurrentState(PlayerState.Die);
            PlayerManager.Instance._start.lifeCount(false);
            if (!_anim.getBoolGround())
                _rb.linearVelocity = Vector2.zero;
            return false;
        }
        else
            return true;
    }
    #endregion


    #region Respawn In Animation
    public void respawn()
    {
        if (PlayerManager.Instance._start._currentLifeCount > 0)
        {
            StartCoroutine(revive());
            return;
        }
        StartCoroutine(restartGame());
    }
    #endregion


    #region Respawn Player
    private IEnumerator revive()
    {
        yield return new WaitForSeconds(1.5f);
        PlayerManager.Instance._start.resetHealthAndMana();
        PlayerManager.Instance.setIsAlive(true);
        Vector3 _point = PlayerManager.Instance._respawnPoint;
        if (_point != Vector3.zero || _point != null)
        {
            _rb.linearVelocity = Vector2.zero;
            transform.position = _point;
        }
        PlayerManager.Instance.setCurrentState(PlayerState.Idle);
    }
    #endregion


    #region Game Over
    private IEnumerator restartGame()
    {
        yield return new WaitForSeconds(2f);
        // Gọi màn hình game over
        GameManager.Instance._gameOver.SetActive(true);
    }
    #endregion


    #region handleKnockback
    private void handleKnockback()
    {
        bool _canMove = _anim.getBoolCanMove();
        if (!_playerController._isDashing && _canMove)
        {
            int _direction = PlayerManager.Instance._enemyHitBoxDirection;
            if (_direction != 0)
            {
                transform.localScale = new Vector3(_direction, 1, 1);

                PlayerManager.Instance.setKnoked(true);
                _rb.linearVelocity = Vector2.zero;
                float y = _flyPower < 10 ? _rb.linearVelocity.y : _flyPower;
                _rb.linearVelocity = new Vector2(_flyPower * _direction * -1, y);
            }
        }
    }
    #endregion


    #region Reset Knocked
    public void resetKnocked()
    {
        PlayerManager.Instance.setKnoked(false);
        _anim.setIntegerKnockBackID(0);
    }
    #endregion
    }

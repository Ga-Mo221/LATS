using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        if (!_anim)
            Debug.LogError("[PlayerAnimation] Chưa gán 'Animator'");
    }


    #region Update Bool Is A Live
    public void updateBoolIsALive(bool amount)
        => _anim.SetBool(AnimationString._isAlive, amount);
    #endregion


    #region Update Bool Knocked
    public void updateBoolKnocked(bool amount)
        => _anim.SetBool(AnimationString._knocked, amount);
    #endregion


    #region Set Integer KnockBackID
    public void setIntegerKnockBackID(int amount)
        => _anim.SetInteger(AnimationString._knockBackID, amount);
    #endregion


    #region Bool Ground
    public void setBoolGround(bool amount)
        => _anim.SetBool(AnimationString._isGround, amount);
    public bool getBoolGround()
        => _anim.GetBool(AnimationString._isGround);
    #endregion


    #region Bool CanMove
    public bool getBoolCanMove() => _anim.GetBool(AnimationString._canMove);

    public void setBoolCanMove(bool amount) => _anim.SetBool(AnimationString._canMove, amount);
    #endregion


    #region Set Bool IsMove
    public void setBoolIsMove(bool isMove, bool isRunning)
    {
        _anim.SetBool(AnimationString._isMove, isMove);
        _anim.SetBool(AnimationString._isRunning, isRunning);
    }
    #endregion


    #region Set Float Y Velocity
    public void setFloatYVelocity(float amount)
        => _anim.SetFloat(AnimationString._yVelocity, amount);
    #endregion


    #region Set Trigger Jumping
    public void setTriggerJumping()
        => _anim.SetTrigger(AnimationString._isJumping);
    #endregion


    #region Set Bool Sitting
    public void setBoolSitting(bool amount)
        => _anim.SetBool(AnimationString._isSitting, amount);
    #endregion


    #region Set Trigger Dashing
    public void setTriggerDashing()
        => _anim.SetTrigger(AnimationString._isDashing);
    #endregion


    #region Integer Weapon Type
    public void setIntegerWeaponType(int amount)
        => _anim.SetInteger(AnimationString._weaponType, amount);

    public int getIntegerWeaponType()
        => _anim.GetInteger(AnimationString._weaponType);
    #endregion


    #region Set Trigger Attack
    public void setTriggerAttack()
        => _anim.SetTrigger(AnimationString._isAttack);
    #endregion


    #region Set Bool Player Detected
    public void setBoolPlayerDetected(bool amount)
        => _anim.SetBool(AnimationString._isDetected, amount);
    #endregion
}

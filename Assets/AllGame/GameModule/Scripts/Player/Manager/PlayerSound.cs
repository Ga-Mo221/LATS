using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public void dashing()
    {
        SoundManager.Instance.PlayDashAudio();
    }

    public void move()
    {
        SoundManager.Instance.PlayMoveAudio();
    }

    public void attack1()
    {
        SoundManager.Instance.PlayAttackAudio(1);
    }

    public void attack2()
    {
        SoundManager.Instance.PlayAttackAudio(2);
    }

    public void attack3()
    {
        SoundManager.Instance.PlayAttackAudio(3);
    }

    public void attack4()
    {
        SoundManager.Instance.PlayAttackAudio(4);
    }

    public void jump1()
    {
        SoundManager.Instance.PlayJumpAudio(1);
    }

    public void jump2()
    {
        SoundManager.Instance.PlayJumpAudio(2);
    }

    public void tiepdat()
    {
        SoundManager.Instance.PlayJumpAudio(3);
    }

    public void Hurt()
    {
        SoundManager.Instance.HurtAudio();
    }

}

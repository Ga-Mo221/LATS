using TMPro;
using UnityEngine;

public class UITest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerStates;

    void Update()
    {
        if (_playerStates)
        {
            switch (PlayerManager.Instance.getCurrentState())
            {
                case PlayerState.Idle:
                    _playerStates.text = "State : IDLE";
                    //Debug.Log("State : IDLE");
                    break;
                case PlayerState.Walk:
                    _playerStates.text = "State : WALKING";
                    //Debug.Log("State : WALKING");
                    break;
                case PlayerState.Run:
                    _playerStates.text = "State : RUNNING";
                    //Debug.Log("State : RUNNING");
                    break;
                case PlayerState.Dash:
                    _playerStates.text = "State : DASHING";
                    //Debug.Log("State : DASHING");
                    break;
                case PlayerState.Sit:
                    _playerStates.text = "State : SITTING";
                    //Debug.Log("State : SITTING");
                    break;
                case PlayerState.Sit_Walk:
                    _playerStates.text = "State : SIT_WALK";
                    //Debug.Log("State : SIT_WALK");
                    break;
                case PlayerState.Jump:
                    _playerStates.text = "State : JUMPING";
                    //Debug.Log("State : JUMPING");
                    break;
                case PlayerState.Ground_Attack:
                    _playerStates.text = "State : ATTACK_GROUND";
                    //Debug.Log("State : ATTACK_GROUND");
                    break;
                case PlayerState.Air_Attack:
                    _playerStates.text = "State : ATTACK_AIR";
                    //Debug.Log("State : ATTACK_AIR");
                    break;
                case PlayerState.Knocked:
                    _playerStates.text = "State : KNOCKED";
                    //Debug.Log("State : KNOCKED");
                    break;
                case PlayerState.Die:
                    _playerStates.text = "State : DIE";
                    //Debug.Log("State : DIE");
                    break;
            }
        }
    }
}

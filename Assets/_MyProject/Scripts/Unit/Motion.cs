using System.Collections;
using UnityEngine;

public class Motion : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private StateManager _state;
    [SerializeField] private Animator _animator;

    private void Update()
    {
        switch (_state.UnitState.Value)
        {
            case EUnitState.Free:
                _animator.SetBool("CanJump", _state.CanJump.Value);
                break;
        }
    }

    public void SetState(int state, EUnitState setState = EUnitState.None)
    {
        if(setState != EUnitState.None)
            _state.UnitState.Value = setState;
        _animator.SetTrigger("Action");
        _animator.SetInteger("State", state);
    }
}
